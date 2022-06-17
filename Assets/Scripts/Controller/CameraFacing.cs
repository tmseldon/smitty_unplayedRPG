using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    void LateUpdate()
    {
        //En este caso mira el punto posici�n de la camara
        transform.LookAt(Camera.main.transform);

        //alternativa
        //En este caso se setea el vector direcci�n de la camara para que se alinee con el objeto
        //transform.forward = Camera.main.transform.forward;
    }
}
