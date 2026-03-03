using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfilePopup : BasePopup
{
    [Header("Popup")]
    [SerializeField] private GameObject NicknameEditPopup;

    [Header("UI Elements")]
    [SerializeField] private Image profileImg;
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text rankText;

    [Header("Buttons")]
    [SerializeField] private Button editProfileIconBtn;
    [SerializeField] private Button editNicknameBtn;
    
    protected override void Init()
    {
        BindButtons();
    }
    
    private void OnEnable()
    {
        RefreshUI(); // 켤 때 일단 한 번 갱신

        // AccountManager의 닉네임 변경 알람 구독
        if (AccountManager.Instance != null)
        {
            AccountManager.Instance.OnUserDataUpdated += RefreshUI;
        }
    }

    private void OnDisable()
    {
        // 팝업 꺼질 때 알람 구독 취소
        if (AccountManager.Instance != null)
        {
            AccountManager.Instance.OnUserDataUpdated -= RefreshUI;
        }
    }


    
    public override void Show()
    {
        base.Show();
    }

    public override void Hide(PopupHideDelegate onComplete = null)
    {
        onComplete?.Invoke();
        gameObject.SetActive(false);
    }

    // 데이터가 변경될 때마다 화면을 갱신하는 함수
    private void RefreshUI()
    {
        UserData me = AccountManager.Instance.CurrentUser;
        if (me != null)
        {
            nicknameText.text = me.nickname;
            rankText.text = $"{me.rank}급";

            // TODO: me.profiledId로 이미지 세팅
        }
    }

    private void BindButtons()
    {
        editProfileIconBtn.BindEventWithSound(OnClickEditIcon);
        editNicknameBtn.BindEventWithSound(OnClickEditNickname);
    }

    private void OnClickEditIcon()
    {
        // TODO: 프사 변경 팝업 호출
    }

    private void OnClickEditNickname()
    {
        NicknameEditPopup.SetActive(true);
    }
}
