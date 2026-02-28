using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BGM
{
    COUNT
}

public enum SFX
{
    baduck_button_click,
    COUNT
}


public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private GameObject soundPanelPrefab;
    private SoundPanelController soundPanelController;

    private const string SOUND_PATH = "Sound";

    private Dictionary<BGM, AudioSource> m_BGMPlayer = new Dictionary<BGM, AudioSource>();
    private AudioSource m_CurrBGMSource;

    private Dictionary<SFX, AudioSource> m_SFXPlayer = new Dictionary<SFX, AudioSource>();


    protected override void Awake()
    {
        base.Awake();
        LoadBGMPlayer();
        LoadSFXPlayer();
    }

    void Start()
    {
        soundPanelController = Instantiate(soundPanelPrefab, transform).GetComponent<SoundPanelController>();

        /*순서문제 생길 수 있는 코드*/
        foreach (var audio in m_BGMPlayer)
        {
            soundPanelController.bgmVolume.value = audio.Value.volume;
            soundPanelController.bgmMute.isOn = audio.Value.mute;
        }

        foreach (var audio in m_SFXPlayer)
        {
            soundPanelController.eventVolume.value = audio.Value.volume;
            soundPanelController.eventMute.isOn = audio.Value.mute;
        }
        ////////////////////////////////////////////////////////////////

        //연결 후, AddListener에 연결
        ConnectToUI();

    }

    void ConnectToUI()
    {
        soundPanelController.bgmVolume.onValueChanged.AddListener(BGMVolume);
        soundPanelController.eventVolume.onValueChanged.AddListener(SFXVolume);

        soundPanelController.bgmMute.onValueChanged.AddListener(BGMMute);
        soundPanelController.eventMute.onValueChanged.AddListener(SFXMute);
    }

    

    //BGM파일 로드
    private void LoadBGMPlayer()
    {
        for (int i = 0; i < (int)BGM.COUNT; i++)
        {
            var audioName = ((BGM)i).ToString();
            var pathStr = $"{SOUND_PATH}/BGM/{audioName}";
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
            
            newGo.transform.SetParent(this.transform);
            m_BGMPlayer[(BGM)i] = newaudioSource;
        }
    }

    private void LoadSFXPlayer()
    {
        for (int i = 0; i < (int)SFX.COUNT; i++)
        {
            var audioName = ((SFX)i).ToString();
            var pathStr = $"{SOUND_PATH}/SFX/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            Debug.Log(pathStr);
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
            newGo.transform.SetParent(this.transform);
 
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

    public void BGMMute(bool isMute)
    {
        foreach (var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.mute = isMute;

        }
    }


    public void SFXMute(bool isMute)
    {
        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.mute = isMute;
        }
    }


    private void BGMVolume(float volume)
    {
        foreach (var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.volume = volume;
        }
    }

    private void SFXVolume(float volume)
    {
        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.volume = volume;
        }
    }

    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Debug.LogWarning($"SFX소스 개수: " + m_SFXPlayer.Count);
    }
}

