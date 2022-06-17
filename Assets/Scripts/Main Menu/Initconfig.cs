using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core
{
    public class Initconfig : MonoBehaviour
    {
        //private void Awake()
        //{            
        //    GameObject persistantObjects = GameObject.FindGameObjectWithTag("Persistant");
        //    if (persistantObjects)
        //    {
        //        Destroy(persistantObjects);
        //    }
        //}

        void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
        }
    }
}

