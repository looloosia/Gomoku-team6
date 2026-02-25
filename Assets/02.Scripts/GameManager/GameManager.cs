using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static Constants;

public class GameManager : Singleton<GameManager>
{
    #region UI

    [SerializeField] private GameObject settingsPopupPrefab;
    [SerializeField] private GameObject confirmPopupPrefab;
    
    // 캔버스
    private Canvas _canvas;

    // 게임 화면의 UI 컨트롤러
    private GamePanelController _gamePanelController; 
    
    // Game Turn UI 업데이트
    public void SetGameTurn(Constants.PlayerType playerTurnType)
    {
        // _gamePanelController.SetPlayerTurnPanel(playerTurnType);
    }

    // Settings 팝업 열기
    public SettingPopup OpenSettingPopup()
    {
        if (_canvas == null)
        {
            _canvas = FindFirstObjectByType<Canvas>();
        }
        var settingsPopupObject = Instantiate(settingsPopupPrefab, _canvas.transform);
        return settingsPopupObject.GetComponent<SettingPopup>();
    }

    // Confirm 팝업 열기
    public ConfirmPopup OpenConfirmPopup()
    {
        if (_canvas == null)
        {
            _canvas = FindFirstObjectByType<Canvas>();
        }
        var confirmPanelObject = Instantiate(confirmPopupPrefab, _canvas.transform);
        return confirmPanelObject.GetComponent<ConfirmPopup>();
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
            
            if (_turnStateManager != null)
            {
                // GamePanelController 참조 가져오기
                _gamePanelController = FindFirstObjectByType<GamePanelController>();

                // TODO: Game Logic 생성
                _gameLogic = new GomokuGameLogic(_gameType, _playerType /*, _board*/, _turnStateManager);
            }
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
