using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Velocidad de movimiento del enemigo (ajustable desde el Inspector)
    public float speed = 1f;

    // Referencia al Transform del jugador, para saber hacia dónde moverse.
    // Se asigna una sola vez en Start(), no en cada frame, por eficiencia.
    private Transform player;
    [SerializeField] private int puntosSalud = 5;
    private Player_Controller playerController;

    void Start()
    {
        // Busca en la escena el GameObject que tenga el tag "Player" y guarda su Transform.
        // El jugador debe tener ese tag asignado.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = GameObject.Find("Doctor").GetComponent<Player_Controller>();
    }

    void Update()
    {
        // Calcula el vector que va desde el enemigo hacia el jugador
        // (posición del jugador menos la posición del enemigo).
        // .normalized deja solo la dirección (magnitud 1), sin importar qué tan lejos esté.
        Vector2 direction = (player.position - transform.position).normalized;

        // Mueve al enemigo en esa dirección, a velocidad constante,
        // multiplicando por Time.deltaTime para que sea independiente de los FPS.
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        //Se valida que el jugador tenga la accion de esplosion y se este activando por primera vez
        if(playerController.exploto)
        {
            Debug.Log("*Pulverizado por bomba*");
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision){
        Debug.Log("Tag: "+collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Player") && playerController.starPowerup){
            Debug.Log("Aaaaaa");
            Destroy(gameObject);
        } else if(collision.gameObject.CompareTag("Spit")){
            Debug.Log("Me derritooo");
            Destroy(gameObject);
        }else{
            Debug.Log("Dañe al jugador");
        }
    }

    public void Explosion(){
        Destroy(gameObject);
    }
}