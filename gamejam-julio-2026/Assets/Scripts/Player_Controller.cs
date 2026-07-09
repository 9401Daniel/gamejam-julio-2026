using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Velocidad de movimiento del jugador (ajustable desde el Inspector)
    public float speed = 5f;

    // Acción de input definida directamente aquí (no en un asset externo).
    // En el Inspector, este campo se expande para configurar manualmente
    // los bindings (teclas) usando un "2D Vector Composite" (Up/Down/Left/Right).
    public InputAction moveAction;

    void OnEnable()
    {
        // Activa la acción para que empiece a "escuchar" el input del teclado.
        // Sin esto, ReadValue() siempre devolvería (0,0) aunque se presionen teclas.
        moveAction.Enable();
    }

    void Update()
    {
        // Lee el valor actual del input como un Vector2 (x = horizontal, y = vertical).
        // Gracias al "2D Vector Composite", presionar una tecla ya devuelve
        // directamente algo como (1,0) para derecha, (0,1) para arriba, etc.
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Bomberman se mueve solo en 4 direcciones, nunca en diagonal.
        // Aquí comparamos qué eje tiene mayor magnitud (valor absoluto) y anulamos el otro,
        // para evitar que el jugador se mueva en diagonal si presiona dos teclas a la vez.
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            // Hay más movimiento horizontal que vertical -> anula el vertical
            input.y = 0f;
        }
        else
        {
            // Hay más movimiento vertical (o empate) -> anula el horizontal
            input.x = 0f;
        }

        // normalized asegura que la magnitud del vector sea siempre 1,
        // para que la velocidad de movimiento sea consistente en cualquier dirección.
        // (Vector3) convierte el Vector2 a Vector3, porque transform.position es un Vector3.
        // Time.deltaTime hace que el movimiento sea independiente de los FPS del juego,
        // para que se mueva a la misma velocidad sin importar qué tan rápido corra el juego.
        transform.position += (Vector3)input.normalized * speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
{
    // Detiene el movimiento del objeto cuando toca a otro sprite
    // Esto evita que lo atraviese basándote en la lógica de tus scripts de movimiento
    Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
    

    if (rb != null)
        {
            // Detiene el movimiento lineal y angular
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
}
}