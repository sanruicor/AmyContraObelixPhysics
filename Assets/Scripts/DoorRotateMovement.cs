using UnityEngine;

public class DoorRotateMovement : MonoBehaviour
{
    [SerializeField] private bool isLeftDoor;
    [SerializeField] private float angularSpeed; 
    private bool isMoving;

    void Start()
    {
        
    }

    void Update()
    {
        if (isMoving)
        {
            if (isLeftDoor)
            {
                transform.Rotate(Vector3.up * angularSpeed * Time.deltaTime);
            }
            else
            {
                transform.Rotate(Vector3.down * angularSpeed * Time.deltaTime);
            }
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
