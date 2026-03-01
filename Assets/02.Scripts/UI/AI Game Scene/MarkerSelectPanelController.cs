using System;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class MarkerSelectPanelController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button blackMarkerBtn;
    [SerializeField] private Button whiteMarkerBtn;
    [SerializeField] private Button randomMarkerBtn;

    public event Action<MarkerChoice> OnMarkerSelectedEvent;

    void Start()
    {
        gameObject.SetActive(true);
        BindButtons();
    }

    private void BindButtons()
    {
        blackMarkerBtn.onClick.AddListener(() => OnClickMarker(MarkerChoice.Black));
        whiteMarkerBtn.onClick.AddListener(() => OnClickMarker(MarkerChoice.White));
        randomMarkerBtn.onClick.AddListener(() => OnClickMarker(MarkerChoice.Random));
    }

    private void OnClickMarker(MarkerChoice choice)
    {
        OnMarkerSelectedEvent?.Invoke(choice);

        PlayerType finalType;
        if (choice == MarkerChoice.Random)
            finalType = (PlayerType)UnityEngine.Random.Range(1,3);
        else
            finalType = (PlayerType)choice;

        bool isPlayerBlack = (finalType == PlayerType.Black);

        UserData me = AccountManager.Instance.CurrentUser;
        GamePanelController gamePanel = FindFirstObjectByType<GamePanelController>();
        
        if (gamePanel != null && me != null)
        {
            // 내 프로필 세팅 (불변)
            gamePanel.SetupPlayerProfile(me.nickname, $"{me.rank}급", isPlayerBlack, null);

            // Constants.GameType과 GameManager.GameType 규격 적용!
            if (GameManager.Instance.GameType == Constants.GameType.SinglePlay)
            {
                // 싱글 플레이 (AI 전)
                gamePanel.SetupAIProfile("알파고(AI)", "18급", !isPlayerBlack, null);
            }
            else if (GameManager.Instance.GameType == Constants.GameType.LocalDualPlay)
            {
                // 로컬 2인 플레이 (하나의 폰/PC로 번갈아가며 두는 모드)
                // 지금은 임시로 Player 2라고 띄워줍니다!
                gamePanel.SetupAIProfile("Player 2", "-", !isPlayerBlack, null);
            }
        }

        GameManager.Instance.OnMarkerSelected(finalType);

        gameObject.SetActive(false);
    }
}
