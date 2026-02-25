using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelController : MonoBehaviour
{
    [Header("Profiles")]
    [SerializeField] private ProfilePanel playerProfile;
    [SerializeField] private ProfilePanel aiProfile;

    [Header("State UI")]
    [SerializeField] private TMP_Text turnIndicatorText; 
    [SerializeField] private TMP_Text timerText;         

    [Header("Buttons")]
    [SerializeField] private Button confirmBtn;  // 착수
    [SerializeField] private Button returnBtn;     // 한수 무르기
    [SerializeField] private Button resignBtn;   // 기권
    [SerializeField] private Button settingBtn;  // 설정

    void Start()
    {
        BindButtons();
        // ShowMarkerSelectPopup();
    }

    private void BindButtons()
    {
        // 처음엔 보드에 돌이 없으니 착수/무르기 비활성화
        // SetActionButtonsInteractable(false);

        // confirmBtn.onClick.AddListener(OnClickConfirm);
        // returnBtn.onClick.AddListener(OnClickReturn);
        // resignBtn.onClick.AddListener(OnClickResign);
        settingBtn.onClick.AddListener(OnClickSetting);
    }

    // private void OnClickResign()
    // {
    //     ConfirmPopup popup = GameManager.Instance.OpenConfirmPopup();
    //     popup.Show(
    //     msg: "기권 하시겠습니까?", 
    //     submsg: "", 
    //     cancelStr: "취소", 
    //     _onCancel: null, 
    //     confirmStr: "확인", 
    //     _onConfirm: () => { Debug.Log("기권 처리"); }
    // )

    public void OnClickSetting()
    {
        GameManager.Instance.OpenSettingPopup();
    }
}
