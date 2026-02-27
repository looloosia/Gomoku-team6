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
    #region UI

    [SerializeField] private GameObject settingsPopupPrefab;
    [SerializeField] private GameObject confirmPopupPrefab;
    [SerializeField] private GameObject markerSelectPanelPrefab;
    
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
        Debug.Log("GameManager: OpenSettingPopup() 실행됨");
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
        Debug.Log("GameManager: OpenSettingPopup() 실행됨");
        if (_canvas == null)
        {
            _canvas = FindFirstObjectByType<Canvas>();
        }
        var confirmPanelObject = Instantiate(confirmPopupPrefab, _canvas.transform);
        return confirmPanelObject.GetComponent<ConfirmPopup>();
    }

    public MarkerSelectPanelController OpenMarkerSelectPanel()
    {
        Debug.Log("GameManager: OpenMarkerSelectPopup() 실행됨");
        if (_canvas == null)
        {
            _canvas = FindFirstObjectByType<Canvas>();
        }
        var markerPanelObject = Instantiate(markerSelectPanelPrefab, _canvas.transform);
        markerPanelObject.name = "[Panel] Marker Select";
        return markerPanelObject.GetComponent<MarkerSelectPanelController>();
    }

    #endregion

    #region Game
    
    // temp(게임 씬에서 바로 테스트 위한 변수)
    private bool _isStartedinMain = false;
    public bool IsStartedinMain =>  _isStartedinMain;
    
    
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
    private PlayerType _currentState;
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

    // 게임 씬에서 시작할 때를 위한 테스트용(추후 삭제예정) ===========================
    // void Start()
    // {
    //     Debug.Log("GameManager: Start()");
    //     // Start 시 Main 씬인지 확인(메인 hierarchy에만 GameManager 오브젝트 있어서)
    //     _isStartedinMain = SceneManager.GetActiveScene().name == SCENE_MAIN;
    //     
    //     if (_isStartedinMain)
    //         return;
    //     
    //     _canvas = FindFirstObjectByType<Canvas>();
    //     if (_canvas == null)
    //     {
    //         Debug.LogError("Canvas is null!");
    //     }
    //     
    //     InitGameScene();
    // }
    // ================================
    
    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("GameManager: OnSceneLoad()");
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
            _gamePanelController = FindFirstObjectByType<GamePanelController>();
            
            GameObject prefab = Resources.Load<GameObject>("Prefabs/TurnManager");
                
            MarkerSelectPanelController markerController = OpenMarkerSelectPanel();
            if (markerController == null)
            {
                Debug.Log("Marker Controller is null!");
            }
            else
            {
                markerController.OnMarkerSelectedEvent += OnMarkerSelected; 
            }
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
        
        // _forbiddensVisualizer = FindFirstObjectByType<ForbiddensVisualizer>();
        //
        // if (_forbiddensVisualizer != null)
        // {
        //     _forbiddensVisualizer.Init(forbiddenSprite, _gameLogic);
        // }
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

    public void NewGameLogic()
    {
        // GomokuGameLogic 생성
        _gameLogic = new GomokuGameLogic(_gameType, _gamePlayerType, _board, _turnStateManager);
        Debug.Log("GameManager에서 _gameLogic 생성함");
    }

    public void OnMarkerSelected(MarkerChoice markerChoice)
    {
        Debug.Log("GameManager: OnMarkerSelected() 실행됨");
        // TODO: 유저가 돌을 선택했을 때 실행될 것

        // random일 경우 랜덤 선택
        if (markerChoice == MarkerChoice.Random)
        {
            int ranNum = Random.Range(1, 3);
            _gamePlayerType = (PlayerType)ranNum;
        }
        else
        {
            _gamePlayerType = (PlayerType)markerChoice;
        }
        
        bool isPlayerBlack = _gamePlayerType == PlayerType.Black; 
        
        NewGameLogic();
    }

    #endregion
    
}