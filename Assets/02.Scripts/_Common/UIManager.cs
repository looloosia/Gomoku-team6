using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [Header("UI Prefabs (Popup)")]
    [SerializeField] private GameObject settingsPopupPrefab;
    [SerializeField] private GameObject confirmPopupPrefab;
    [SerializeField] private GameObject markerSelectPanelPrefab;

    [Header("Canvas Hierarchy")]
    [SerializeField] private Transform popupCanvasTransform;

    private GamePanelController _gamePanelController; 
    public GamePanelController GamePanelController => _gamePanelController;

    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        // 팝업 캔버스는 UIManager가 직접 들고 다니니까 놔두고,
        // 씬 안에 배치되어 있는 메인 게임 패널만 여기서 새로 찾아줍니다!
        if (scene.name == Constants.SCENE_GAME)
        {
            _gamePanelController = FindFirstObjectByType<GamePanelController>();
        }
    }

    public void OpenSettingPopup()
    {
        // 팝업 전용 캔버스(popupCanvasTransform)를 부모로 하여 생성
        var settingsPopupObject = Instantiate(settingsPopupPrefab, popupCanvasTransform);
        settingsPopupObject.GetComponent<SettingPopup>();
    }

    public ConfirmPopup OpenConfirmPopup()
    {
        var confirmPanelObject = Instantiate(confirmPopupPrefab, popupCanvasTransform);
        return confirmPanelObject.GetComponent<ConfirmPopup>();
    }

    public MarkerSelectPanelController OpenMarkerSelectPanel()
    {
        var markerPanelObject = Instantiate(markerSelectPanelPrefab, popupCanvasTransform);
        markerPanelObject.name = "[Panel] Marker Select";
        return markerPanelObject.GetComponent<MarkerSelectPanelController>();
    }
}