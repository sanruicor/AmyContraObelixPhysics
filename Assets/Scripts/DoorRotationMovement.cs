using UnityEngine;

public class DoorRotationMovement : MonoBehaviour
{
    [Tooltip ("Velocidad de rotación, positiva en el sentido de las agujas del reloj, negativa en el contrario")]
    [SerializeField] private float rotationSpeed;
    private bool isMoving;

    void Start()
    {
        
    }

    void Update()
    {
        if (isMoving)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    public void StartMovement()
    {
        isMoving = true;
    }

    public void StopMovement()
    {
        isMoving = false;
    }
}
