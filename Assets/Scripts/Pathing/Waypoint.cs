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
    }
}

