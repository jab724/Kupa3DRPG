using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kupa
{
    public class AudioSource_BGM : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;

        public void Play()
        {
            Manager_BGM.Self.Play(clip);
        }
        public void PlayFade(float fadeTime = 1f)
        {
            Manager_BGM.Self.PlayFade(clip, fadeTime);
        }
    }
}
