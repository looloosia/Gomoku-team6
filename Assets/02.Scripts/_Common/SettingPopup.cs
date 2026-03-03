using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : BasePopup
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle muteToggle;


    protected override void Init()
    {
        base.Init();

        bgmSlider.value = SoundManager.Instance.GetBGMVolume();
        sfxSlider.value = SoundManager.Instance.GetSFXVolume();

        muteToggle.isOn = SoundManager.Instance.IsSoundMute();

        bgmSlider.onValueChanged.AddListener(SoundManager.Instance.BGMVolume);
        sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SFXVolume);

        muteToggle.onValueChanged.AddListener(SoundManager.Instance.BGMMute);
        muteToggle.onValueChanged.AddListener(SoundManager.Instance.SFXMute);
    }

    private void OnBgmVolumeChanged(float volume)
    {
        // TODO: [음향 담당] BGM 볼륨 조절 로직
        // 슬라이더 값(volume)은 기본적으로 0.0 ~ 1.0 사이
        // 예시: SoundManager.Instance.SetBgmVolume(volume);
        
        Debug.Log($"[UI] BGM 볼륨 변경됨: {volume}");
    }

    private void OnSfxVolumeChanged(float volume)
    {
        // TODO: [음향 담당] 효과음(SFX) 볼륨 조절 로직
        
        Debug.Log($"[UI] 효과음 볼륨 변경됨: {volume}");
    }

    private void OnMuteToggled(bool isMuted)
    {
        // TODO: [음향 담당] 전체 음소거 로직
        // isMuted가 true면 체크됨(음소거 ON), false면 체크해제(음소거 OFF)
        // 예시: SoundManager.Instance.SetMute(isMuted);
        
        Debug.Log($"[UI] 음소거 상태 변경됨: {isMuted}");
    }
}
