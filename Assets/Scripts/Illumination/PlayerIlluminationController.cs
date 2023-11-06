using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Illumination
{
    public class PlayerIlluminationController : MonoBehaviour
    {
        public Transform leftHand;

        public Transform rightHand;

        public Transform head;

        public Transform chest;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            IlluminationManager.Instance.ResetIllumination();
        }

        private void OnDrawGizmosSelected()
        {
            Handles.Label(leftHand.position, Math.Round(IlluminationManager.Instance.GetIllumination(leftHand.position,HumanBodyBones.LeftHand), 2).ToString());
            Handles.Label(rightHand.position, Math.Round(IlluminationManager.Instance.GetIllumination(rightHand.position,HumanBodyBones.RightHand), 2).ToString());
            Handles.Label(head.position, Math.Round(IlluminationManager.Instance.GetIllumination(head.position,HumanBodyBones.Head), 2).ToString());
            Handles.Label(chest.position, Math.Round(IlluminationManager.Instance.GetIllumination(chest.position,HumanBodyBones.Chest), 2).ToString());
        }
    }
}

