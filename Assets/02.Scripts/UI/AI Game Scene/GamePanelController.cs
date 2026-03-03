using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelController : MonoBehaviour
{
    [Header("Profiles")]
    [SerializeField] private ProfilePanel playerProfile;
    [SerializeField] private ProfilePanel aiProfile;

    [Header("State UI")]
    [SerializeField] private TMP_Text turnText; 
    [SerializeField] private TMP_Text timerText;         

    [Header("Buttons")]
    [SerializeField] private Button confirmBtn;  // 착수
    [SerializeField] private Button returnBtn;     // 한수 무르기
    [SerializeField] private Button resignBtn;   // 기권
    [SerializeField] private Button settingBtn;  // 설정

    // 외부로 쏘아 올릴 이벤트 / 델리게이트
    public event Action OnConfirmMoveEvent; // 착수 눌렀을 때
    public event Action OnReturnMoveEvent; // 무르기 눌렀을 때
    public event Action OnResignEvent; // 기권 확정했을 때

    void Start()
    {
        BindButtons();
    }

    private void BindButtons()
    {
        // 처음엔 보드에 돌이 없으니 착수/무르기 비활성화
        SetActionButtonsInteractable(false);

        confirmBtn.BindEventWithSound(() => { OnConfirmMoveEvent?.Invoke(); });

        returnBtn.BindEventWithSound(() => { OnReturnMoveEvent?.Invoke(); });

        resignBtn.BindEventWithSound(OnClickResign);
        settingBtn.BindEventWithSound(OnClickSetting);
    }

    private void OnClickResign()
    {
        ConfirmPopup popup = UIManager.Instance.OpenConfirmPopup();
        popup.Show("기권 하시겠습니까?", "기권할 경우 코인과 승급 포인트를 잃습니다.", "취소", null, "확인", () => 
        {
            OnResignEvent?.Invoke();
            GameManager.Instance.ChangeToLobbyScene();
        });
    }

    private void OnClickSetting()
    {
        UIManager.Instance.OpenSettingPopup();
    }

    public void SetupPlayerProfile(string nickname, string rankData, bool isBlack, Sprite profileSprite)
    {
        playerProfile.SetProfileInfo(nickname, rankData, profileSprite);
        playerProfile.SetMarkerImage(isBlack);
    }

    public void SetupAIProfile(string aiName, string aiRank, bool isBlack, Sprite profileSprite)
    {
        aiProfile.SetProfileInfo(aiName, aiRank, profileSprite);
        aiProfile.SetMarkerImage(isBlack);
    }

    public void UpdateTurnUI(string msg)
    {
        turnText.text = msg;
    }

    public void UpdateTimerUI(int timeLeft)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeLeft);
        timerText.text = time.ToString(@"mm\:ss");
    
        timerText.color = (timeLeft <= 5) ? Color.red : Color.white;
    }

    public void OnStoneTemporarilyPlaced()
    {
        SetActionButtonsInteractable(true); 
    }

    // 돌이 보드에 올려져있는지 여부에 따라 버튼 활성화/비활성화
    public void SetActionButtonsInteractable(bool isInteractable)
    {
        confirmBtn.interactable = isInteractable;
        returnBtn.interactable = isInteractable;
    }
}
