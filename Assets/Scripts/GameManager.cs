using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject[] fruitPrefabs;
    [SerializeField] Transform player;
    [SerializeField] Light mainLight;
    [SerializeField] GameObject nexLevelDoor;
    private int totalScore;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        totalScore = 0;
        nexLevelDoor.SetActive(false);
        StartCoroutine(SpawnFruit());
    }

    void Update()
    {

    }

    private IEnumerator SpawnFruit()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            if (Random.value < 0.05f)
            {
                Vector3 spawnPoint = new Vector3(Random.Range(-8f, 8f), 1f, Random.Range(-8f, 8f));
                while (Vector3.Distance(spawnPoint, player.position) < 4f) // Comprobar que esté a más de 4m
                {
                    spawnPoint = new Vector3(Random.Range(-8f, 8f), 1f, Random.Range(-8f, 8f));
                }

                Instantiate(fruitPrefabs[Random.Range(0, fruitPrefabs.Length)], spawnPoint, Quaternion.identity);
            }
        }
    }

    public void AddPoints(int points)
    {
        totalScore += points;
        Debug.Log("[GameManager] totalScore " + totalScore);

        if (totalScore > 1000)
        {
            nexLevelDoor.SetActive(true);
        }
    }

    public void StartLevelChange()
    {
        StartCoroutine(FadeLightOut());
    }

    private IEnumerator FadeLightOut()
    {
        while (mainLight.intensity > 0.02f)
        {
            // Disminuir la intensidad de la luz en 0.002f
            mainLight.intensity -= 0.002f;
            yield return new WaitForSeconds(0.01f);
        }
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene("AmyYLasPlataformas");
    }
}
