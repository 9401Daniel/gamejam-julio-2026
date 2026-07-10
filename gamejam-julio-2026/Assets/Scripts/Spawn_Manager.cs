using UnityEngine;
using TMPro;

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

    private int round = 1;

    void Start()
    {
        SpawnEnemy();
        UpdateRoundText();
    }

    Vector2 GenerateSpawnPosition()
    {
        float xPos = spawnCenter.x + Random.Range(-spawnRangeX, spawnRangeX);
        float yPos = spawnCenter.y + Random.Range(-spawnRangeY, spawnRangeY);
        return new Vector2(xPos, yPos);
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);

        Enemy enemyController = newEnemy.GetComponent<Enemy>();
        if (enemyController != null)
        {
            enemyController.speed = baseSpeed + (speedBoost * (round - 1));
        }
    }

    public void OnPlayerHit()
    {
        round++;
        SpawnEnemy();
        ResetPlayerPosition();
        UpdateRoundText();
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
}