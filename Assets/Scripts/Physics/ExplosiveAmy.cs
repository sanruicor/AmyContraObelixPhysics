using UnityEngine;
using UnityEngine.InputSystem;

public class ExplosiveAmy : MonoBehaviour
{
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce;


    void Start()
    {
        
    }

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            MakeExplosion();
        }
    }

    void MakeExplosion()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hit, 20))
        {
            Collider[] collidersFound = Physics.OverlapSphere(hit.point, explosionRadius);

            foreach (Collider c in collidersFound)
            {
                Rigidbody rb = c.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, hit.point, explosionRadius, 1, ForceMode.Impulse);
                }
            }
        }
    }
}
