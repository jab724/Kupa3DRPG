using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BGM은 동시에 여러개가 재생 될 일이 없으므로 별도로 관리한다. 단, 페이드 교체 연출을 위해 AudioSource를 2개 사용한다.
namespace Kupa
{
    [RequireComponent(typeof(AudioSource))]
    public class Manager_BGM : MonoBehaviour
    {
        private static Manager_BGM instance;
        public static Manager_BGM Self
        {
            get
            {
                return instance;
            }
        }

        private const int STATE_READY = 0;          //최초. BGM이 재생되고 있지 않은 상태
        private const int STATE_PLAY_AUDIO_1 = 1;   //1번 AudioSource 재생 중
        private const int STATE_PLAY_AUDIO_2 = 2;   //2번 AudioSource 재생 중

        private AudioSource audioSource1;
        private AudioSource audioSource2;

        private int state = STATE_READY;

        private float volume { get { return PreferenceData.MasterVolume * PreferenceData.BgmVolume * 0.0001f; } }   //페이드 연출 중에 사용하기 위한 볼륨

        private void Awake()
        {
            if (instance == null) instance = this;
            else { Destroy(gameObject); return; }

            var audioSources = GetComponents<AudioSource>();
            audioSource1 = audioSources[0];
            audioSource2 = audioSources[1];
            audioSource1.playOnAwake = audioSource2.playOnAwake = false;
            audioSource1.loop = audioSource2.loop = true;
            ChangeVolume();

            PreferenceData.AddListenerBgmVolumeChangeEvent(ChangeVolume);
        }

        private void Start()
        {
            audioSource1.volume = audioSource2.volume = 0f;
        }

        public void Play(AudioClip clip)        //페이드 연출 없이 재생
        {
            if (CheckOverrideClip(clip)) return;
            switch (state)
            {
                case STATE_READY:
                    state = STATE_PLAY_AUDIO_1;
                    audioSource1.clip = clip;
                    audioSource1.Play();
                    break;
                case STATE_PLAY_AUDIO_1:
                    state = STATE_PLAY_AUDIO_1;
                    audioSource1.clip = clip;
                    audioSource1.Play();
                    break;
                case STATE_PLAY_AUDIO_2:
                    state = STATE_PLAY_AUDIO_2;
                    audioSource2.clip = clip;
                    audioSource2.Play();
                    break;
                default:
                    Debug.LogError(this.GetType().Name + " :: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :::: 접근 오류");
                    state = STATE_READY;
                    Play(clip);
                    break;
            }
        }
        public void PlayFade(AudioClip clip, float fadeTime = 1f)   //페이드 연출을 주면서 재생
        {
            if (CheckOverrideClip(clip)) return;
            if (fadeTime <= 0f) Play(clip);
            StopCoroutine("PlayFadeCor");
            switch (state)
            {
                case STATE_READY:
                    state = STATE_PLAY_AUDIO_1;
                    audioSource1.clip = clip;
                    audioSource1.Play();
                    break;
                case STATE_PLAY_AUDIO_1:
                    state = STATE_PLAY_AUDIO_2;
                    audioSource2.clip = clip;
                    audioSource2.Play();
                    break;
                case STATE_PLAY_AUDIO_2:
                    state = STATE_PLAY_AUDIO_1;
                    audioSource1.clip = clip;
                    audioSource1.Play();
                    break;
                default:
                    Debug.LogError(this.GetType().Name + " :: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :::: 접근 오류");
                    state = STATE_READY;
                    PlayFade(clip, fadeTime);
                    break;
            }
            StartCoroutine("PlayFadeCor", fadeTime);
        }

        private IEnumerator PlayFadeCor(float fadeTime)
        {
            switch (state)
            {
                case STATE_PLAY_AUDIO_1:
                    while (0.01f < audioSource2.volume || audioSource1.volume < volume)
                    {
                        audioSource1.volume = Mathf.MoveTowards(audioSource1.volume, volume, Time.deltaTime / fadeTime);
                        audioSource2.volume = Mathf.MoveTowards(audioSource2.volume, 0f, Time.deltaTime / fadeTime);
                        yield return null;
                    }
                    audioSource1.volume = volume;
                    audioSource2.volume = 0f;
                    audioSource2.Stop();
                    break;
                case STATE_PLAY_AUDIO_2:
                    while (audioSource2.volume < volume || 0.01f < audioSource1.volume)
                    {
                        audioSource1.volume = Mathf.MoveTowards(audioSource1.volume, 0f, Time.deltaTime / fadeTime);
                        audioSource2.volume = Mathf.MoveTowards(audioSource2.volume, volume, Time.deltaTime / fadeTime);
                        yield return null;
                    }
                    audioSource2.volume = volume;
                    audioSource1.volume = 0f;
                    audioSource1.Stop();
                    break;
                default:
                    Debug.LogError(this.GetType().Name + " :: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " :::: 접근 오류");
                    break;
            }
        }

        private bool CheckOverrideClip(AudioClip clip)      //동일한 음악인지 체크한다. 같은 BGM인데 다시 재생되는 것을 방지
        {
            bool result = false;
            if (state == STATE_PLAY_AUDIO_1) result = audioSource1.clip == clip;
            else if (state == STATE_PLAY_AUDIO_2) result = audioSource2.clip == clip;
            return result;
        }

        private void ChangeVolume()
        {
            audioSource1.volume = audioSource2.volume = PreferenceData.BgmVolume * PreferenceData.MasterVolume * 0.0001f;
        }

        private void OnDestroy()
        {
            PreferenceData.RemoveListenerBgmVolumeChangeEvent(ChangeVolume);
        }
    }
}