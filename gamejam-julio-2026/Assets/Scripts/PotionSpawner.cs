using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PotionSpawner : MonoBehaviour
{
    public List<GameObject> potionPrefabs;
    [SerializeField]private float tiempoSpawn = 4.0f;
    private Vector3[] sitiosSpawn = new Vector3[4];
    private bool[] pocionGenerada = {false,false,false,false}; 
    private int[] tipoPocion = {0,1,0,1,2,0,1,0,1,2,0,1};
    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake(){

        sitiosSpawn[0] = new Vector3(-14,6,0);
        sitiosSpawn[1] = new Vector3(-2,7,0);
        sitiosSpawn[2] = new Vector3(11,7,0);
        sitiosSpawn[3] = new Vector3(11,-6.6f,0);
    
    }
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(SpawnPotion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int ElegirPocion(){
        return Random.Range(0, 12);
    }

    private int ElegirPosicion(){
        return Random.Range(0, 4);
    }

    public void LiberarPosicion(Vector3 posicionLibrar){
        for (int i = 0; i < sitiosSpawn.Length; i++)
        {
            if(posicionLibrar == sitiosSpawn[i]){
                pocionGenerada[i] = false;
            }
        }
    }

    IEnumerator SpawnPotion(){
        int indicePocionElegida=0;
        bool estaOcupado = false;
        int posicionElegida =0;
        Vector3 posicionSpawneo = new Vector3(0,0,0);
        while (!gameManager.gameOver)
        {
            
            indicePocionElegida = tipoPocion[ElegirPocion()];
            posicionElegida = ElegirPosicion();
            estaOcupado = pocionGenerada[posicionElegida];
            posicionSpawneo = sitiosSpawn[posicionElegida];
            yield return new WaitForSeconds(tiempoSpawn);
            if(!estaOcupado){
                pocionGenerada[posicionElegida] = true;
                Instantiate(potionPrefabs[indicePocionElegida], posicionSpawneo, potionPrefabs[indicePocionElegida].transform.rotation);
            }
        }
    }

}
