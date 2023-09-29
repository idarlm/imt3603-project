using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabMaterials
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        private AbstractState _currentState;

        public EntityState CurrentStateType { get; private set; }

        [SerializeField]
        private List<AbstractState> _stateImplementations;

        [SerializeField]
        private List<EntityState> _stateKeys;

        public enum EntityState
        {
            Idle,
            Shoot
        }

        public void SetState(EntityState entityState)
        {
            int stateIndex = _stateKeys.IndexOf(entityState);

            var state = _stateImplementations[stateIndex];
            var oldState = _currentState;

            oldState?.Exit();
            state?.Enter();

            _currentState = state;
            CurrentStateType = entityState;
        }

        public void Update()
        {
            _currentState?.Execute();
        }
    }
}
