using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Velocidad de movimiento del enemigo (ajustable desde el Inspector)
    public float speed = 0.0f;

    // Referencia al Transform del jugador, para saber hacia dónde moverse.
    // Se asigna una sola vez en Start(), no en cada frame, por eficiencia.
    private Transform player;

    [SerializeField] private int puntosSalud = 5;

    private Player_Controller playerController;

    public SpawnManagerX spawnManagerx;

    public int countEnemies;

    // Variables para detectar paredes y hacer parpadear el sprite.
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D enemyCollider;
    private Coroutine parpadeoCoroutine;
    private Color colorOriginal;
    private bool estaDentroDePared;

    [SerializeField] private float alfaParpadeo = 0.2f;
    [SerializeField] private float velocidadParpadeo = 0.15f;

    void Start()
    {
        // Busca en la escena el GameObject que tenga el tag "Player" y guarda su Transform.
        // El jugador debe tener ese tag asignado.
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerController =
            GameObject.Find("Doctor").GetComponent<Player_Controller>();

        spawnManagerx =
            GameObject.Find("SpawnManager").GetComponent<SpawnManagerX>();

        // Obtiene el SpriteRenderer del enemigo.
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        // Obtiene el BoxCollider2D del enemigo.
        enemyCollider = GetComponent<BoxCollider2D>();

        // Guarda el color original para restaurarlo al salir de la pared.
        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }
    }

    void Update()
    {
        // Calcula el vector que va desde el enemigo hacia el jugador
        // (posición del jugador menos la posición del enemigo).
        // .normalized deja solo la dirección (magnitud 1), sin importar qué tan lejos esté.
        Vector2 direction =
            (player.position - transform.position).normalized;

        // Mueve al enemigo en esa dirección, a velocidad constante,
        // multiplicando por Time.deltaTime para que sea independiente de los FPS.
        transform.position +=
            (Vector3)direction * speed * Time.deltaTime;

        // Se valida que el jugador tenga la acción de explosión
        // y se esté activando por primera vez.
        if (playerController.exploto)
        {
            Debug.Log("*Pulverizado por bomba*");
            EliminateEnemy();
        }

        // Detecta si el enemigo está atravesando una pared.
        DetectarPared();
    }

    private void DetectarPared()
    {
        if (enemyCollider == null)
        {
            return;
        }

        // Calcula la posición real del centro del BoxCollider2D.
        Vector2 centroCollider =
            transform.TransformPoint(enemyCollider.offset);

        // Calcula el tamaño real tomando en cuenta la escala.
        Vector2 tamañoCollider = new Vector2(
            enemyCollider.size.x * Mathf.Abs(transform.lossyScale.x),
            enemyCollider.size.y * Mathf.Abs(transform.lossyScale.y)
        );

        // Obtiene todos los colliders que se superponen con el enemigo.
        Collider2D[] collidersDetectados =
            Physics2D.OverlapBoxAll(
                centroCollider,
                tamañoCollider,
                transform.eulerAngles.z
            );

        bool detectoPared = false;

        foreach (Collider2D colliderDetectado in collidersDetectados)
        {
            // Ignora el collider del propio enemigo.
            if (colliderDetectado == enemyCollider)
            {
                continue;
            }

            // Solo detecta objetos con el tag "Pared".
            if (colliderDetectado.CompareTag("Pared"))
            {
                detectoPared = true;
                break;
            }
        }

        // Comienza el parpadeo al entrar en una pared.
        if (detectoPared && !estaDentroDePared)
        {
            estaDentroDePared = true;

            if (parpadeoCoroutine == null)
            {
                parpadeoCoroutine =
                    StartCoroutine(ParpadearEnPared());
            }
        }
        // Detiene el parpadeo al salir de todas las paredes.
        else if (!detectoPared && estaDentroDePared)
        {
            estaDentroDePared = false;

            if (parpadeoCoroutine != null)
            {
                StopCoroutine(parpadeoCoroutine);
                parpadeoCoroutine = null;
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.color = colorOriginal;
            }
        }
    }

    private IEnumerator ParpadearEnPared()
    {
        while (estaDentroDePared)
        {
            if (spriteRenderer != null)
            {
                Color colorTransparente = colorOriginal;
                colorTransparente.a = alfaParpadeo;
                spriteRenderer.color = colorTransparente;
            }

            yield return new WaitForSeconds(velocidadParpadeo);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = colorOriginal;
            }

            yield return new WaitForSeconds(velocidadParpadeo);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorOriginal;
        }

        parpadeoCoroutine = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Tag: " + collision.gameObject.tag);

        if (
            collision.gameObject.CompareTag("Player") &&
            playerController.starPowerup
        )
        {
            Debug.Log("Aaaaaa");
            EliminateEnemy();
        }
        else if (collision.gameObject.CompareTag("Spit"))
        {
            Debug.Log("Me derritooo");
            EliminateEnemy();
        }
        else
        {
            Debug.Log("Dañe al jugador");
        }
    }

    public void Explosion()
    {
        EliminateEnemy();
    }

    /*
    private void EliminateEnemy()
    {
        Destroy(gameObject);
        Debug.Log(GameObject.FindGameObjectsWithTag("Enemy").Length);

        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 )
        {
            spawnManagerx.SpawnEnemy(spawnManagerx.round);
            spawnManagerx.round++;
        }
    }
    */

    private void EliminateEnemy()
    {
        // Unity todavía cuenta al enemigo actual porque Destroy
        // se ejecuta completamente al finalizar el frame.
        int enemyCount =
            GameObject.FindGameObjectsWithTag("Enemy").Length;

        Debug.Log("Enemigos antes de eliminar: " + enemyCount);

        // Si solamente queda este enemigo, genera la siguiente ronda.
        if (enemyCount <= 1)
        {
            spawnManagerx.SpawnEnemy(spawnManagerx.round);
            spawnManagerx.round++;
        }
        Destroy(gameObject);
    }
}