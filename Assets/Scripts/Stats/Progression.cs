using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenuAttribute(fileName = "ProgressionData", menuName = "Stats/New Progression Data", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionData[] _infoClasses = null;

        Dictionary<ClassType, Dictionary<Status, float[]>> _lookUpTable = new Dictionary<ClassType, Dictionary<Status, float[]>>();

        private void OnEnable()
        {
            BuildLookup();
        }

        public float GetStat(Status stat, ClassType characterClass, int level)
        {
            //BuildLookup();
            if(! _lookUpTable.ContainsKey(characterClass)) { return 0; }

            Dictionary<Status, float[]> getStatAndLevels = _lookUpTable[characterClass];

            if (getStatAndLevels != null)
            {
                if (!getStatAndLevels.ContainsKey(stat)) { return 0; }

                float[] levelsInStat = getStatAndLevels[stat];
                if (levelsInStat != null && levelsInStat.Length >= level)
                {
                    return levelsInStat[level - 1];
                }
            }

            return 0;
        }

        public float[] GetLevels(Status stat, ClassType characterClass)
        {
            return _lookUpTable[characterClass][stat];
        }


        private void BuildLookup()
        {
            if(_lookUpTable.Count != 0) { return; }

            foreach (ProgressionData data in _infoClasses)
            {
                Dictionary<Status, float[]> tempStatInfo = new Dictionary<Status, float[]>();

                foreach (ProgressionStatInfo statInfo in data.stats)
                {
                    tempStatInfo.Add(statInfo.statsChar, statInfo.levels);
                }

                _lookUpTable.Add(data.className, tempStatInfo);
            }

        }

        [System.Serializable]
        class ProgressionData
        {
            public ClassType className;
            public ProgressionStatInfo[] stats;
        }
        
        [System.Serializable]
        class ProgressionStatInfo
        {
            public Status statsChar;
            public float[] levels;
        }

    }
}
