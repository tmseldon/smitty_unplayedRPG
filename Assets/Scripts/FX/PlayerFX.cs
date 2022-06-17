using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.SceneManagement;


namespace RPG.FX
{
    public class PlayerFX : MonoBehaviour
    {
        [SerializeField] ParticleSystem portalFX;
        [SerializeField] AudioClip teleportSound;
        
        //PortalLevel[] portalManager;
        private void OnEnable()
        {
            PortalLevel.TransitionToPortal += StartPortalFX;

            /*
            portalManager = FindObjectsOfType<PortalLevel>();
            foreach (PortalLevel portal in portalManager)
            {
                portal.TransitionFinish += StartPortalFX;
            }
            */
        }
        private void OnDisable()
        {
            PortalLevel.TransitionToPortal -= StartPortalFX;
        }

        private void StartPortalFX()
        {
            //Debug.Log("llamando funcion suscriptora");
            portalFX.Stop();
            portalFX.Play();
            AudioSource.PlayClipAtPoint(teleportSound, Camera.main.transform.position, 0.6f);
            //PortalLevel.TransitionFinish -= StartPortalFX;
        }
    }
}

