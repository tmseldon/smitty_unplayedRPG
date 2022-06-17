using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class EndGameTrigger : MonoBehaviour
    {
        public void ChangeStateInGame()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<EndGameController>().ChangeEndGameState();
        }
    }
}