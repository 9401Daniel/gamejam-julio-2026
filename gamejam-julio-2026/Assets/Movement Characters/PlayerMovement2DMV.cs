using UnityEngine;

// Descomenta esta línea si utilizarás el NUEVO Input System.
// using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2DMV : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Referencias")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private Vector2 movement;

    // 0 = Idle
    // 1 = Arriba / W
    // 2 = Abajo / S
    // 3 = Izquierda / A
    // 4 = Derecha / D
    private int direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        // =========================================================
        // SISTEMA VIEJO DE INPUTS
        // Déjalo activo si utilizas Input Manager.
        // =========================================================
        ReadOldInput();


        // =========================================================
        // NUEVO INPUT SYSTEM
        // Comenta ReadOldInput() y descomenta ReadNewInput().
        // También descomenta:
        // using UnityEngine.InputSystem;
        // =========================================================
        // ReadNewInput();


        animator.SetInteger("Direction", direction);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;

        // Para versiones anteriores de Unity:
        // rb.velocity = movement * moveSpeed;
    }

    private void ReadOldInput()
    {
        // La última tecla presionada tiene prioridad.

        if (Input.GetKeyDown(KeyCode.W))
        {
            SetDirection(Vector2.up, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SetDirection(Vector2.down, 2);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SetDirection(Vector2.left, 3);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SetDirection(Vector2.right, 4);
        }

        UpdateHeldKeysOldInput();
    }

    private void UpdateHeldKeysOldInput()
    {
        // Si la tecla correspondiente a la dirección actual
        // continúa presionada, mantiene esa dirección.

        switch (direction)
        {
            case 1:
                if (Input.GetKey(KeyCode.W))
                    return;
                break;

            case 2:
                if (Input.GetKey(KeyCode.S))
                    return;
                break;

            case 3:
                if (Input.GetKey(KeyCode.A))
                    return;
                break;

            case 4:
                if (Input.GetKey(KeyCode.D))
                    return;
                break;
        }

        // Cuando se suelta la última tecla presionada,
        // busca otra tecla que todavía siga presionada.

        if (Input.GetKey(KeyCode.W))
        {
            SetDirection(Vector2.up, 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            SetDirection(Vector2.down, 2);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            SetDirection(Vector2.left, 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            SetDirection(Vector2.right, 4);
        }
        else
        {
            StopMovement();
        }
    }

    /*
    private void ReadNewInput()
    {
        if (Keyboard.current == null)
        {
            StopMovement();
            return;
        }

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            SetDirection(Vector2.up, 1);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            SetDirection(Vector2.down, 2);
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            SetDirection(Vector2.left, 3);
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            SetDirection(Vector2.right, 4);
        }

        UpdateHeldKeysNewInput();
    }

    private void UpdateHeldKeysNewInput()
    {
        switch (direction)
        {
            case 1:
                if (Keyboard.current.wKey.isPressed)
                    return;
                break;

            case 2:
                if (Keyboard.current.sKey.isPressed)
                    return;
                break;

            case 3:
                if (Keyboard.current.aKey.isPressed)
                    return;
                break;

            case 4:
                if (Keyboard.current.dKey.isPressed)
                    return;
                break;
        }

        if (Keyboard.current.wKey.isPressed)
        {
            SetDirection(Vector2.up, 1);
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            SetDirection(Vector2.down, 2);
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            SetDirection(Vector2.left, 3);
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            SetDirection(Vector2.right, 4);
        }
        else
        {
            StopMovement();
        }
    }
    */

    private void SetDirection(Vector2 newMovement, int newDirection)
    {
        movement = newMovement;
        direction = newDirection;
    }

    private void StopMovement()
    {
        movement = Vector2.zero;
        direction = 0;
    }
}