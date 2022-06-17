using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Control
{
    public class ChangeResolution : MonoBehaviour
    {
        TMP_Dropdown _resolutionsOptions;

        private void Awake()
        {
            _resolutionsOptions = GetComponent<TMP_Dropdown>();
        }


        // Start is called before the first frame update
        void Start()
        {
            _resolutionsOptions.onValueChanged.AddListener(delegate { ResolutionSelection(_resolutionsOptions.value); });
            //Mismo resultado, otra notacion con lambda
            //_resolutionsOptions.onValueChanged.AddListener((x) => Testing(_resolutionsOptions.value));
        }

        private void ResolutionSelection(int resolutionOption)
        {
            if(resolutionOption == 1)
            {
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                Screen.SetResolution(1024, 576, false, 60);
            }
            if (resolutionOption == 2)
            {
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                Screen.SetResolution(1280, 720, false, 60);
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            }
        }
    }

}
