using UnityEngine;
using UnityEngine.AI;

namespace AIController
{
    public class AIContext
    {
        public NavMeshAgent Agent { set; get; }
        public Transform Target { set; get; }
        public Vector3[] PatrolWaypoints { set; get; }
    }
}