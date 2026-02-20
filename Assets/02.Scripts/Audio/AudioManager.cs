using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    COUNT
}

public enum SFX
{
    baduck_button_click,
    COUNT
}

public class AudioManager : MonoBehaviour
{
    public Transform BGMTrs;
    public Transform SFXTrs;

    private const string AUDIO_PATH = "Audio";

    private Dictionary<BGM, AudioSource> m_BGMPlayer = new Dictionary<BGM, AudioSource>();
    private AudioSource m_CurrBGMSource;

    private Dictionary<SFX, AudioSource> m_SFXPlayer = new Dictionary<SFX, AudioSource>();


    private void Awake()
    {

        LoadBGMPlayer();
        LoadSFXPlayer();
    }

    private void LoadBGMPlayer()
    {
        for (int i = 0; i < (int)BGM.COUNT; i++)
        {
            var audioName = ((BGM)i).ToString();
            var pathStr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;

            if (!audioClip)
            {
                Debug.LogError($"{audioClip} clip does not exist.");
                continue;
            }

            var newGo = new GameObject(audioName);
            var newaudioSource = newGo.AddComponent<AudioSource>();
            newaudioSource.clip = audioClip;
            newaudioSource.loop = true;
            newaudioSource.playOnAwake = false;
            newGo.transform.parent = BGMTrs;

            m_BGMPlayer[(BGM)i] = newaudioSource;



        }
    }

    private void LoadSFXPlayer()
    {
        for (int i = 0; i < (int)SFX.COUNT; i++)
        {
            var audioName = ((SFX)i).ToString();
            var pathStr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;

            if (!audioClip)
            {
                Debug.LogError($"{audioClip} clip does not exist.");
                continue;
            }

            var newGo = new GameObject(audioName);
            var newAudioSource = newGo.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = false;
            newAudioSource.playOnAwake = false;
            newGo.transform.parent = SFXTrs;

            m_SFXPlayer[(SFX)i] = newAudioSource;
        }
    }

    public void PlayBGM(BGM bgm)
    {
        if (m_CurrBGMSource)
        {
            m_CurrBGMSource.Stop();
            m_CurrBGMSource = null;
        }

        if (m_BGMPlayer.ContainsKey(bgm))
        {
            Debug.LogError($"Invalid clip name.{bgm}");
            return;
        }

        m_CurrBGMSource = m_BGMPlayer[bgm];
        m_CurrBGMSource.Play();
    }

    public void PauseBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Pause();
    }

    public void ResumeBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.UnPause();
    }

    public void StopBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Stop();
    }

    public void PlaySFX(SFX sfx)
    {
        if (!m_SFXPlayer.ContainsKey(sfx))
        {
            Debug.LogError($"Invalid clip name.({sfx})");
            return;
        }

        m_SFXPlayer[sfx].Play();
    }

    public void MuteBGM()
    {
        foreach (var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.volume = 0f;

        }
    }

    public void UnMuteBGM()
    {
        foreach (var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.volume = 0f;

        }
    }
    public void MuteSFX()
    {
        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }
    }
    public void UnMuteSFX()
    {
        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.volume = 1f;
        }
    }
}
