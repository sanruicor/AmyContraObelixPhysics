using UnityEngine;
using UnityEngine.InputSystem;

public class AmyMovement : MonoBehaviour
{
    [Header ("Next Level")]
    [SerializeField] private Transform nextLevelDoor;
    [SerializeField] private float nextLevelDoorDetectionRange = 3f;
    private bool autopilot = false;
    private bool autopilotStop = false;

    [Header ("Settings")]
    public CharacterController controller;
    private enum PlayerState { Idle, Run, Jump, Fall, Push }
    private PlayerState playerState;
    private float playerSpeed = 5.0f;
    private float pushSpeedPenalty = 0.65f;
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;

    private float verticalSpeed;
    private bool groundedPlayer;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference interactionAction;

    private Transform cameraTransform;
    private Animator animator;
    private Transform pickableObject;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Este es el equivalente a activar/habilitar la asignación del input action al proyecto
        moveAction.action.Enable();
        jumpAction.action.Enable();
        interactionAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        interactionAction.action.Disable();
    }

    void Update()
    {
        if (autopilot)
        {
            Debug.Log("Amy in autopilot");
            Autopilot();
            return;
        }
        autopilot = CheckForNextLevelDoor();

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
        {
            // Slight downward velocity to keep grounded stable
            if (verticalSpeed < -2f)
            {
                verticalSpeed = -2f;
            }

        }
        else
        {
            Debug.Log("Amy is falling");
        }

        // Read input
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        // Guardamos en una variable la longitud del vector move original, para restaurarla después de la transformación/proyección
        // horizontal que hacemos para orientarlo en la dirección de la cámara
        float moveMagnitude = Mathf.Clamp(move.magnitude, 0f, 1f);
        // Transformamos la dirección del vector para que se refiera al transform de la cámara
        // "Hacemos que Amy se mueva en la dirección de la cámara pasando las coordenadas locales de la cámara a las coordenadas del mundo"
        move = cameraTransform.TransformDirection(move);
        // Proyectamos el vector en un plano horizontal, por el simple método de poner a 0 la coordenada y.
        move.y = 0;
        //Restauramos la longitud original del vector move
        move = move.normalized * moveMagnitude;

        if (move != Vector3.zero)
        {
            transform.forward = move;
            if (groundedPlayer)
            {
                ChangeState(PlayerState.Run);
            }
        }
        else
        {
            if (groundedPlayer)
            {
                ChangeState(PlayerState.Idle);
            }
        }

        // Jump using WasPressedThisFrame()
        if (groundedPlayer && jumpAction.action.WasPressedThisFrame())
        {
            verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            ChangeState(PlayerState.Jump);
            // Si iniciamos el salto Amy ya no está en el suelo.
            // Esto no sirve para evitar la transición a Push si Amy tiene un Pushable delante
            groundedPlayer = false;
        }

        // Apply gravity
        verticalSpeed += gravityValue * Time.deltaTime;

        if (verticalSpeed < 0 && !groundedPlayer)
        {
            ChangeState(PlayerState.Fall);
            groundedPlayer = false;
        }

        Pushable p = CheckForPushable();

        // Calculamos la velocidad horizontal de Amy. El caso más sencillo es que esté sin ningún obketo pushable
        // delante, en este caso se aplica playerSpeed
        float appliedPlayerSpeed = playerSpeed;
        // Si hay un objeto pushable delante el comportamiento será distinto si Amy está en el suelo, o si está en el aire
        if (p != null)
        {
            if (groundedPlayer)
            {
                appliedPlayerSpeed = playerSpeed * pushSpeedPenalty;
                if (move != Vector3.zero)
                {
                    ChangeState(PlayerState.Push);
                }
            }
            else
            {
                appliedPlayerSpeed = 0f;
            }

            p.Push(move * appliedPlayerSpeed * Time.deltaTime);
        }

        if (interactionAction.action.WasPressedThisFrame())
        {
            ProcessInteraction();
        }

        // Move
        Vector3 playerVelocity = move * appliedPlayerSpeed + Vector3.up * verticalSpeed;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private bool CheckForNextLevelDoor()
    {
        //? Si el portal está desactivado o no tenemos su referencia, hay que devolver false sin ninguna comprobación
        if (nextLevelDoor == null || !nextLevelDoor.gameObject.activeSelf)
        {
            return false;
        }
        return Vector3.Distance(transform.position, nextLevelDoor.position) < nextLevelDoorDetectionRange;
    }

    private void Autopilot()
    {
        if (autopilotStop)
        {
            return;
        }
        Vector3 movementDirection = (nextLevelDoor.transform.position - transform.position).normalized;
        movementDirection.y = 0;
        movementDirection = movementDirection.normalized;
        Vector3 playerVelocity = movementDirection * playerSpeed + Vector3.up * verticalSpeed;
        controller.Move(playerVelocity * Time.deltaTime);

        //* Orientamos a Amy hacia la dirección de movimiento, suavizando el giro con Vector3.Lerp
        transform.forward = Vector3.Lerp(transform.forward, movementDirection, 0.01f);

        if (Vector3.Distance(nextLevelDoor.transform.position, transform.position) < 0.05f)
        {
            autopilotStop = true;
            ChangeState(PlayerState.Idle);
            // Llamamos al GameManager para que inicie el cambio de nivel
            GameManager.instance.StartLevelChange();
        }
    }

    private void ProcessInteraction()
    {
        if (pickableObject == null)
        {
            pickableObject = CheckForPickable();
            if (pickableObject != null)
            {
                pickableObject.parent = transform;  //* Hacemos el objeto hijo de Amy
                pickableObject.localPosition = new Vector3(0f, 1f, 0.5f); //* Lo posicionamos localmente respecto a Amy
                pickableObject.localRotation = Quaternion.identity; //* Horientamos la caja respecto a Amy
                pickableObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else
        {
            pickableObject.parent = null;
            pickableObject.GetComponent<Rigidbody>().isKinematic = false;
            pickableObject = null;
        }
    }

    private Transform CheckForPickable()
    {
        Transform t = null;

        Ray r = new Ray(transform.position + Vector3.up * 0.7f, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 0.5f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag("Pickable"))
            {
                t = hit.collider.transform;
            }
        }
        else
        {
            r = new Ray(transform.position + Vector3.up * 0.35f, transform.forward);
            if (Physics.Raycast(r, out hit, 0.5f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.CompareTag("Pickable"))
                {
                    t = hit.collider.transform;
                }
            }
        }

        return t;
    }

    private Pushable CheckForPushable()
    {
        Pushable p = null;

        Ray r = new Ray(transform.position + Vector3.up, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 0.30f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            p = hit.collider.GetComponent<Pushable>();
        }

        return p;
    }

    private void ChangeState(PlayerState newState)
    {
        if (newState == playerState)
        {
            return;
        }

        // Limpiamos los triggers que hayan podido quedar acumulados
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("Fall");
        animator.ResetTrigger("Push");

        playerState = newState;
        switch (playerState)
        {
            case PlayerState.Idle:
                animator.SetTrigger("Idle");
                break;
            case PlayerState.Run:
                animator.SetTrigger("Run");
                break;
            case PlayerState.Jump:
                animator.SetTrigger("Jump");
                break;
            case PlayerState.Fall:
                animator.SetTrigger("Fall");
                break;
            case PlayerState.Push:
                animator.SetTrigger("Push");
                break;
        }
    }
}
