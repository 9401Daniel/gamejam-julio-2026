using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerX : MonoBehaviour
{

    //Script que se asigna a la cámara principal para que siga 
    //al gameobject Doctor.
    public GameObject Doctor;
    private Vector3 offset = new Vector3(0, 0, -10);

    void Start()
    {

    }

    void Update()
    {
        transform.position = Doctor.transform.position + offset;
    }
}