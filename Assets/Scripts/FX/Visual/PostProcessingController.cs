﻿using UnityEngine;
using UnityEngine.Rendering;

namespace FX.Visual
{
    public class PostProcessingController : MonoBehaviour
    {
        private PostProcessingQue _postProcessingQue;
        [SerializeField] private Volume volume;
        void Start()
        {
            AIInteractionFXManager.Instance.PostProcessingController = this;
            _postProcessingQue = PostProcessingQue.Instance;
        }
        
        void Update()
        {
            _postProcessingQue.Progress(Time.deltaTime, volume);
        }
    }
}