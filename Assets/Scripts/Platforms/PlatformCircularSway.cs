using UnityEngine;

public class PlatformCircularSway : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform rotationCenter;
    [SerializeField] private float speed;
    private float movementAmplitudeAngle;
    Vector3 centerToStartPoint;
    Vector3 centerToEndPoint;

    void Start()
    {
        transform.position = startPoint.position;
        centerToStartPoint = startPoint.position - rotationCenter.position;
        centerToEndPoint = endPoint.position - rotationCenter.position;
        movementAmplitudeAngle = Vector3.Angle(centerToStartPoint, centerToEndPoint);
    }

    void Update()
    {
        transform.RotateAround(rotationCenter.position, Vector3.up, speed * Time.deltaTime);

        Vector3 centerToPlatform = transform.position - rotationCenter.position;

        float platformAngle;
        if (speed > 0)
            platformAngle = Vector3.Angle(centerToStartPoint, centerToPlatform);
        else
            platformAngle = Vector3.Angle(centerToEndPoint, centerToPlatform);
        
        if (platformAngle > movementAmplitudeAngle)
        {
            speed = -speed;
        }
    }
}
