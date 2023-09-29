// MoveTo.cs

using System;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour {
    
    public Transform goal;
    private NavMeshAgent _agent;

    void Start () {
        _agent = GetComponent<NavMeshAgent>();
        _agent.destination = goal.position; 
    }

    private void Update()
    {
        _agent.destination = goal.position;
    }
}