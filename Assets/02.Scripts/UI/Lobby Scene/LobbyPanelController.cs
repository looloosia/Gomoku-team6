using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanelController : MonoBehaviour
{
    [Header("Player Profile UI")]
    [SerializeField] private Image profileImg;
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text nicknameText;
    
    [Header("Popups")]
    [SerializeField] private GameObject popupCanvas;

    [SerializeField] private GameObject gameOptionPopup;
    [SerializeField] private GameObject gameRecordPopup;
    // [SerializeField] private RankPopup rankPopup;
    [SerializeField] private GameObject playerInfoPopup;
    
    [Header("Buttons")]
    [SerializeField] private Button playGameBtn;
    [SerializeField] private Button playerRecodeBtn;
    [SerializeField] private Button rankBtn;
    [SerializeField] private Button storeBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button playerinfoBtn;

    void Start()
    {
        Init();
        BindButtons();
        UpdateUserProfile();
    }

    void Init()
    {
        popupCanvas.SetActive(true);
    }

    void BindButtons()
    {
        playGameBtn.onClick.AddListener(() => {gameOptionPopup.SetActive(true);});
        playerRecodeBtn.onClick.AddListener(() => {gameRecordPopup.SetActive(true);});
        rankBtn.onClick.AddListener(ShowRankPopup);
        storeBtn.onClick.AddListener(ChangeSceneStore);
        settingBtn.onClick.AddListener(ShowSettingPopup);
        backBtn.onClick.AddListener(ChangeSceneMain);
        playerinfoBtn.onClick.AddListener(() => {playerInfoPopup.SetActive(true);});
    }

    private void UpdateUserProfile()
    {
        // AccountManager에서 현재 로그인한 유저 데이터 가져오기
        UserData me = AccountManager.Instance.CurrentUser;

        if (me != null)
        {
            // 데이터가 있으면 텍스트에 꽂아주기
            rankText.text = $"{me.rank}급";
            nicknameText.text = me.nickname;

            // TODO: 프로필 이미지는 제공된 이미지 리스트에서 번호(profileId)로 가져올 예정
            // profileImg.sprite = ProfileManager.Instance.GetSprite(me.profileId);
        }
        else
        {
            // 로그인 데이터가 없을 때의 예외 처리 (에디터 테스트용)
            nicknameText.text = "로그인 필요";
            rankText.text = "-";
        }
    }

    private void ShowRankPopup()
    {
        // 서버내 플레이어들의 랭킹 팝업창 띄우기
    }

    private void ChangeSceneStore()
    {
        // 상점 씬으로 이동
    }

    private void ShowSettingPopup()
    {
        UIManager.Instance.OpenSettingPopup();
    }

    private void ChangeSceneMain()
    {
        GameManager.Instance.ChangeToMainScene();
    }
}
