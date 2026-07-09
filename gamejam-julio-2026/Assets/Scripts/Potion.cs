using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public string colorPocion="";
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
         // Checa si el otro collider tiene un componente de controlador de jugador
        if (other.GetComponent<Player_Controller>() != null) {
            
            // Destruye la pocion
            Destroy(gameObject);
            gameManager.ActualizarPociones(colorPocion);
        }
    }
}
