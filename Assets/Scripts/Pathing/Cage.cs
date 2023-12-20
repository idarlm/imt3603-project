using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathing
{
    public class Cage : MonoBehaviour
    {
        [SerializeField] private Transform aiTarget;
        [SerializeField] private Transform playerTarget;
        [SerializeField] private ToggleTrigger cageTrigger;

        public Vector3 GetAITargetPosition()
        {
            return aiTarget.position;
        }

        public Vector3 GetPlayerTargetPosition()
        {
            return playerTarget.position;
        }

        public void Reset()
        {
            cageTrigger.ToggleOff();
        }

        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red.WithAlpha(0.5f);
            Gizmos.DrawSphere(aiTarget.position, 0.5f);
            Gizmos.color = Color.green.WithAlpha(0.5f);
            Gizmos.DrawSphere(playerTarget.position, 0.5f);
        }
        #endif
    }
}
