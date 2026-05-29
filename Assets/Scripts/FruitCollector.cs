using UnityEngine;

public class FruitCollector : MonoBehaviour
{
    private float collectionDistance = 0.8f;
    AudioSource audioSource;
    [SerializeField] AudioClip fruitCollectedClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // para ponernos a 1m del suelo, no a la altura de los pies de Amy usamos transform.position + Vector3.up
        Collider[] foundColliders = Physics.OverlapSphere(transform.position + Vector3.up, collectionDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        if (foundColliders != null)
        {
            foreach (Collider c in foundColliders)
            {
                // Buscamos el componente Fruit en el objeto al que pertenece el colisionador
                // Para comprobar que sea una fruta, y porque si lo es lo para saber los puntos
                Fruit fruit = c.GetComponent<Fruit>();

                if (fruit)
                {
                    audioSource.PlayOneShot(fruitCollectedClip);
                    GameManager.instance.AddPoints(fruit.points);
                    Destroy(c.gameObject);
                }
            }
        }
    }
}
