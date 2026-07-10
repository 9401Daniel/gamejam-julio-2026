using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PotionSpawner : MonoBehaviour
{
    public List<GameObject> potionPrefabs;
    [SerializeField]private float tiempoSpawn = 4.0f;
    private Vector3[] sitiosSpawn = new Vector3[4];
    private bool[] pocionGenerada = new bool[4]; 
    private int[] tipoPocion = {0,1,0,1,2,0,1,0,1,2,0,1};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake(){

        sitiosSpawn[0] = new Vector3(-14,6,0);
        sitiosSpawn[0] = new Vector3(-2,7,0);
        sitiosSpawn[0] = new Vector3(11,7,0);
        sitiosSpawn[0] = new Vector3(11,-6.6f,0);
    
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
