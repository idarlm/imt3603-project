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
            _meshRenderer.materials[0].color = new Color(1.0f, 1.0f, 1.0f, Easing.InQuad(stateMachine.DetectionPercentage));
            // _meshRenderer.material.color = _meshRenderer.material.color.WithAlpha(stateMachine.DetectionPercentage);
        }
    }
}