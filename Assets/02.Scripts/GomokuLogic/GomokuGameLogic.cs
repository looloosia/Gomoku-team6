using System.Collections;
using UnityEngine;
using static Constants;
using static GomokuLibrary;
public class GomokuGameLogic
{
    //public BlockController blockController;
    private PlayerType[,] board;

    public DemoBaseState playerAState;
    public DemoBaseState playerBState;

    private DemoBaseState currentState;

    private DemoTurnStateManager turnStateManager;
    public enum GameResult { None, Win, Lose, Draw };

    public PlayerType[,] Board { get { return board; } }

    private Coroutine counterRoutine = null;

    //승리, 패배 판정 용 임시 카운터 변수
    public int demoCounter = 5;
    
    
    //흑돌인지 백돌인지 선택(흑: 선, 백: 후)
    public GomokuGameLogic(GameType gameType, PlayerType playerType/*, BlockController blockController*/,DemoTurnStateManager turnStateManager)
    {
        //this.blockController = blockController;
        board = new PlayerType[BOARD_SIZE, BOARD_SIZE];

        this.turnStateManager = turnStateManager;
        //turnStateManager.endGameDelegate = 
        PlayerType otherPlayerType = playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black; //상대방 타입(멀티플레이일 경우 게스트)
        
        switch (gameType)
        {
            case GameType.LocalDualPlay:
                playerAState = new DemoBaseState(playerType);
                playerBState = new DemoBaseState(otherPlayerType);
                ////흑돌인 Player먼저 시작
                SetState(playerAState);
                break;
            case GameType.SinglePlay:
                //playerAState = new PlayerState(true);
                //playerBState = new AIState(false);

                ////첫 턴인 Player먼저 시작
                //SetState(playerAState);
                break;

        }
    }

    //턴 혹은 상태가 바뀔 때 호출되는 메서드
    public void SetState(DemoBaseState newState) //델리게이트 호출
    {
        currentState?.OnExit(this);//기존 스테이트는 끝
        currentState = newState;
        currentState?.OnEnter(this);//새로운 스테이트 시작
    }

    //public bool PlaceMarker(int index, PlayerType playerType)
    //{
    //    int row = index / BOARD_SIZE;
    //    int col = index % BOARD_SIZE;
    //    if (board[row, col] != Constants.PlayerType.None)
    //        return false;

    //    blockController.PlaceMarker(index, playerType);
    //    board[row, col] = playerType;
    //    return true;
    //}

    //턴 변경
    public void ChangeGameState()
    {
        Debug.Log("ChangeGameState");

        if (currentState == playerAState)
        {
            currentState = playerBState;
            turnStateManager.SetState(playerBState);
        }
        else
        {
            currentState = playerAState;
            turnStateManager. SetState(playerAState);
        }
        SetState(currentState);
    }

    public GameResult CheckGameResult(Constants.PlayerType playerType, int inRow, int inCol) //인풋: 바둑돌 놓은 좌표 
    {
        //승리 조건 확인 로직 구현
        if (CheckGameWin(PlayerType.Black, board, inRow, inCol))
        {
            return GameResult.Win;
        }

        if (CheckGameWin(PlayerType.White, board, inRow, inCol))
        {
            return GameResult.Lose;
        }
        
        return GameResult.None;
    }

    public void EndGame(/*GameResult gameResult,*/ DemoBaseState playerState) 
    {

        //string resultString = "";
        if(playerAState == playerState)
        {
            Debug.Log(playerAState.Type + " win");
            Debug.Log(playerBState.Type + " lose");
        }
        else
        {
            Debug.Log(playerBState.Type + " win");
            Debug.Log(playerAState.Type + " lose");
        }
        //switch (gameResult)
        //{
        //    case GameResult.Win:
        //        resultString = "Player1 승리";
        //        break;
        //    case GameResult.Lose:
        //        resultString = "Player2 승리";
        //        break;
        //}
        //GameManager.Instance.OpenConfirmPanel(resultString, () => { GameManager.Instance.ChangeToMainScene(); });
    }

    public bool RuleChecker()
    {
        return true;
    }

    //IEnumerator CounterRoutine(PlayerType playerType)
    //{
    //    int timeLimit = TIME_LIMIT;

    //    while (timeLimit>0)
    //    {
    //        timeLimit--;
    //        //TODO: 타이머 UI호출 함수에 매개변수로 timeLimit보내기
    //        yield return new WaitForSeconds(1f);
    //    }
    //    //TODO: 반복문 끝나면 현재 유저가 졌다고 판정
    //}
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
public class DemoBaseState
{
    private PlayerType playerType;
    public PlayerType Type { get { return playerType; }}

    public DemoBaseState(PlayerType playerType)
    {
        this.playerType = playerType;
        Debug.Log(playerType+"생성");

    }
    public  void OnEnter(GomokuGameLogic gameLogic)
    {
        gameLogic.demoCounter--;
        if (gameLogic.demoCounter ==0)
        {
            gameLogic.EndGame(this);
            return;
        }
        gameLogic.ChangeGameState();
        
        Debug.Log($"{this} turn Enter");
    }
    public void HandleMove(GomokuGameLogic gameLogic, int index)
    {
        Debug.Log("HandleMove");
        
    }
    public void OnExit(GomokuGameLogic gameLogic)
    {
        Debug.Log("OnExit");
    }
    public void HandleNextTurn(GomokuGameLogic gameLogic)
    {
        Debug.Log("HandleNextTurn");
    }

    public void ProcessMove(GomokuGameLogic gameLogic, Constants.PlayerType playerType, int inRow, int inCol)
    {
        //특정 위치 마커 표시
        //    if (gameLogic.RuleChecker())
        //    {
        //        GomokuGameLogic.GameResult gameResult = gameLogic.CheckGameResult(playerType, inRow, inCol);

        //        if (gameResult == GomokuGameLogic.GameResult.None)
        //        {

        //            HandleNextTurn(gameLogic);
        //        }
        //        else
        //        {
        //            gameLogic.EndGame(gameResult, playerType);

        //        }
        //    }
        Debug.Log("ProcessMove");
    }
    
}