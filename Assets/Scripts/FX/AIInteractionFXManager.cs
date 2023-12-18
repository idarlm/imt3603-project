using FX.Audio;
using FX.Visual;

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
        
        
        
        // Near Detection
        
        public void OnPlayerNearlyDetected()
        {
            if (!IsPlayerNearlyDetected() && !IsPlayerDetected())
            {
                AudioController.PlayNearlySeenSting();
            }
            _nearDetections++;
        }
        
        public void OnLostSightOfPlayerNear()
        {
            _nearDetections--;
        }
        
        private bool IsPlayerNearlyDetected()
        {
            return _nearDetections > 0;
        }
        
        // Detection
        
        public void OnPlayerDetected()
        {
            if (!IsPlayerDetected())
            {
                AudioController.PlaySeenSting();
                AudioController.PlayChaseMusic();
            }
            _detections++;
        }
        
        public void OnLostSightOfPlayer()
        {
            _detections--;
            if (!IsPlayerDetected() && !_isPlayerGrabbed)
            {
                AudioController.PlayCalmMusic();
            }
        }
        
        private bool IsPlayerDetected()
        {
            return _detections > 0;
        }
        
        // Grabbed
        
        public void OnPlayerGrabbed()
        {
            AudioController.PlayGrabbedSound();
            _isPlayerGrabbed = true;
        }
        
        public bool IsPlayerGrabbed()
        {
            return _isPlayerGrabbed;
        }
        
        // Placed in cage
        
        public void OnPlayerCaptured()
        {
            _detections = 0;
            _isPlayerGrabbed = false;
            _nearDetections = 0;
            AudioController.PlayCalmMusic();
        }

        
        
        


    }
}