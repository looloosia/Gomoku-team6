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
    public event Action<bool> OnMarkerSelectedEvent;
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

        confirmBtn.onClick.AddListener(() => 
        {
            OnConfirmMoveEvent?.Invoke();
            SetActionButtonsInteractable(false);
        });

        returnBtn.onClick.AddListener(() => 
        {
            OnReturnMoveEvent?.Invoke();
            SetActionButtonsInteractable(false);
        });

        resignBtn.onClick.AddListener(OnClickResign);
        settingBtn.onClick.AddListener(OnClickSetting);
    }

    private void OnClickResign()
    {
        ConfirmPopup popup = GameManager.Instance.OpenConfirmPopup();
        popup.Show("기권 하시겠습니까?", "", "취소", null, "확인", () => {OnResignEvent?.Invoke();});
    }

    private void OnClickSetting()
    {
        GameManager.Instance.OpenSettingPopup();
    }

    public void SetupPlayerProfile(string nickname, string rankData, bool isBlack)
    {
        playerProfile.SetProfileInfo(nickname, rankData);
        playerProfile.SetMarkerImage(isBlack);
    }

    public void SetupAIProfile(string aiName, string aiRank, bool isBlack)
    {
        aiProfile.SetProfileInfo(aiName, aiRank);
        aiProfile.SetMarkerImage(isBlack);
    }

    public void UpdateTurnUI(string msg)
    {
        turnText.text = msg;
    }

    public void UpdateTimerUI(int timeLeft)
    {
        timerText.text = timeLeft.ToString();
        timerText.color = (timeLeft <= 5) ? Color.red : Color.black;
    }

    public void OnStoneTemporarilyPlaced()
    {
        SetActionButtonsInteractable(true); 
    }

    // 돌이 보드에 올려져있는지 여부에 따라 버튼 활성화/비활성화
    private void SetActionButtonsInteractable(bool isInteractable)
    {
        confirmBtn.interactable = isInteractable;
        returnBtn.interactable = isInteractable;
    }
}
