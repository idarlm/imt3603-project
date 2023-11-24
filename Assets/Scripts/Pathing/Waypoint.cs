using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


namespace Pathing
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private Waypoint nextWaypoint;
        [SerializeField] private Waypoint previousWaypoint;
        
        /// <summary>
        /// If true, it indicates an entity using the path should start traversing it in reverse order
        /// </summary>
        public bool isEndpoint;

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public Waypoint GetNext()
        {
            return nextWaypoint;
        }

        public void SetNext(Waypoint next)
        {
            nextWaypoint = next;
        }

        public Waypoint GetPrevious()
        {
            return previousWaypoint;
        }

        public void SetPrevious(Waypoint previous)
        {
            previousWaypoint = previous;
        }
        /// <summary>
        /// Removes this waypoint from the chain by making the adjacent nodes in the
        /// chain refer to each other instead of this one.
        /// </summary>
        public void RemoveFromPath()
        {
            nextWaypoint.SetPrevious(previousWaypoint);
            previousWaypoint.SetNext(nextWaypoint);
        }

        /// <summary>
        /// Inserts this waypoint after the provided argument waypoint
        /// </summary>
        /// <param name="previous"></param>
        public void InsertAfter(ref Waypoint previous)
        {
            nextWaypoint = previous.GetNext();
            previousWaypoint = previous;
            previous.GetNext().SetPrevious(this);
            previous.SetNext(this);
        }
        
        /// <summary>
        /// Inserts this waypoint before the provided argument waypoint
        /// </summary>
        /// <param name="next"></param>
        public void InsertBefore(ref Waypoint next)
        {
            nextWaypoint = next;
            previousWaypoint = next.GetPrevious();
            next.GetPrevious().SetNext(this);
            next.SetPrevious(this);
        }

        private void DrawDirections(Color next, Color previous, Color node, float nodeRadius = 0.5f, float length = 0.5f)
        {
            length = Math.Clamp(length, 0, 1);
            var position = transform.position;
            Gizmos.color = node;
            Gizmos.DrawSphere(position, nodeRadius);
            if (nextWaypoint != null)
            {
                var nextPosition = nextWaypoint.transform.position;
                Gizmos.color = next;
                var midwayPoint = Vector3.Lerp(transform.position, nextPosition, length);
                Gizmos.DrawLine(position, midwayPoint);
                Vector3 arrowheadDirection = (nextPosition - position).normalized;
                Vector3 arrowheadLeft = Quaternion.Euler(0, 160, 0) * arrowheadDirection;
                Vector3 arrowheadRight = Quaternion.Euler(0, -160, 0) * arrowheadDirection;
                Gizmos.DrawRay(midwayPoint, arrowheadLeft);
                Gizmos.DrawRay(midwayPoint, arrowheadRight);
            }
            if( previousWaypoint != null)
            {
                Gizmos.color = previous;
                Gizmos.DrawLine(
                    transform.position, 
                    Vector3.Lerp(position, previousWaypoint.transform.position, length)
                    );
            }
        }


        public void OnDrawGizmos()
        {
            DrawDirections(
                Color.red.WithAlpha(0.25f),
                Color.blue.WithAlpha(0.25f),
                Color.white.WithAlpha(0.25f)
                );
            var position = transform.position;
            if (nextWaypoint != null && nextWaypoint == this)
            {
                
                Gizmos.DrawIcon(position,"warning", true);
                Handles.Label(position + Vector3.up, "Next waypoint cannot be self");
            }
            if(previousWaypoint != null && previousWaypoint == this)
            {
                Gizmos.DrawIcon(position, "warning", true);
                Handles.Label(position + Vector3.up, "Previous waypoint cannot be self");
            }
        }

        public void OnDrawGizmosSelected()
        {
            DrawDirections(
                Color.red, Color.blue,Color.white, 1, 0.9f
                );
            if(nextWaypoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(nextWaypoint.transform.position, 1f);
            }

            if (previousWaypoint != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(previousWaypoint.transform.position, 1f);
            }
               
        }
    }
}

