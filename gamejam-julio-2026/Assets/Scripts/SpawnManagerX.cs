using UnityEngine;
using TMPro;
using System.Collections;

public class SpawnManagerX : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject enemyPrefab;
    public GameObject player;                 // Objeto "Doctor"
    public Transform respawnPoint;             // Lugar donde se hacen las pociones
    public TextMeshProUGUI gameText;           // opcional, si se agrega UI de texto
    [Header("Zona de spawn (ajustar según el mapa)")]
    public Vector2 spawnCenter = Vector2.zero;
    public float spawnRangeX = 5f;
    public float spawnRangeY = 3f;

    [Header("Dificultad")]
    public float baseSpeed = 3f;      // velocidad inicial del enemigo (ronda 1)
    public float speedBoost = 0.5f;   // Cuanto aumenta la velocidad por cada ronda

    public int round = 1;

    public int remplazo = 1;

    public GameManager gameManager;

    void Start()
    {
        SpawnEnemy(round);
        UpdateRoundText();
    }

    Vector2 GenerateSpawnPosition()
    {
        float xPos = spawnCenter.x + Random.Range(-spawnRangeX, spawnRangeX);
        float yPos = spawnCenter.y + Random.Range(-spawnRangeY, spawnRangeY);
        return new Vector2(xPos, yPos);
    }

    public void SpawnEnemy(int enemigos)
    {
        for (int n = 0; n < enemigos; n++)
        {
            Debug.Log("numero de enemigo: " + n);
            GameObject newEnemy = Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }

        /*Enemy enemyController = newEnemy.GetComponent<Enemy>();
        if (enemyController != null)
        {
            enemyController.speed = baseSpeed + (speedBoost * (round - 1));
        }
        */
    }

    public void OnPlayerHit()
    {
        if (gameManager.gameOver == false)
        {
            StartCoroutine("IncrementoControlado");
        }
    }

    void ResetPlayerPosition()
    {
        player.transform.position = respawnPoint.position;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    void UpdateRoundText()
    {
        if (gameText != null)
        {
            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            gameText.text = "Ronda " + round + " - Enemigos activos: " + enemyCount;
        }
    }

    IEnumerator IncrementoControlado()
    {
        Debug.Log(remplazo);
        SpawnEnemy(remplazo);
        ResetPlayerPosition();

        yield return new WaitForSeconds(2.0f);

        UpdateRoundText();
        remplazo++;
    }
}