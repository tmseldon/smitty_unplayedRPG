using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Core;
using RPG.Saving;
using Newtonsoft.Json.Linq;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, IJsonSaveable
    {
        [SerializeField] float maxSpeed = 5.66f;
        [SerializeField] float _maxDistancePath = 40f;

        NavMeshAgent navMeshCharacter;
        Animator animatorController;
        Ray lastRay;
        ActionScheduler actionScheduler;
        Health health;

        private void Awake()
        {
            navMeshCharacter = GetComponent<NavMeshAgent>();
            animatorController = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if(health.HaveDead) 
            {
                navMeshCharacter.enabled = false;
                return; 
            }
            //MoveToCursor();
            UpdateAnimator();
        }

        void UpdateAnimator()
        {
            Vector3 velocity = navMeshCharacter.velocity;
            Vector3 localVelocity = transform.InverseTransformVector(velocity);

            //Debug.Log($"velocidad: {velocity.magnitude} y velocida dlocal: {localVelocity.magnitude}");

            float speed = localVelocity.z;
            animatorController.SetFloat("forwardSpeed", speed);
        }

        public void StartMoveAction(Vector3 destination, float speedFactor)
        {
            actionScheduler.StartAction(this);
            MoveTo(destination, speedFactor);
        }


        public void MoveTo(Vector3 destinationPoint, float speedFactor)
        {
            navMeshCharacter.isStopped = false;
            navMeshCharacter.speed = Mathf.Clamp01(speedFactor) * maxSpeed;
            navMeshCharacter.destination = destinationPoint;
        }

        public bool CanMoveTo(Vector3 positionHit)
        {
            //Determine if there is a complete viable path to the point and calculate the distance
            NavMeshPath possiblePath = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, positionHit, NavMesh.AllAreas, possiblePath);

            if (!hasPath) { return false; }
            if (possiblePath.status != NavMeshPathStatus.PathComplete) { return false; }
            if (GetPathDistance(possiblePath) > _maxDistancePath) { return false; }

            return true;
        }

        private float GetPathDistance(NavMeshPath possiblePath)
        {
            float totalDistance = 0;
            Vector3[] cornersPoint = possiblePath.corners;
            int numbercorners = cornersPoint.Length;

            if (numbercorners < 2) { return totalDistance; }

            for (int index = 0; index < numbercorners - 1; index++)
            {
                totalDistance += Vector3.Distance(cornersPoint[index], cornersPoint[index + 1]);
            }

            return totalDistance;
        }

        public void Cancel()
        {
            navMeshCharacter.isStopped = true;
            //fighter.Cancel();
        }

        public JToken CaptureAsJToken()
        {
            MoverSaveData dataToSave = new MoverSaveData();

            dataToSave.positionPlayer = new SerializableVector3(transform.position);
            dataToSave.idScene = SceneManager.GetActiveScene().buildIndex;

            return JToken.FromObject(dataToSave);
        }

        public void RestoreFromJToken(JToken state)
        {
            MoverSaveData loadState = state.ToObject<MoverSaveData>();

            if (loadState.idScene != SceneManager.GetActiveScene().buildIndex) { return; }

            navMeshCharacter.enabled = false;
            transform.position = loadState.positionPlayer.ToVector();
            navMeshCharacter.enabled = true;
            actionScheduler.CancelCurrentAction();
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public int idScene;
            public SerializableVector3 positionPlayer;
        }

        /*
         * 
         * ISaveable implementation
        public object CaptureState()
        {
            MoverSaveData dataToSave = new MoverSaveData();

            dataToSave.positionPlayer = new SerializableVector3(transform.position);
            dataToSave.idScene = SceneManager.GetActiveScene().buildIndex;

            return dataToSave;

        }

        public void RestoreState(object state)
        {
            MoverSaveData loadState = (MoverSaveData) state;

            if(loadState.idScene != SceneManager.GetActiveScene().buildIndex) { return; }

            navMeshCharacter.enabled = false;
            transform.position = loadState.positionPlayer.ToVector();
            navMeshCharacter.enabled = true;

        }
        */
    }
}


//**********************************************************************************
//Código inicial cuando el movimiento estaba acoplado a esta Clase, se quita esto y
//se mueve a PlayerController

/*
private void MoveToCursor()
{
    if (Input.GetMouseButton(0))
    {
        ProcessClick();
    }

    //Debug.DrawRay(lastRay.origin, lastRay.direction * 100, Color.red);
}

private void ProcessClick()
{
    Ray rayByMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit rayInfo;

    bool isRayHit = Physics.Raycast(rayByMouse, out rayInfo);
    if (isRayHit)
    {
        navMeshCharacter.destination = rayInfo.point;
    }
}
*/
