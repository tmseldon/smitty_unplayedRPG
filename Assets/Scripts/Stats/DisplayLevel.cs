using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace RPG.Stats
{
    public class DisplayLevel : MonoBehaviour
    {
        private TextMeshProUGUI _textCurrentLevel;
        private BaseStats _baseStatsPlayer;

        void Awake()
        {
            _textCurrentLevel = GetComponent<TextMeshProUGUI>();
            _baseStatsPlayer = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        // Update is called once per frame
        void Update()
        {
            _textCurrentLevel.SetText($"Level_ {_baseStatsPlayer.GetLevel()}");
        }
    }
}


