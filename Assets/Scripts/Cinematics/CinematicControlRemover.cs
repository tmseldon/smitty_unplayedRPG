using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        PlayableDirector directorCine;
        GameObject player;
        ActionScheduler playerActionScheduler;
        PlayerController playerController;

        private void Awake()
        {
            directorCine = GetComponent<PlayableDirector>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerActionScheduler = player.GetComponent<ActionScheduler>();
            playerController = player.GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            directorCine.played += DisableControls;
            directorCine.stopped += EnableControls;
        }

        private void OnDisable()
        {
            directorCine.played -= DisableControls;
            directorCine.stopped -= EnableControls;
        }

        private void DisableControls(PlayableDirector director)
        {
            Debug.Log("control removido");
            playerActionScheduler.CancelCurrentAction();
            playerController.enabled = false;
        }

        private void EnableControls(PlayableDirector director)
        {
            Debug.Log("control restaurado");
            playerController.enabled = true;
        }
    }

}

