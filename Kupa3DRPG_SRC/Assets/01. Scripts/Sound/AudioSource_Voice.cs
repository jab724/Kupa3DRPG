using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kupa
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSource_Voice : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.clip = clip;
            ChangeVolume();

            PreferenceData.AddListenerVoiceVolumeChangeEvent(ChangeVolume);
        }

        public void Play()
        {
            audioSource.Play();
        }

        private void ChangeVolume()
        {
            audioSource.volume = PreferenceData.VoiceVolume * PreferenceData.MasterVolume * 0.0001f;
        }

        private void OnDestroy()
        {
            PreferenceData.RemoveListenerVoiceVolumeChangeEvent(ChangeVolume);
        }
    }
}
