using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicPlatform : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float springConstant;
    private Rigidbody rb;
    private Vector3 startPosition;
    private int cubeCount;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            GameObject cube = Instantiate(cubePrefab);
            cube.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.4f + 0.5f * cubeCount, Random.Range(-0.5f, 0.5f));
            cubeCount++;
        }
    }

    void FixedUpdate()
    {
        rb.AddForce((startPosition - transform.position) * springConstant, ForceMode.Force);
    }
}
