using UnityEngine;

public class DoorLinearMovement : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [Tooltip ("Tiempo para completar un ciclo completo de ida y vuelta")]
    [SerializeField] private float travelTime;
    [SerializeField]  private float initialPhase;
    private float movingTime;
    [SerializeField] private bool isMoving;

    void Start()
    {
        // Para evitar problemas de división por 0, le ponemos valor de 1 a travelTime
        if (travelTime == 0)
        {
            travelTime = 1f;
        }
        
        movingTime = travelTime * initialPhase;
    }

    void Update()
    {
        if (isMoving)
        {
            movingTime += Time.deltaTime;
            // Calculamos el valor de t que debemos usar para la interpolación lineal
            float t = Mathf.SmoothStep(0f, 1f, myPingPong(movingTime/travelTime));
            // Calculamos la nueva posición de la puerta interpolando entre las dos posiciones extremas del movimiento
            Vector3 newPosition = Vector3.Lerp(startPoint.position, endPoint.position, t);
            transform.position = newPosition;
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

    private float myPingPong(float t)
    {
        return Mathf.PingPong(t*2f, 1f);
    }
}
