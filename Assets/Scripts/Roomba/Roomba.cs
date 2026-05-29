using UnityEngine;

public class Roomba : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject projectilePrefab;
    private AudioSource audioSource;
    [SerializeField] AudioClip hitAudioClip;
    private float speed = 3f;
    private float minDistance = 3f;
    private float minShotDistance = 5f;
    private int health = 4;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance  > minDistance)
        {
            Vector3 movementDirection = GetMovementDirection();
            transform.position += movementDirection * speed * Time.deltaTime;
        }

        if (playerDistance < minShotDistance)
        {
            if (Random.value < 0.002f)
            {
                Shot();
            }
        }
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        return direction.normalized;
    }

    private Vector3 GetShotDirection()
    {
        Vector3 direction = player.position + Vector3.up * 1.1f - transform.position;
        return direction.normalized;
    }

    private void Shot()
    {
        Projectile projectile = Instantiate(projectilePrefab, transform.position + Vector3.up * 0.1f, Quaternion.identity).GetComponent<Projectile>();
        projectile.Shot(GetShotDirection());
    }

    public void Hit()
    {
        Debug.Log("[Roomba] Hit");
        audioSource.PlayOneShot(hitAudioClip);
        health--;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
