using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//효과음용 AudioSource
namespace Kupa
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSource_SFX : MonoBehaviour
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

            PreferenceData.AddListenerSfxVolumeChangeEvent(ChangeVolume);
        }

        public void Play()
        {
            audioSource.Play();
        }

        private void ChangeVolume()
        {
            audioSource.volume = PreferenceData.SfxVolume * PreferenceData.MasterVolume * 0.0001f;
        }

        private void OnDestroy()
        {
            PreferenceData.RemoveListenerSfxVolumeChangeEvent(ChangeVolume);
        }
    }
}
