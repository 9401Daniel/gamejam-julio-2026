using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Charco : MonoBehaviour
{
    private float duracionCharco = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DesaparecerCharco());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DesaparecerCharco(){
        yield return new WaitForSeconds(duracionCharco);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("¿Quien me piso?");
        Debug.Log("Tag: "+other.tag);
        Debug.Log(other.GetComponent<Enemy>());
        if (other.CompareTag("Enemy")) {
            
            // Destruye el charco y al enemigo que lo piso
            Destroy(gameObject);
            Destroy(other.GetComponent<GameObject>());
        }

    }
}
