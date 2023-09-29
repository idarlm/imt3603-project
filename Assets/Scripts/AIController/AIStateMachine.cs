using System.Collections.Generic;
using AIController.ChaseBehaviour;
using LabMaterials;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace AIController
{
    public class AIStateMachine : StateMachineMono<AIContext>
    {
        [SerializeField] private string currentStateSerialized;
        private IState<AIContext> _currentState;
        private AIContext _context;

        [SerializeField] private string chaseState;

        [SerializeField] private Transform target;
        
        
        private void Start()
        {
            _currentState = ChaseFactory.CreateChaseState(currentStateSerialized);
            this.
            _context = new AIContext
            {
                Agent = GetComponent<NavMeshAgent>(),
                Target = target.transform
            };
        }

        public void ChangeState(IAIState nextState)
        {
            base.ChangeState(nextState);
            currentStateSerialized = nextState.GetName();
        }

        private void Update()
        {
            _currentState?.Update(_context);
        }
    }
    
}