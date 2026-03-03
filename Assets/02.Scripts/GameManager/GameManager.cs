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
    #region Fields & Properties
    // 착수금지 표시 sprite
    [SerializeField]
    private Sprite forbiddenSprite;
    
    // Game Logic
    private GomokuGameLogic _gameLogic;
    public GomokuGameLogic GameLogic => _gameLogic;

    // Board
    private Board _board;
    public Board Board => _board;
    
    // TurnStateManager
    private TurnStateManager _turnStateManager;
    public TurnStateManager TurnStateManager => _turnStateManager;

    // 게임의 종류 
    private GameType _gameType;
    public GameType GameType => _gameType;

    // 게임의 플레이어 타입
    private PlayerType _gamePlayerType;
    public PlayerType GamePlayerType => _gamePlayerType;
    
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
    #endregion

    #region Functions
    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        // TODO: 돌 색상 선택되면 돌의 정보와 함께 유저 닉네임과 급수 인게임 메인 UI에 띄워야
        // 유저 정보는 DB에서 꺼내 쓰기
        // 프로필 이미지는 아직 구현안돼서 null처리

        if (scene.name == SCENE_GAME)
        {
            InitGameScene();
            SoundManager.Instance.PlayBGM(BGM.SomniaVariation10);
        }
        else if (scene.name == SCENE_MAIN)
        {
            SoundManager.Instance.PlayBGM(BGM.MasCafe);
        }
    }

    private void InitGameScene()
    {
        _turnStateManager = FindFirstObjectByType<TurnStateManager>();
        _board = FindFirstObjectByType<Board>();
            
        // TurnManager가 씬에 없을 경우 생성
        if (_turnStateManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/TurnManager");
            
            if (prefab != null)
            {
                GameObject instance = Instantiate(prefab);
                instance.name = "TurnManager";
                
                _turnStateManager = instance.GetComponent<TurnStateManager>();

                if (_turnStateManager == null)
                {
                    Debug.Log("TurnManager is null!");
                }
            }
        }
        UIManager.Instance.OpenMarkerSelectPanel();
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
    public void NewGameLogic(PlayerType playerType = PlayerType.None)
    {
        if (_gameType != GameType.LocalDualPlay && _gameType != GameType.SinglePlay)
        {
            Debug.LogError("게임 타입 설정이 안 되어 SinglePlay로 설정. 현재 게임 타입: " + _gameType);
            _gameType = GameType.SinglePlay;
        }
        if (playerType != PlayerType.None)
        {
            Debug.Log("None이아님");
            _gamePlayerType = playerType;
        }
        _gameLogic = new GomokuGameLogic(_gameType, _gamePlayerType, _board, _turnStateManager);
        Debug.Log($"<color=yellow>GameManager에서 GomokuGameLogic 생성함: {_gameType} gameType</color>");

        // [추가] 기보 기록 시작
        string opponentName = (_gameType == GameType.SinglePlay) ? "알파고(AI)" : "Player 2";
        ReplayManager.Instance.StartRecording(_gameType, opponentName, "18", _gamePlayerType);
    }

    public void OnMarkerSelected(PlayerType finalType)
    {
        _gamePlayerType = finalType;

        if (UIManager.Instance.GamePanelController == null)
        {
            Debug.LogError("로직 생성 전 GamePanel 참조없음");
        }
        NewGameLogic();
    }
    #endregion
}