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
    public bool gameOver = false;
    [SerializeField] private int puntosSalud = 3;
    private Player_Controller playerController;
    public Image corazonUno;
    public Image corazonDos;
    public Image corazonTres;
    public Image primeraPocion;
    public Image segundaPocion;
    public Sprite pocimaRoja;
    public Sprite pocimaAzul;
    public Sprite pocimaVerde;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOver = false;
        puntosSalud = 3;
        playerController = GameObject.Find("Doctor").GetComponent<Player_Controller>();
        MostrarPociones();
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
        playerController.tieneAccion=false;
        playerController.crearPocion=false;
        MostrarPociones();
    }

    //Genera la accion del jugador a partir de las pociones en el inventario
    public string MezclarPociones(){
        string combinacion = ""+pociones[0]+"+"+pociones[1];
        string reaccion="";
        Debug.Log(combinacion);
        switch (combinacion)
        {
            case "roja+roja":
                reaccion="BOOM";
                break;
            case "roja+azul" or "azul+roja":
                reaccion="DASH";
                break;
            case "azul+azul":
                reaccion = "ENEMY";
                break;
            case "verde+verde":
                reaccion="1UP";
                break;
            case "verde+roja" or "roja+verde":
                reaccion="SPIT";
                break;
            case "verde+azul" or "azul+verde":
                reaccion="TP";
                break;
        }
        return reaccion;
    }

    private void TerminarJuego(){
        gameOver = true;
    }

    // Resta un punto de salud al usuario
    public void RestarHP(){
        puntosSalud--;
        playerController.Teleport();
        ActualizarCorazones();
        if(puntosSalud == 0){
            TerminarJuego();
        }
    }

    public void RestaurarHP(){
        if(puntosSalud<3){
            puntosSalud++;
            ActualizarCorazones();
        }
        VaciarInventario();
    }

    public void ActualizarCorazones(){
        switch (puntosSalud)
        {
            case 3:
                corazonUno.gameObject.SetActive(true);
                corazonDos.gameObject.SetActive(true);
                corazonTres.gameObject.SetActive(true);
                break;
            case 2:
                corazonUno.gameObject.SetActive(true);
                corazonDos.gameObject.SetActive(true);
                corazonTres.gameObject.SetActive(false);
                break;
            case 1: 
                corazonUno.gameObject.SetActive(true);
                corazonDos.gameObject.SetActive(false);
                corazonTres.gameObject.SetActive(false);
                break;
            default:
                corazonUno.gameObject.SetActive(false);
                corazonDos.gameObject.SetActive(false);
                corazonTres.gameObject.SetActive(false);
                break;
        }
    }

    private void MostrarPociones(){
        if(pociones[0]==""){
            primeraPocion.gameObject.SetActive(false);
        }else{
            primeraPocion.gameObject.SetActive(true);
            switch (pociones[0])
            {
                case "verde":
                    primeraPocion.sprite = pocimaVerde;
                    break;
                case "roja":
                    primeraPocion.sprite = pocimaRoja;
                    break;
                case "azul":
                    primeraPocion.sprite = pocimaAzul;
                    break;
            }
        }

        if(pociones[1]==""){
            segundaPocion.gameObject.SetActive(false);
        }else{
            segundaPocion.gameObject.SetActive(true);
            switch (pociones[1])
            {
                case "verde":
                    segundaPocion.sprite = pocimaVerde;
                    break;
                case "roja":
                    segundaPocion.sprite = pocimaRoja;
                    break;
                case "azul":
                    segundaPocion.sprite = pocimaAzul;
                    break;
            }
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
        if(ValidarInventario()){
            playerController.crearPocion = true;
            playerController.crearEfecto();
        }
        MostrarPociones();
    }

}
