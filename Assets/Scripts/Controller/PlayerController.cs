using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.AI;
using System;
using RPG.Attributes;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private RaycastHit rayHit;
        private Fighter fighterPlayer;
        private Health healthPlayer;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType Name;
            public Texture2D IconCursor;
            public Vector2 HotspotLocation;
        }

        [SerializeField] float _distanceToClickWithMouse = 12f;
        [SerializeField] float _rangeDetectionNavMesh = 2f;
        [SerializeField] List<CursorMapping> _listOfCursors = new List<CursorMapping>();
        [SerializeField] float _raycastRadius = 0.8f;

        void Awake()
        {
            mover = GetComponent<Mover>();
            healthPlayer = GetComponent<Health>();
        }

        void Update()
        {
            if(InteractWithUI()) {return;}
            if(healthPlayer.HaveDead) 
            {
                SetCursor(CursorType.None);
                return; 
            }
            if(InteractWithComponent()) { return; }
            if(InteractWithMovement()) { return; }

            SetCursor(CursorType.None);
        }


        private bool InteractWithComponent()
        {
            RaycastHit[] rayHitsByMouse = RayCastAllSorted();
            foreach (RaycastHit rayhitting in rayHitsByMouse)
            {
                if(rayhitting.distance < _distanceToClickWithMouse)
                {
                    IRaycastable[] rayCastComponent = rayhitting.transform.GetComponents<IRaycastable>();
                    foreach (IRaycastable rayCastable in rayCastComponent)
                    {
                        if (rayCastable.HandleRayCast(this))
                        {
                            SetCursor(rayCastable.GetCursor());
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RayCastAllSorted()
        {
            List<RaycastHit> allRayHits = Physics.SphereCastAll(GetRayByMouse(), _raycastRadius).ToList();
            allRayHits.OrderByDescending(x => x.distance);
            return allRayHits.ToArray();
        }

        private bool InteractWithMovement()
        {
            Vector3 rayHitPoint;
            bool isHitting = RaycastNavMesh(out rayHitPoint);

            if (isHitting)
            {
                if(!mover.CanMoveTo(rayHitPoint)) { return false; }

                if(Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(rayHitPoint, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }


        private bool RaycastNavMesh(out Vector3 positionHit)
        {
            positionHit = new Vector3();

            //Getting the raycast based on the mouse's click and determine if hits terrain
            Ray rayByMouse = GetRayByMouse();
            bool isHitting = Physics.Raycast(rayByMouse, out rayHit);

            if(!isHitting) { return false; }
            
            //Determine if that point is near of a walkable Navmesh, either way the point is discarded
            NavMeshHit hitNav;
            bool isInNaveMesh = NavMesh.SamplePosition(rayHit.point, out hitNav, _rangeDetectionNavMesh, NavMesh.AllAreas);
            
            if(!isInNaveMesh) { return false; }
            
            positionHit = hitNav.position;

            return true;
        }

        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private static Ray GetRayByMouse()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }


        private void SetCursor(CursorType typeSelected)
        {
            CursorMapping selectedInfo = _listOfCursors.Find(x => x.Name == typeSelected);
            Cursor.SetCursor(selectedInfo.IconCursor, selectedInfo.HotspotLocation, CursorMode.Auto);
        }
    }
}

