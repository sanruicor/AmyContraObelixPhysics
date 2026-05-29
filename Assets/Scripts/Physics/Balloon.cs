using UnityEngine;
using UnityEngine.InputSystem;

public class Balloon : MonoBehaviour
{
    float buoyancyForce = 20f;
    float buoyancyLoss = 0.01f;
    float buoyancyGain = 0.03f;
    Rigidbody rb;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float netBuoyancyChange;

        if (Keyboard.current.spaceKey.isPressed)
        {
            netBuoyancyChange = buoyancyGain * Time.deltaTime;    
        }
        else
        {
            netBuoyancyChange = -buoyancyLoss * Time.deltaTime;
        }

        buoyancyForce += netBuoyancyChange;
        if (buoyancyForce < 0)
        {
            buoyancyForce = 0;
        }
        rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Force);
    }
}
