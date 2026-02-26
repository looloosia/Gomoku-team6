using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordPanelController : MonoBehaviour
{
    [Header("Profiles")]
    [SerializeField] private ProfilePanel player1Profile; // 왼쪽 (AI)
    [SerializeField] private ProfilePanel player2Profile; // 오른쪽 (나)

    [Header("Top Buttons")]
    [SerializeField] private Button exitReviewBtn;  // 복기 종료 (구 기권 버튼)
    [SerializeField] private Button settingBtn;     // 설정 버튼

    [Header("Replay Control Buttons (하단 컨트롤러)")]
    [SerializeField] private Button firstBtn; // 처음으로 (<<)
    [SerializeField] private Button prevBtn;  // 한 수 뒤로 (<)
    [SerializeField] private Button nextBtn;  // 한 수 앞으로 (>)
    [SerializeField] private Button lastBtn;  // 끝으로 (>>)

    public event Action OnFirstMoveEvent;
    public event Action OnPrevMoveEvent;
    public event Action OnNextMoveEvent;
    public event Action OnLastMoveEvent;

    private void Start()
    {
        BindButtons();
    }

    private void BindButtons()
    {
        // 하단 복기 컨트롤 버튼 연결
        firstBtn.onClick.AddListener(() => OnFirstMoveEvent?.Invoke());
        prevBtn.onClick.AddListener(() => OnPrevMoveEvent?.Invoke());
        nextBtn.onClick.AddListener(() => OnNextMoveEvent?.Invoke());
        lastBtn.onClick.AddListener(() => OnLastMoveEvent?.Invoke());

        // 복기 종료 버튼: 만능 팝업창 띄우기!
        exitReviewBtn.onClick.AddListener(OnClickExitReview);
        
        // 설정 버튼
        settingBtn.onClick.AddListener(() => GameManager.Instance.OpenSettingPopup());
    }

    private void OnClickExitReview()
    {
        ConfirmPopup popup = GameManager.Instance.OpenConfirmPopup();
        popup.Show("복기를 종료하시겠습니까?", "", "취소", null, "확인", () => 
        {
            // 확인 누르면 로비 씬으로 이동!
        });
    }

    // 씬 시작될 때 로직 스크립트에서 이 함수를 불러서 프로필을 세팅
    public void SetupProfiles(string p1Name, string p1Rank, bool p1IsBlack, string p2Name, string p2Rank, bool p2IsBlack)
    {
        player1Profile.SetProfileInfo(p1Name, p1Rank, null);
        player1Profile.SetMarkerImage(p1IsBlack);

        player2Profile.SetProfileInfo(p2Name, p2Rank, null);
        player2Profile.SetMarkerImage(p2IsBlack);
    }
}
