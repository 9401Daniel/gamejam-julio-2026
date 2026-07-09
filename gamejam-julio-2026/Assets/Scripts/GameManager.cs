using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] private string [] pociones = {"",""};
    [SerializeField] private int ultimaPocionRecogida = 1;
    private bool gameOver = false;
    private int puntosSalud = 3;
    private Player_Controller playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOver = false;
        puntosSalud = 3;
        playerController = GameObject.Find("Player_Controller").GetComponent<Player_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Reinicia el juego
    public void ReiniciarPartida(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    //Valida que el jugador cuente con 2 pociones en su inventario
    private bool ValidarInventario(){

        if(pociones[0]!= "" && pociones[1]!=""){
            return true;
        }else{
            return false;
        }

    }

    //Elimina las pociones del inventario del jugador
    public void VaciarInventario(){
        pociones[0] ="";
        pociones[1] ="";
        ultimaPocionRecogida = 1;
    }

    //Genera la accion del jugador a partir de las pociones en el inventario
    private void MezclarPociones(){
        string combinacion = ""+pociones[0]+"+"+pociones[1];
        Debug.Log(combinacion);
        switch (combinacion)
        {
            case "roja+roja":
                break;
            case "roja+azul" or "azul+roja":
                break;
            case "azul+azul":
                break;
            case "verde+verde":
                break;
            case "verde+roja" or "roja+verde":
                break;
            case "verde+azul" or "azul+verde":
                break;
        }
    }

    private void TerminarJuego(){
        gameOver = true;
    }

    // Resta un punto de salud al usuario
    public void RestarHP(){
        puntosSalud--;
        if(puntosSalud == 0){
            TerminarJuego();
        }
    }

    //Gestiona las pociones del inventario
    public void ActualizarPociones(string pocionRecogida){

        int pocionReciente = 0;
        if (ultimaPocionRecogida == 0)
        {

            pocionReciente = 1;
            pociones[1] = pocionRecogida;

        }else{
            
            pocionReciente = 0;
            pociones[0] = pocionRecogida;

        }
        ultimaPocionRecogida = pocionReciente;
        if (ValidarInventario())
        {
            
        }
    }

}
