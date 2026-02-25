using UnityEngine;
using UnityEngine.UI;

public class LobbyPanelController : MonoBehaviour
{
    [Header("Popups")]
    [SerializeField] private GameObject popupCanvas;

    [SerializeField] private GameOptionPopup gameOptionPopup;
    // [SerializeField] private RankPopup rankPopup;
    // [SerializeField] private SettingPopup settingPopup; >> 프리팹으로 Instantiate으로 인한 삭제
    
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
    }

    void Init()
    {
        popupCanvas.SetActive(true);
        gameOptionPopup.Hide();
        // settingPopup.Hide(); >> 프리팹으로 Instantiate으로 인한 삭제
    }

    void BindButtons()
    {
        playGameBtn.onClick.AddListener(ShowGameOptionPopup);
        playerRecodeBtn.onClick.AddListener(ChangeScenePlayerRecode);
        rankBtn.onClick.AddListener(ShowRankPopup);
        storeBtn.onClick.AddListener(ChangeSceneStore);
        settingBtn.onClick.AddListener(ShowSettingPopup);
        backBtn.onClick.AddListener(ChangeSceneMain);
        playerinfoBtn.onClick.AddListener(ChangeSceneMain);
    }

    private void ShowGameOptionPopup()
    {
        gameOptionPopup.Show();
    }

    private void ChangeScenePlayerRecode()
    {
        // 내 기보 씬으로 이동
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
        GameManager.Instance.OpenSettingPopup();
    }

    private void ChangeSceneMain()
    {
        // 메인 씬으로 이동
    }

    private void ShowPlayerInfoPopup()
    {
        // 내 정보 번경이 가능한 팝업창 띄우기
    }
}
