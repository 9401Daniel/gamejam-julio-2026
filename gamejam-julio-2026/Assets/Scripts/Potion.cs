
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Potion : MonoBehaviour
{
    public string colorPocion="";
    private GameManager gameManager;
    private PotionSpawner potionSpawner;

    public AudioSource audioSourcePickUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        potionSpawner = GameObject.Find("PotionSpawner").GetComponent<PotionSpawner>();
        audioSourcePickUp = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other) {
        
         // Checa si el otro collider tiene un componente de controlador de jugador
        if (other.GetComponent<Player_Controller>() != null) {

            audioSourcePickUp.Play();
            // Destruye la pocion
            Vector3 posicionPocima = gameObject.transform.position;
            Destroy(gameObject);
            potionSpawner.LiberarPosicion(posicionPocima);
            gameManager.ActualizarPociones(colorPocion);
        }
    }



}
