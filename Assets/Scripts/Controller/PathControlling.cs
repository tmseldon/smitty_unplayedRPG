using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control
{
    public class PathControlling : MonoBehaviour
    {
        [SerializeField] float radiusWaypoint = 0.2f;

        private List<Vector3> listWaypointsPatrolling = new List<Vector3>();
        private int numberWaypoints;

        public int NumberWaypoints { get { return numberWaypoints; } }
        //public List<Vector3> ListWaypointsPatrolling { get { return listWaypointsPatrolling; } }


        private void OnAwake()
        {
            //numberWaypoints = transform.childCount;
            //PopulateListWaypoints();
        }

        private void PopulateListWaypoints()
        {
            for(int index = 0; index < numberWaypoints; index++)
            {
                listWaypointsPatrolling.Add(GetWaypointPosition(index));
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            numberWaypoints = transform.childCount;

            for (int indice = 0; indice < numberWaypoints; indice++)
            {
                Gizmos.DrawSphere(GetWaypointPosition(indice), radiusWaypoint);
                Gizmos.DrawLine(GetWaypointPosition(indice), GetWaypointPosition(GetNextIndex(indice)));
            }
        }

        public Vector3 GetWaypointPosition(int index)
        {
            return transform.GetChild(index).position;
        }

        public int GetNextIndex(int currentIndex)
        {
            if(currentIndex + 1 == transform.childCount)
            {
                return 0;
            }

            return currentIndex + 1;
        }
    }
}

