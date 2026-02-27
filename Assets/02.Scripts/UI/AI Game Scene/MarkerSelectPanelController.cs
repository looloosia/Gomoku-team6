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
            gamePanel.SetupPlayerProfile(me.nickname, $"{me.rank}급", isPlayerBlack, null);
            gamePanel.SetupAIProfile("알파고(AI)", "18급",  !isPlayerBlack, null);
        }

        GameManager.Instance.OnMarkerSelected(finalType);

        gameObject.SetActive(false);
    }
}
