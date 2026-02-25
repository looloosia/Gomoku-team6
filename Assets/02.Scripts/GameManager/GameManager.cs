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
    public GamePanelController GamePanelController => _gamePanelController;
    
    // Game Turn UI 업데이트
    public void SetGameTurn(Constants.PlayerType playerTurnType)
    {
        // _gamePanelController.SetPlayerTurnPanel(playerTurnType);
    }

    // Settings 팝업 열기
    public void OpenSettingPopup()
    {
        if (_canvas == null)
        {
            _canvas = FindFirstObjectByType<Canvas>();
        }
        var settingsPopupObject = Instantiate(settingsPopupPrefab, _canvas.transform);
        settingsPopupObject.GetComponent<SettingPopup>();
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
    
    // 착수금지 표시 sprite
    [SerializeField]
    private Sprite forbiddenSprite;
    
    // Game Logic
    private GomokuGameLogic _gameLogic;
    public GomokuGameLogic GameLogic => _gameLogic;

    // Board
    private Board _board;
    public Board Board => _board;
    
    // ForbiddenVisualizer
    private ForbiddensVisualizer _forbiddensVisualizer;
    public ForbiddensVisualizer ForbiddensVisualizer => _forbiddensVisualizer;
    
    // TurnStateManager
    private TurnStateManager _turnStateManager;
    public TurnStateManager TurnStateManager => _turnStateManager;

    // 게임의 종류 
    private GameType _gameType;
    public GameType GameType => _gameType;

    // 플레이어 타입
    private PlayerType _playerType;
    public PlayerType PlayerType => _playerType;

    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        // 새로운 씬에서 Canvas 참조 가져오기
        _canvas = FindFirstObjectByType<Canvas>();
        
        // TODO: 게임타입, 플레이어타입 설정기능 완성되면 설정된 대로 하게 수정
        _gameType = GameType.LocalDualPlay;

        // TODO: 플레이어의 흑백 설정에 맞게 수정
        _playerType = PlayerType.Black;

        if (scene.name == SCENE_GAME)
        {
            _turnStateManager = FindFirstObjectByType<TurnStateManager>();
            
            // TurnStateManager가 씬에 없을 경우 생성
            if (_turnStateManager == null)
            {
                GameObject prefab = Resources.Load<GameObject>("Prefabs/TurnManager");
                
                if (prefab != null)
                {
                    GameObject instance = Instantiate(prefab);
                    instance.name = "TurnManager";
                
                    _turnStateManager = instance.GetComponent<TurnStateManager>();
                    _forbiddensVisualizer = instance.GetComponentInChildren<ForbiddensVisualizer>();
                }
            }
            
            _forbiddensVisualizer = FindFirstObjectByType<ForbiddensVisualizer>();
            _board = FindFirstObjectByType<Board>();
            
            // GomokuGameLogic 생성
            _gameLogic = new GomokuGameLogic(_gameType, _playerType/*, _board*/, _turnStateManager);
            
            if (_turnStateManager != null)
            {
                // GamePanelController 참조 가져오기
                _gamePanelController = FindFirstObjectByType<GamePanelController>();
            }

            if (_forbiddensVisualizer != null)
            {
                _forbiddensVisualizer.Init(forbiddenSprite);
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