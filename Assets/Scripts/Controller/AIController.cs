using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [Header("Params Enemy")]
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float timeSuspicious = 2.5f;
        [SerializeField] PathControlling pathControl;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 1.5f;
        [Range(0f,1f)]
        [SerializeField] float fractionOfSpeedPatrol = 0.2f;

        [Header("Params Aggro")]
        [SerializeField] float _aggroCooldown = 5f;
        [SerializeField] float _shoutDistance = 0.5f;

        GameObject player;
        Fighter fighter;
        Health healthAI;
        Mover moverAI;
        ActionScheduler actionScheduler;

        //Vector3 initialGuardPosition;
        LazyValue<Vector3> initialGuardPosition;
        float timeSinceLastSawPlayer;
        float timeWaitingOnWaypoint;
       //List<Vector3> waypointsWhereIPatrol;
        int currentWaypointIndex = 0;
        float timeWaitingOnSpot = Mathf.Infinity;
        float _timeSinceAggro = Mathf.Infinity;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            fighter = GetComponent<Fighter>();
            healthAI = GetComponent<Health>();
            moverAI = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();

            initialGuardPosition = new LazyValue<Vector3>(SetInitPos);
        }

        void Start()
        {
            initialGuardPosition.ForceInit();
            //waypointsWhereIPatrol = pathControl.ListWaypointsPatrolling;
        }

        private Vector3 SetInitPos()
        {
            return transform.position;
        }

        void Update()
        {
            if (healthAI.HaveDead)
            {
                fighter.enabled = false;
                return;
            }

            if (IsAggravated() && fighter.CanAttack(player))
            {
                StartAttack();
                timeSinceLastSawPlayer = 0;
            }
            else if (timeSinceLastSawPlayer < timeSuspicious)
            {
                BeSuspicious();
            }
            else
            {
                StartPatrolling();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceAggro += Time.deltaTime;
            //timeWaitingOnSpot += Time.deltaTime;                    

        }

        private void StartPatrolling()
        {
            Vector3 nextPosition = initialGuardPosition.Value;
            
            if(pathControl != null)
            {
                if(AtWaypoint())
                {
                    if(timeWaitingOnSpot > waypointDwellTime)
                    {
                        CycleWaypoints();
                        timeWaitingOnSpot = 0;
                    }

                    //CycleWaypoints();
                    //timeWaitingOnSpot = 0;
                    timeWaitingOnSpot += Time.deltaTime;                    
                }


                nextPosition = GetCurrentWaypoint();
            }

            moverAI.StartMoveAction(nextPosition, fractionOfSpeedPatrol);
            /*
            if (timeWaitingOnSpot > waypointDwellTime)
            {
                moverIA.StartMoveAction(nextPosition);
            }*/

        }

        public void Aggravate()
        {
            _timeSinceAggro = 0f;
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return pathControl.GetWaypointPosition(currentWaypointIndex);
        }

        private void CycleWaypoints()
        {
            int nextWaypointIndex = pathControl.GetNextIndex(currentWaypointIndex);
            currentWaypointIndex = nextWaypointIndex;
        }


        private void BeSuspicious()
        {
            actionScheduler.CancelCurrentAction();
        }

        private bool IsAggravated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance || _timeSinceAggro < _aggroCooldown;
        }

        private void StartAttack()
        {
            fighter.Attack(player);
            AggravateNearbyAllies();
        }

        private void AggravateNearbyAllies()
        {
            RaycastHit[] raycastHitAllies = Physics.SphereCastAll(transform.position, _shoutDistance, Vector3.up);

            foreach(RaycastHit rayHitting in raycastHitAllies)
            {
                AIController enemyController = rayHitting.transform.GetComponent<AIController>();
                if (enemyController == null) { continue; }

                enemyController.Aggravate();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
