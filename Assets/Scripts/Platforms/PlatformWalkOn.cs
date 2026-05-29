using UnityEngine;

public class PlatformWalkOn : MonoBehaviour
{
    private CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            transform.parent = other.transform;
            cc.skinWidth = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            transform.parent = null;
            cc.skinWidth = 0.08f;   //! Aunque no lo restaures, el CharacterController se comporta como si lo hicieras
        }
    }
}
