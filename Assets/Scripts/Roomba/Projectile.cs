using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private float shotForce = 20f;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        
    }

    public void Shot(Vector3 direction)
    {
        rb.AddForce(direction * shotForce, ForceMode.Impulse);
    }
}
