using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Utility;

namespace FX.Effects
{
    public abstract class Effect
    {
        protected float TransitionTime = 0.5f;
        protected float ElapsedTime = 0f;

        protected Effect() { }

        public void Progress(float deltaTime, Volume volume)
        {
            ElapsedTime += deltaTime;
            ApplyEffect(volume);
        }

        public bool IsDone()
        {
            return ElapsedTime >= TransitionTime;
        }
        
        protected abstract void ApplyEffect(Volume volume);
    }
    
    
    
    
    public class FadeToColor : Effect {
        private readonly Color _targetColor;
        private  Color _oldColor;
        private bool _startColorSet = false;
        
        public FadeToColor(Color color, float transitionTime = 0.5f)
        {
            _targetColor = color;
            TransitionTime = transitionTime;
        }
        
        protected override void ApplyEffect(Volume volume) {
            if (volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments))
            {
                if(!_startColorSet)
                {
                    _oldColor = colorAdjustments.colorFilter.value;
                    _startColorSet = true;
                }
                var newColor = Color.Lerp(_oldColor, _targetColor, Easing.InCubic(ElapsedTime / TransitionTime));
                colorAdjustments.colorFilter.Override(newColor);
            }
        }
    }
    

    public class Fear : Effect
    {
        public Fear (float TransitionTime = 0.5f)
        {
            this.TransitionTime = TransitionTime;
        }
        
        protected override void ApplyEffect(Volume volume) {
            if (volume.profile.TryGet<Vignette>(out var vignette))
            {
                vignette.intensity.Override(0.5f * Math.Clamp (Easing.InCubic(ElapsedTime / TransitionTime),0f,1f));
            }

            if (volume.profile.TryGet<PaniniProjection>(out var panini))
            {
                panini.distance.Override(Math.Clamp (Easing.InCubic(ElapsedTime / TransitionTime),0f,1f));
            }
            
            if(volume.profile.TryGet<Bloom>(out var bloom))
            {
                bloom.intensity.Override(5f * Math.Clamp (Easing.InCubic(ElapsedTime / TransitionTime),0f,1f));
            }
        }
    }
    
    public class Calm : Effect
    {
        public Calm (float TransitionTime = 0.5f)
        {
            this.TransitionTime = TransitionTime;
        }
        
        protected override void ApplyEffect(Volume volume) {
            if (volume.profile.TryGet<Vignette>(out var vignette))
            {
                vignette.intensity.Override(1f - 0.7f * Math.Clamp (Easing.InCubic(ElapsedTime / TransitionTime),0f,1f));
            }
        }
    }
    
    
    
}