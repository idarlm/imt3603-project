using System;
using System.Collections.Generic;
using System.Linq;
using AIController;
using FX.Visual;
using UnityEngine;
using UnityEngine.Audio;

namespace FX.Audio
{
    internal class AudioFade
    {
        private readonly float _targetVolume;
        private readonly float _startVolume;
        private readonly float _transitionTime;
        private float _elapsedTime;
        private readonly AudioSource _audioSource;
        private readonly bool _stopOnFadeEnd = false;
            
        // TODO: Repeat of fading logic found in visual FX portion, consider generics
        public AudioFade(AudioSource audioSource, float targetVolume, float transitionTime, bool stopOnFadeEnd = false)
        {
            stopOnFadeEnd = stopOnFadeEnd;
            _audioSource = audioSource;
            _targetVolume = targetVolume;
            _transitionTime = transitionTime;
            _startVolume = audioSource.volume;
        }
            
        public void Progress(float deltaTime)
        {
            _elapsedTime += deltaTime;
            _audioSource.volume = Mathf.Lerp(_startVolume, _targetVolume, _elapsedTime / _transitionTime);
            if (IsDone() && _stopOnFadeEnd)
            {
                _audioSource.Stop();
            }
        }
            
        public bool IsDone()
        {
            return _elapsedTime >= _transitionTime;
        }
    }
    
    
    
    
    public class AudioController :  MonoBehaviour
    {
        [SerializeField] [Range(0,1)] private float musicVolume = 1f;
        [SerializeField] [Range(0,1)] private float fxVolume = 1f;
        
        [SerializeField] private AudioClip nearlySeenSting;
        [SerializeField] [Range(0,1)] private float nearlySeenStingVolume = 1f;
        
        [SerializeField] private AudioClip seenSting;
        [SerializeField] [Range(0,1)] private float seenStingVolume = 1f;
        
        [SerializeField] private AudioClip grabbedSound;
        [SerializeField] [Range(0,1)] private float grabbedSoundVolume = 1f;
        
        [SerializeField] private AudioClip chaseMusic;
        [SerializeField] [Range(0,1)] private float chaseMusicVolume = 1f;
        
        [SerializeField] private AudioClip calmMusic;
        [SerializeField] [Range(0,1)] private float calmMusicVolume = 1f;
        
        [SerializeField] private AudioClip escapedDetectionCue;
        [SerializeField] [Range(0,1)] private float escapedDetectionCueVolume = 1f;
        
        [SerializeField]private AudioMixer audioMixer;
        
        private AudioSource _activeAudioSource;
        private AudioSource _queuedAudioSource;
        private AudioSource _fxChannel;
        private List<AudioFade> _audioFades = new List<AudioFade>();
        
        private void Start()
        {
            _activeAudioSource = gameObject.AddComponent<AudioSource>();
            _activeAudioSource.loop = true;
            _activeAudioSource.volume = calmMusicVolume * musicVolume;
            _activeAudioSource.clip = calmMusic;
            
            _queuedAudioSource = gameObject.AddComponent<AudioSource>();
            _queuedAudioSource.loop = true;
            _queuedAudioSource.volume = chaseMusicVolume * musicVolume;
            _queuedAudioSource.clip = chaseMusic;
            
            _fxChannel = gameObject.AddComponent<AudioSource>();
            _fxChannel.loop = false;
            _fxChannel.volume = fxVolume;
            
            AIInteractionFXManager.Instance.AudioController = this;
            
            PlayCalmMusic(0);
        }
        
        void Update()
        {
            _audioFades = _audioFades
                .Select( fade => {
                    fade.Progress(Time.deltaTime);
                    return fade;
                })
                .Where(fade => !fade.IsDone())
                .ToList();
        }
        
        
        
        public void PlayNearlySeenSting()
        {
            _fxChannel.volume = nearlySeenStingVolume;
            _fxChannel.PlayOneShot(nearlySeenSting);
        }
        
        
        public void PlaySeenSting()
        {
            _fxChannel.volume = seenStingVolume;
            _fxChannel.PlayOneShot(seenSting);
        }
        
        
        public void PlayGrabbedSound()
        {
            _fxChannel.volume = grabbedSoundVolume;
            _fxChannel.PlayOneShot(grabbedSound);
        }
        
        public void PlayEscapedDetectionCue()
        {
            _fxChannel.volume = escapedDetectionCueVolume;
            _fxChannel.PlayOneShot(escapedDetectionCue);
        }
        
        
        public void PlayChaseMusic(float transitionTime = 1f)
        {
            StartNewTrack(chaseMusic, chaseMusicVolume * musicVolume, transitionTime);
        }
        
        
        public void PlayCalmMusic(float transitionTime = 1f)
        {
            StartNewTrack(calmMusic, calmMusicVolume * musicVolume, transitionTime);
        }
        
        private void StartNewTrack(AudioClip track, float volume, float transitionTime = 0, float startVolume = 0f)
        {
            StopCurrentTrack(transitionTime);
            
            _queuedAudioSource.clip = track;
            _queuedAudioSource.volume = startVolume;
            (_activeAudioSource, _queuedAudioSource) = (_queuedAudioSource, _activeAudioSource);
            _activeAudioSource.Play();
            
            _audioFades.Add(new AudioFade(_activeAudioSource, musicVolume * chaseMusicVolume, transitionTime));
        }
        
        
        public void StopCurrentTrack(float fadeOutTime = 0f)
        {
            if(fadeOutTime > 0f)
            {
                _audioFades.Add(new AudioFade(_activeAudioSource, 0f, fadeOutTime, true));
            }
            else
            {
                _activeAudioSource.Stop();
            }
        }


    }
}