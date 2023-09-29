using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabMaterials
{
    public abstract class AbstractState : MonoBehaviour
    {
        protected StateMachine _stateMachine;
    
        private void OnEnable()
        {
            if (_stateMachine == null)
                _stateMachine = GetComponentInParent<StateMachine>();
        }

        public virtual void Enter()
        {
        
        }

        public virtual void Exit()
        {

        }

        public abstract void Execute();
    }
}
