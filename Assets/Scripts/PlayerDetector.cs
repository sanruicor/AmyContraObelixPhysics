using UnityEngine;
using UnityEngine.Events;

public class PlayerDetector : MonoBehaviour
{
    // Eventos de Unity para determinar la entrada y salida del jugador del área de detección.
    public UnityEvent playerEnter;
    public UnityEvent playerExit;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Invocar el evento PlayerEnter
            playerEnter.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Invocar el evento PlayerExit
            playerExit.Invoke();
        }
    }
}
