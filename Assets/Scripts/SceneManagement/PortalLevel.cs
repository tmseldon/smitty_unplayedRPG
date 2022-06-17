using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class PortalLevel : MonoBehaviour
    {
        enum Portals
        {
            Portal_A, Portal_B, Portal_C, Portal_D,
        }

        //Signature delegate for event and event
        public delegate void Notify();
        public static Notify TransitionToPortal;

        Fader faderSystem;

        [SerializeField] string goToLevelName;
        [SerializeField] Transform spawnPoint;
        [SerializeField] Portals destinationPortal;

        [SerializeField] float timeFadeIn = 1f;
        [SerializeField] float timeFadeOut = 0.5f;
        [SerializeField] float timeWaitFade = 0.5f;

        //public Transform SpawnPoint {  get { return spawnPoint; } }

        private void Awake()
        {
            faderSystem = FindObjectOfType<Fader>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        IEnumerator Transition()
        {
            SavingWrapper checkPoint = FindObjectOfType<SavingWrapper>();

            DontDestroyOnLoad(this.gameObject);
            OnTransitionPortal();
            RemovePlayerControl(true);

            checkPoint.Save();

            yield return faderSystem.FadeInFX(timeFadeIn);
            yield return SceneManager.LoadSceneAsync(goToLevelName);
            RemovePlayerControl(true);

            PortalLevel otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            checkPoint.Load();

            yield return new WaitForSeconds(timeWaitFade);
            //No necesita el yield return en este caso, para darle control al jugador más rapidamente
            //de manera que no espere que el efecto esté terminado para jugar
            faderSystem.FadeOutFX(timeFadeOut);

            OnTransitionPortal();
            RemovePlayerControl(false);

            checkPoint.Save();
            Destroy(this.gameObject);
        }

        private void RemovePlayerControl(bool removeControl)
        {
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = !removeControl;
        }

        private void UpdatePlayer(PortalLevel otherPortal)
        {
            if(otherPortal)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                NavMeshAgent navMeshPlayer = player.GetComponent<NavMeshAgent>();

                navMeshPlayer.enabled = false;
                //navMeshPlayer.Warp(otherPortal.spawnPoint.position);
                player.transform.position = otherPortal.spawnPoint.position;
                player.transform.rotation = otherPortal.spawnPoint.rotation;
                navMeshPlayer.enabled = true;
            }
        }

        private PortalLevel GetOtherPortal()
        {
            PortalLevel[] portalesDimensionales = FindObjectsOfType<PortalLevel>();

            foreach(PortalLevel portal in portalesDimensionales)
            {
                if(portal == this) continue;

                if(portal.destinationPortal == this.destinationPortal)
                {
                    return portal;
                }
            }

            return null;
        }

        //En general se deja como protected virtual, para que clase que hereden esta funcion llamen al delegado tambien
        protected virtual void OnTransitionPortal()
        {
            TransitionToPortal?.Invoke();
            //Debug.Log(gameObject.name);
        }
    }
}

