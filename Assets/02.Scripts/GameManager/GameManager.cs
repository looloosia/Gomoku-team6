using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static Constants;

/// <summary>
/// forbiddensVisualizer 임시 주석처리
/// </summary>
public class GameManager : Singleton<GameManager>
{
    // 캔버스
    private Canvas _canvas;
    
    // private GamePanelController _gamePanelController;
    
    // Game Turn UI 업데이트
    // public void SetGameTurn(Constants.PlayerType playerTurnType)
    // {
    //     _gamePanelController.SetPlayerTurnPanel(playerTurnType);
    // }
    
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
    // private ForbiddensVisualizer _forbiddensVisualizer;
    // public ForbiddensVisualizer ForbiddensVisualizer => _forbiddensVisualizer;
    
    // TurnStateManager
    private TurnStateManager _turnStateManager;
    public TurnStateManager TurnStateManager => _turnStateManager;

    // 게임의 종류 
    private GameType _gameType;
    public GameType GameType => _gameType;

    // 게임의 플레이어 타입
    private PlayerType _gamePlayerType;
    public PlayerType GamePlayerType => _gamePlayerType;
    
    // 현재 턴 플레이어 타입
    // private PlayerType _currentState;
    // public PlayerType CurrentPlayerType =>
    //     (_gameLogic?.CurrentState != null) 
    //         ? _gameLogic.CurrentState.Type 
    //         : PlayerType.None;
    
    // AI의 이름
    private string _aiName;
    public string AIName
    {
        get { return _aiName; }
        set { _aiName = value; }
    }
    
    // AI의 랭크 -나중에 string이 아닌 다른 타입으로 바꿀수도
    private string _aiRank;
    public string AIRank
    {
        get { return _aiRank; }
        set { _aiRank = value; }
    }
    
    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("GameManager: OnSceneLoad() 실행됨");
        // 새로운 씬에서 Canvas 참조 가져오기
        _canvas = FindFirstObjectByType<Canvas>();
        
        // TODO: 게임타입, 플레이어타입 설정기능 완성되면 설정된 대로 하게 수정
        _gameType = GameType.LocalDualPlay;
        
        // TODO: 돌 색상 선택되면 돌의 정보와 함께 유저 닉네임과 급수 인게임 메인 UI에 띄워야
        // 유저 정보는 DB에서 꺼내 쓰기
        // 프로필 이미지는 아직 구현안돼서 null처리

        if (scene.name == SCENE_GAME)
        {
            InitGameScene();
        }
    }

    private void InitGameScene()
    {
        _turnStateManager = FindFirstObjectByType<TurnStateManager>();
        _board = FindFirstObjectByType<Board>();
        
        if (_canvas == null)
        {
            Debug.LogError("Canvas is null!");
        }
            
        // TurnManager가 씬에 없을 경우 생성
        if (_turnStateManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/TurnManager");
            
            if (prefab != null)
            {
                GameObject instance = Instantiate(prefab);
                instance.name = "TurnManager";
                
                _turnStateManager = instance.GetComponent<TurnStateManager>();
                // _forbiddensVisualizer = instance.GetComponentInChildren<ForbiddensVisualizer>();

                if (_turnStateManager == null)
                {
                    Debug.Log("TurnManager is null!");
                }
            }
        }
        
        UIManager.Instance.OpenMarkerSelectPanel();
        
        // TODO: 금수표시관련
        // _forbiddensVisualizer = FindFirstObjectByType<ForbiddensVisualizer>();
        //
        // if (_forbiddensVisualizer != null)
        // {
        //     _forbiddensVisualizer.Init(forbiddenSprite, _gameLogic);
        // }
    }

    // 씬 전환 (Game으로)
    public void ChangeToGameScene(GameType gameType)
    {
        _gameType = gameType;
        SceneManager.LoadScene(SCENE_GAME);       
    }

    // 씬 전환 (Main으롤)
    public void ChangeToMainScene()
    {
        SceneManager.LoadScene(SCENE_MAIN);
    }

    // 씬 전환 (Lobby으로)
    public void ChangeToLobbyScene()
    {
        SceneManager.LoadScene(SCENE_LOBBY);
    }
    
    // GomokuGameLogic 생성
    public void NewGameLogic()
    {
        _gameLogic = new GomokuGameLogic(_gameType, _gamePlayerType, _board, _turnStateManager);
        Debug.Log("<color=yellow>GameManager에서 GomokuGameLogic 생성함</color>");
    }

    public void OnMarkerSelected(MarkerChoice markerChoice)
    {
        NewGameLogic();
    }
}