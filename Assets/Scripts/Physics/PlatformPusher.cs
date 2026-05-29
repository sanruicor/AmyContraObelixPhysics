using UnityEngine;

public class PlatformPusher : MonoBehaviour
{
    [SerializeField] Vector3 startPush;


        void Start()
    {
        transform.position += startPush;
    }

    void Update()
    {
        
    }
}
