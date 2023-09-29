using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabMaterials
{
    public class ShootState : AbstractState
    {
        [SerializeField]
        private GameObject _opponent;

        public override void Execute()
        {
        
            var opponentDistance = (_opponent.transform.position - gameObject.transform.position);  
            var opponentDirection = opponentDistance.normalized;

            var lookAtOpponentOrientation = Quaternion.LookRotation(opponentDirection);
            // snaps the gameObject to look at the enemy, note that you could also SLERP to smoothly look at the position (spherical linear interpolation)
            gameObject.transform.rotation = lookAtOpponentOrientation;

            // todo fire bullets at enemy
            // ... pew pew
        }
    }
}
