using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class GameManager : Singleton<GameManager>
{
    #region UI

    [SerializeField] private GameObject settingsPanelPrefab;
    [SerializeField] private GameObject confirmPanelPrefab;
    
    // 캔버스
    private Canvas _canvas;

    // 게임 화면의 UI 컨트롤러
    // private GamePanelController _gamePanelController; 
    
    // Game Turn UI 업데이트
    public void SetGameTurn(Constants.PlayerType playerTurnType)
    {
        // _gamePanelController.SetPlayerTurnPanel(playerTurnType);
    }

    // Settings 패널 열기
    public void OpenSettingsPanel()
    {
        var settingsPanelObject = Instantiate(settingsPanelPrefab, _canvas.transform);
        // settingsPanelObject.GetComponent<SettingsPanelController>().Show();
    }

    // Confirm 패널 열기
    public void OpenConfirmPanel(string message /* , ConfirmPanelController.OnConfirmButtonClicked onConfirmButtonClicked */)
    {
        var confirmPanelObject = Instantiate(confirmPanelPrefab, _canvas.transform);
        // confirmPanelObject.GetComponent<ConfirmPanelController>().Show(message, onConfirmButtonClicked);
    }

    #endregion

    #region Game

    // Game Logic
    private GomokuGameLogic _gameLogic;

    private Board _board;
    
    public Board Board => _board;

    // 게임의 종류 
    private GameType _gameType;

    // 플레이어 타입
    private PlayerType _playerType;
    
    private TurnStateManager _turnStateManager;

    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        // 새로운 씬에서 Canvas 참조 가져오기
        _canvas = FindFirstObjectByType<Canvas>();
        
        // 임시로 게임 타입, 플레이어 타입 설정(추후 수정하기)
        _gameType = GameType.LocalDualPlay;

        // 항상 흑이 먼저 게임 시작
        _playerType = PlayerType.Black;

        if (scene.name == SCENE_GAME)
        {
            _turnStateManager = FindFirstObjectByType<TurnStateManager>();
            _board = FindFirstObjectByType<Board>();

            if (_turnStateManager != null)

                // GamePanelController 참조 가져오기
                // _gamePanelController = FindFirstObjectByType<GamePanelController>();

                // Game Logic 생성
                _gameLogic = new GomokuGameLogic(_gameType, _playerType /*, _board*/, _turnStateManager);
        }
    }

    // 씬 전환 (Main > Game)
    public void ChangeToGameScene(GameType gameType)
    {
        _gameType = gameType;
        SceneManager.LoadScene(SCENE_GAME);       
    }

    // 씬 전환 (Game > Main)
    public void ChangeToMainScene()
    {
        SceneManager.LoadScene(SCENE_MAIN);
    }

    #endregion
    
}
