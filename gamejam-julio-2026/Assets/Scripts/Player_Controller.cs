using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class Player_Controller : MonoBehaviour
{
    // Velocidad de movimiento del jugador (Se puede ajustar desde el inspector)
    public float speed = 5f;

    // Acción de input definida directamente aquí 
    // En el Inspector, este campo se expande para configurar manualmente
    // los bindings (teclas) usando un "2D Vector Composite" (Up/Down/Left/Right).
    public InputAction moveAction;
    private GameManager gameManager;
    public SpawnManagerX spawnManager; // referencia al spawn manager, para avisarle cuando el jugador recibe daño
    private string powerup = "BOOM"; 
    public bool tieneAccion = true;
    public bool crearPocion;
    private bool starPowerup;
    private bool hasCoolDown;
    private int starDaño = 10;
    public float abilityCoolDown;
    public float starDuration;
    public Vector3 posInicial = new Vector3(-12,-6,0);

    void Start(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManagerX>();
        starDuration = 2.0f;
        tieneAccion = false;
        hasCoolDown = false;
    }

    void OnEnable()
    {
        // Activa la acción para que empiece a "escuchar" el input del teclado.
    
        moveAction.Enable();
    }

    void Update()
    {
        // Lee el valor actual del input como un Vector2 (x = horizontal, y = vertical).
        // Gracias al "2D Vector Composite", presionar una tecla ya devuelve
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Aquí comparamos qué eje tiene mayor magnitud (valor absoluto) y anulamos el otro,
        // para evitar que el jugador se mueva en diagonal si presiona dos teclas a la vez.
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            // Hay más movimiento horizontal que vertical entonces anula el vertical
            input.y = 0f;
        }
        else
        {
            // Hay más movimiento vertical o empate, entonces -> anula el horizontal
            input.x = 0f;
        }

        // normalized asegura que la magnitud del vector sea siempre 1,
        // para que la velocidad de movimiento sea consistente en cualquier dirección.
        // Time.deltaTime hace que el movimiento sea independiente de los FPS del juego,
        // para que se mueva a la misma velocidad sin importar qué tan rápido corra el juego.
        transform.position += (Vector3)input.normalized * speed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space) && tieneAccion)
        {
            Debug.Log("¿Que hara esto?");
            UsarHabilidad();
        }
    }

    private void UsarHabilidad(){
        switch (powerup)
        {
            case "DASH":
                if(!hasCoolDown){
                    StartCoroutine("StarCooldown");
                    StartCoroutine("PowerUpCooldown");
                }
                break;
            case "BOOM":
                if(!hasCoolDown){
                    StartCoroutine("PowerUpCooldown");
                    Explosion();
                }
                break;
            case "ENEMY":
                break;
            case "1UP":
                if(!hasCoolDown){
                    StartCoroutine("PowerUpCooldown");
                    VidaExtra();
                }
                break;
            case "SPIT":
                if(!hasCoolDown){
                    StartCoroutine("PowerUpCooldown");
                    Debug.Log("Hola");
                }
                break;
            case "TP":
                if(!hasCoolDown){
                    StartCoroutine("PowerUpCooldown");
                    Teleport();
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Se detecto un evento trigger");
        // Detiene el movimiento del objeto cuando toca a otro sprite
        // Esto evita que lo atraviese
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Detiene el movimiento lineal y angular
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        Debug.Log("Choque con algo");

        // Solo reacciona si realmente chocó con algo etiquetado "Enemy".
        if (collision.gameObject.CompareTag("Enemy")){

            // Obtiene el componente Enemy directamente del objeto con el que se chocó
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (starPowerup && enemy != null){
                Debug.Log("Adios mounstruo");
                enemy.RecibirGolpe(starDaño);
            } else {
                Debug.Log("Recibi daño");
                gameManager.RestarHP();
                spawnManager.OnPlayerHit(); // dispara nueva oleada y reposiciona al jugador
            }
        }
    }

    //Se habilita la accion del jugador siempre y cuando cuente con 2 pociones
    public void crearEfecto(){
        if(crearPocion){
            powerup=gameManager.MezclarPociones();
            crearPocion = false;
            tieneAccion = true;
        }
    }

    IEnumerator StarCooldown(){
        starPowerup=true;
        yield return new WaitForSeconds(starDuration);
        starPowerup=false;
    }

    IEnumerator PowerUpCooldown(){
        hasCoolDown=true;
        yield return new WaitForSeconds(abilityCoolDown);
        hasCoolDown=false;
    } 

    private void Explosion(){
        gameManager.RestarHP();
        gameManager.VaciarInventario();
        transform.position = posInicial;
    }

    public void Teleport(){
        transform.position = posInicial;
    }

    private void VidaExtra(){
        gameManager.RestaurarHP();
    }

    public void InvocarEnemigo(){
        gameManager.VaciarInventario();
    }
}