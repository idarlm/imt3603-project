using System;
using FX.Audio;
using FX.Visual;
using FX.Visual.Effects;
using UnityEngine;

namespace FX
{
    public class AIInteractionFXManager
    {
        private static AIInteractionFXManager _instance;
        public static AIInteractionFXManager Instance
        {
            get { return _instance ??= new AIInteractionFXManager(); }
        }

        private int _detections;
        private bool _isPlayerGrabbed;
        private int _nearDetections;
        public AudioController AudioController { set; get; }
        public PostProcessingController PostProcessingController { set; get; }
        public Action OnPlayerCapturedAction { set; get; }
        
        
        
        // Near Detection
        
        public void OnPlayerNearlyDetected()
        {
            if (!IsPlayerNearlyDetected() && !IsPlayerDetected() && !IsPlayerGrabbed())
            {
                AudioController.PlayNearlySeenSting();
            }
            _nearDetections++;
        }
        
        public void OnLostSightOfPlayerNear()
        {
            _nearDetections = Math.Max(--_nearDetections, 0);
        }
        
        private bool IsPlayerNearlyDetected()
        {
            return _nearDetections > 0;
        }
        
        // Detection
        
        public void OnPlayerDetected()
        {
            if (!IsPlayerDetected() && !IsPlayerGrabbed())
            {
                PostProcessingQue.Instance.QueEffect(new Fear(3));
                AudioController.PlaySeenSting();
                AudioController.PlayChaseMusic();
            }
            _detections++;
        }
        
        public void OnLostSightOfPlayer()
        {
            _detections--;
            if (!IsPlayerDetected() && !IsPlayerGrabbed())
            {
                PostProcessingQue.Instance.QueEffect(new Calm(6));
                AudioController.StopCurrentTrack(0.5f);
                AudioController.PlayEscapedDetectionCue();
                AudioController.PlayCalmMusic(3f);
            }
        }
        
        private bool IsPlayerDetected()
        {
            return _detections > 0;
        }
        
        // Grabbed
        
        public void OnPlayerGrabbed()
        {
            PostProcessingQue.Instance.QueEffect(new FadeToColor(Color.black, 4f));
            AudioController.StopCurrentTrack(1f);
            AudioController.PlayGrabbedSound();
            _isPlayerGrabbed = true;
        }
        
        public bool IsPlayerGrabbed()
        {
            return _isPlayerGrabbed;
        }
        
        // Placed in cage
        
        public void OnPlayerPlacedInCage()
        {
            _detections = 0;
            _isPlayerGrabbed = false;
            _nearDetections = 0;
            
            PostProcessingQue.Instance.QueEffect(new FadeToColor(Color.white, 4f));
            PostProcessingQue.Instance.QueEffect(new Calm(6));
            AudioController.PlayCalmMusic(4f);
            OnPlayerCapturedAction?.Invoke();
        }

        
        
        


    }
}