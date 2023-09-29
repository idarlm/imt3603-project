using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabMaterials
{
    public class IdleState : AbstractState
    {
        public const int IDLE_ANIM_NUM = 5;
        public const float DETECTION_RANGE = 10f;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private GameObject _opponent;

        public override void Enter()
        {
            base.Enter();

            _animator.SetInteger("IdleAnimVariant", Random.Range(0, IDLE_ANIM_NUM));
            _animator.SetTrigger("EnterIdle");
        }

        public override void Execute()
        {
            if(Vector3.Distance(gameObject.transform.position, _opponent.transform.position) < DETECTION_RANGE)
            {
                _stateMachine.SetState(StateMachine.EntityState.Shoot);
            }
        }
    }
}
