using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace AIController
{
    public class DetectionVisual : MonoBehaviour
    {[SerializeField] private AIStateMachine stateMachine;
        private MeshRenderer _meshRenderer;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            _meshRenderer.materials[0].color = Color.white.WithAlpha(Easing.InQuad(stateMachine.DetectionPercentage));
            // _meshRenderer.material.color = _meshRenderer.material.color.WithAlpha(stateMachine.DetectionPercentage);
        }
    }
}