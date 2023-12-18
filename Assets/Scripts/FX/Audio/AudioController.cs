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
            
            PlayCalmMusic();
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
        
        
        private void QueNewTrack(AudioClip track, float volume, float transitionTime = 0f)
        {
            _queuedAudioSource.clip = track;
            _queuedAudioSource.volume = volume;
            SwapQueuedToActive();
            _activeAudioSource.Play();
            
            if (transitionTime > 0f)
            {
                FadeBetweenSources(_queuedAudioSource, _activeAudioSource, volume, transitionTime);
            }
            else
            {
                _queuedAudioSource.Stop();
                _queuedAudioSource.volume = 0;
                _activeAudioSource.volume = volume;
            }
        }
        
        
        private void SwapQueuedToActive()
        {
            (_activeAudioSource, _queuedAudioSource) = (_queuedAudioSource, _activeAudioSource);
        }
        
        
        private void FadeBetweenSources(AudioSource sourceOne, AudioSource sourceTwo, float targetVolume, float transitionTime)
        {
            _audioFades.Add(new AudioFade(sourceOne, 0f, transitionTime, true));
            _audioFades.Add(new AudioFade(sourceTwo, musicVolume * chaseMusicVolume, transitionTime));
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
        
        
        public void PlayChaseMusic()
        {
            QueNewTrack(chaseMusic, chaseMusicVolume * musicVolume, 1f);
        }
        
        
        public void PlayCalmMusic()
        {
            QueNewTrack(calmMusic, calmMusicVolume * musicVolume, 1f);
        }
    }
}