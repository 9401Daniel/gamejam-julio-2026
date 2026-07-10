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
    // Guarda la última tecla de movimiento presionada.
    private Key currentMoveKey = Key.None;
    private GameManager gameManager;
    private Enemy enemyManager;
    public GameObject spitPrefab;
    public GameObject enemigoPrefab;
    [SerializeField] public string powerup = "BOOM";
    public bool tieneAccion = true;
    public bool crearPocion;
    [SerializeField] public bool starPowerup;
    [SerializeField] public bool hasCoolDown;
    public bool exploto = false;
    public float abilityCoolDown;
    public float starDuration;
    public Vector3[] trampillasCharco = new Vector3[3];
    public Vector3 posInicial = new Vector3(-12, -6, 0);

    public SpawnManagerX spawnManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManagerX>();
        starDuration = 2.0f;
        abilityCoolDown = 4.0f;
        tieneAccion = false;
        hasCoolDown = false;
        trampillasCharco[0] = new Vector3(-11f, 7f, 0f);
        trampillasCharco[1] = new Vector3(2.03f, 7.05f, 0f);
        trampillasCharco[2] = new Vector3(15.05f, -0.98f, 0f);
        transform.position = posInicial;
    }

    void OnEnable()
    {
        // Activa la acción para que empiece a "escuchar" el input del teclado.

        moveAction.Enable();
    }

    /*
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

        if (Input.GetKeyDown(KeyCode.Space) && tieneAccion)
        {
            Debug.Log("¿Que hara esto?");
            UsarHabilidad();
        }
    }
    */

    void Update()
    {
        // Lee el valor configurado en moveAction.
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Obtiene el Animator sin agregar una nueva variable a la clase.
        Animator animator = GetComponent<Animator>();

        // Direction:
        // 0 = Idle
        // 1 = Arriba
        // 2 = Abajo
        // 3 = Izquierda
        // 4 = Derecha
        int direction = animator.GetInteger("Direction");

        if (Keyboard.current != null)
        {
            // La última flecha presionada tiene prioridad.
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                direction = 1;
            }

            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                direction = 2;
            }

            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                direction = 3;
            }

            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                direction = 4;
            }

            // Mantiene la última dirección mientras esa flecha siga presionada.
            if (direction == 1 && Keyboard.current.upArrowKey.isPressed)
            {
                input = Vector2.up;
            }
            else if (direction == 2 && Keyboard.current.downArrowKey.isPressed)
            {
                input = Vector2.down;
            }
            else if (direction == 3 && Keyboard.current.leftArrowKey.isPressed)
            {
                input = Vector2.left;
            }
            else if (direction == 4 && Keyboard.current.rightArrowKey.isPressed)
            {
                input = Vector2.right;
            }
            else
            {
                // Si se soltó la última flecha, busca otra que continúe presionada.
                if (Keyboard.current.upArrowKey.isPressed)
                {
                    input = Vector2.up;
                    direction = 1;
                }
                else if (Keyboard.current.downArrowKey.isPressed)
                {
                    input = Vector2.down;
                    direction = 2;
                }
                else if (Keyboard.current.leftArrowKey.isPressed)
                {
                    input = Vector2.left;
                    direction = 3;
                }
                else if (Keyboard.current.rightArrowKey.isPressed)
                {
                    input = Vector2.right;
                    direction = 4;
                }
                else
                {
                    input = Vector2.zero;
                    direction = 0;
                }
            }
        }
        else
        {
            input = Vector2.zero;
            direction = 0;
        }

        // Actualiza la animación con la misma dirección del movimiento.
        animator.SetInteger("Direction", direction);

        // Movimiento únicamente horizontal o vertical.
        // Nunca permite desplazamiento diagonal.
        transform.position +=
            (Vector3)input * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && tieneAccion)
        {
            Debug.Log("¿Que hara esto?");
            UsarHabilidad();
        }
    }

    private void UsarHabilidad()
    {
        switch (powerup)
        {
            case "DASH":
                if (!hasCoolDown)
                {
                    StartCoroutine("StarCooldown");
                    StartCoroutine("PowerUpCooldown");
                }
                break;
            case "BOOM":
                if (!hasCoolDown)
                {
                    StartCoroutine("PowerUpCooldown");
                    StartCoroutine(BOOMCooldown());
                }
                break;
            case "ENEMY": // Pocion azul - azul
                if (!hasCoolDown)
                {
                    StartCoroutine("PowerUpCooldown");
                    CrearMonstruo();
                }
                break;
            case "1UP":
                if (!hasCoolDown)
                {
                    StartCoroutine("PowerUpCooldown");
                    VidaExtra();
                }
                break;
            case "SPIT":
                if (!hasCoolDown)
                {
                    StartCoroutine("PowerUpCooldown");
                    CrearCharco();
                }
                break;
            case "TP": // Pocion verde - azul 
                if (!hasCoolDown)
                {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Choque con algo");
        if (collision.gameObject.CompareTag("Enemy") && starPowerup)
        {
            Debug.Log("Adios mounstruo");
            spawnManager.OnPlayerHit();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Recibi daño");
            gameManager.RestarHP();
            spawnManager.OnPlayerHit();
        }
    }

    //Se habilita la accion del jugador siempre y cuando cuente con 2 pociones
    public void crearEfecto()
    {
        if (crearPocion)
        {
            powerup = gameManager.MezclarPociones();
            crearPocion = false;
            tieneAccion = true;
        }
    }

    IEnumerator StarCooldown()
    {
        starPowerup = true;
        yield return new WaitForSeconds(starDuration);
        starPowerup = false;
    }

    IEnumerator PowerUpCooldown()
    {
        hasCoolDown = true;
        yield return new WaitForSeconds(abilityCoolDown);
        hasCoolDown = false;
    }

    /*
    IEnumerator BOOMCooldown()
    {
        yield return new WaitForSeconds(1.0f);
        exploto = false;
    }
    */

    IEnumerator BOOMCooldown()
    {
        // Activa la explosión, resta la vida y vacía el inventario.
        Explosion();

        // Espera un frame para que todos los enemigos detecten
        // que exploto es true y ejecuten Destroy.
        yield return null;

        // Desactiva inmediatamente la explosión para que los
        // enemigos nuevos no sean destruidos.
        exploto = false;

        // Espera otro frame para asegurarse de que Unity
        // haya terminado de destruir a todos los enemigos.
        yield return null;

        // Si ya no quedan enemigos, genera la siguiente ronda.
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            spawnManager.SpawnEnemy(spawnManager.round);
            spawnManager.round++;
        }
    }

    /*
    private void Explosion()
    {
        exploto = true;
        gameManager.RestarHP();
        gameManager.VaciarInventario();
        //enemyManager.Explosion();
        transform.position = posInicial;
    }
    */

    private void Explosion()
    {
        exploto = true;
        gameManager.RestarHP();
        gameManager.VaciarInventario();
        //enemyManager.Explosion();
        transform.position = posInicial;
    }

    private void CrearMonstruo()
    {
        Instantiate(enemigoPrefab, trampillasCharco[0], enemigoPrefab.transform.rotation);
        gameManager.VaciarInventario();
    }

    public void Teleport()
    {
        transform.position = posInicial;
    }

    private void VidaExtra()
    {
        gameManager.RestaurarHP();
    }

    private void CrearCharco()
    {
        Instantiate(spitPrefab, trampillasCharco[0], spitPrefab.transform.rotation);
        Instantiate(spitPrefab, trampillasCharco[1], spitPrefab.transform.rotation);
        Instantiate(spitPrefab, trampillasCharco[2], spitPrefab.transform.rotation);

    }

    public void InvocarEnemigo()
    {
        gameManager.VaciarInventario();
    }
}