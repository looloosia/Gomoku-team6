using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static Constants;
using static GomokuLibrary;

public class GomokuGameLogic
{
    private PlayerType[,] virtualBoard;
    public PlayerType[,] VirtualBoard { get { return virtualBoard; } }

    public Action<int, int> onBlockClicked;//스테이트가 받을 액션
    bool isStart = false;

    public BaseState playerAState;
    public BaseState playerBState;

    private Board gomokuBoard;

    private BaseState currentState;

    private TurnStateManager turnStateManager;


    private Coroutine counterRoutine = null;

    //승리, 패배 판정 용 임시 카운터 변수(삭제예정)
    public int demoCounter = 5;


    //흑돌인지 백돌인지 선택(흑: 선, 백: 후)
    public GomokuGameLogic(GameType gameType, PlayerType playerType, Board gomokuBoard, TurnStateManager turnStateManager)
    {
        virtualBoard = new PlayerType[BOARD_SIZE, BOARD_SIZE];

        this.turnStateManager = turnStateManager;
        this.turnStateManager.onEndGame += EndGame;

        if (gomokuBoard != null)
        {

            this.gomokuBoard = gomokuBoard;
            gomokuBoard.onPlaceStone += OnBlockClicked;
        }
        else
        {
            Debug.LogError("유효한 보드가 없습니다");
        }

        PlayerType otherPlayerType = playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black; //상대방 타입(멀티플레이일 경우 게스트)

        //TODO: stoneonclick에서 쏠 액션을 받을 함수 연결, (state가 연결할 함수)

        switch (gameType)
        {
            case GameType.LocalDualPlay:
                playerAState = new PlayerState(playerType);
                playerBState = new PlayerState(otherPlayerType);
                ////흑돌인 Player먼저 시작
                StartFirstState();
                break;

            case GameType.SinglePlay:
                playerAState = new PlayerState(playerType);
                playerBState = new _02.Scripts.States.AIState(otherPlayerType);

                //첫 턴인 Player먼저 시작
                StartFirstState();
                break;

        }
    }

    public void OnBlockClicked(Block block)
    {
        Debug.Log("ONBLOCKCLICKED");
        int row = block.GetBlockData().row;
        int col = block.GetBlockData().col;

        onBlockClicked?.Invoke(row, col);
    }

    private void StartFirstState()
    {
        if (playerAState.Type == Constants.PlayerType.Black)
        {
            SetState(playerAState);
        }
        else
        {
            SetState(playerBState);
        }
        isStart = true;
    }

    //턴 혹은 상태가 바뀔 때 호출되는 메서드
    public void SetState(BaseState newState)
    {

        currentState?.OnExit(this); //기존 스테이트 끝
        currentState = newState;
        currentState?.OnEnter(this);    //새로운 스테이트 시작

        if (isStart)
        {
            gomokuBoard.UpdateBlock();
            isStart = true;
        }
        Debug.Log($"{currentState.Type}: CURRENTONENTER");
        gomokuBoard.SetCurrentStone(currentState.Type);

        //금수 자리 해제 및 새로 체크
        ClearForbiddenPositionCheck(virtualBoard);
        if (currentState.Type == PlayerType.Black)
            CheckForbiddenPostions(virtualBoard, currentState.Type, BOARD_SIZE);

        turnStateManager.SetState(newState);
    }

    public bool PlaceMarker(PlayerType playerType, int inRow, int inCol)
    {
        if (virtualBoard[inRow, inCol] != Constants.PlayerType.None) //무엇인가 있는 경우
        {
            if (virtualBoard[inRow, inCol] == Constants.PlayerType.Forbidden)
            {
                Debug.Log("금수 자리");
            }
            else
                Debug.Log("빈 칸에 돌을 놓아주세요");
            return false;
        }
        //if (RuleChecker(playerType, inRow, inCol) != ForbiddenType.None)
        //    return false;

        virtualBoard[inRow, inCol] = playerType;
        return true;
    }

    //턴 변경
    public void ChangeGameState()
    {

        if (currentState == playerAState)
        {
            SetState(playerBState);
        }
        else
        {
            SetState(playerAState);
        }
    }

    public Constants.GameResult CheckGameResult(Constants.PlayerType playerType, int inRow, int inCol) //인풋: 바둑돌 놓은 좌표 
    {
        //승리 조건 확인 로직 구현
        if (CheckGomoku(virtualBoard, playerType, inRow, inCol, 15))
        {
            return Constants.GameResult.Win;
        }

        return GameResult.None;
    }
    public void EndGame(BaseState playerState, Constants.GameResult gameResult)
    {

        if (gameResult == Constants.GameResult.Win) //승리
        {
            if (playerState == playerAState)
            {
                Debug.Log("A 승리");
                Debug.Log("B 패배");
            }
            else
            {
                Debug.Log("B 승리");
                Debug.Log("A 패배");
            }
        }

        else if (gameResult == Constants.GameResult.Lose) //패배(시간 초과 등)
        {
            if (playerState == playerAState)
            {
                Debug.Log("B 승리");
                Debug.Log("A 패배");
            }
            else
            {
                Debug.Log("A 승리");
                Debug.Log("B 패배");
            }
        }

        //TurnStateManager에서 아예 다 꺼버리는 함수 호출
        turnStateManager.onEndGame -= EndGame;
        //Board의 Action 해제.

        //GameManager.Instance.OpenConfirmPanel(resultString, () => { GameManager.Instance.ChangeToMainScene(); });
    }

    //public ForbiddenType RuleChecker(Constants.PlayerType playerType, int inRow, int inCol)
    //{
    //    //3목인 경우 return ForbiddenType.Three;
    //    //4목인 경우 return ForbiddenType.Four;
    //    //장목인 경우 return ForbiddenType.Long;


    //    return ForbiddenType.None;
    //}
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
//public class DemoBaseState
//{
//    private PlayerType playerType;
//    public PlayerType Type { get { return playerType; }}

//    public DemoBaseState(PlayerType playerType)
//    {
//        this.playerType = playerType;
//        Debug.Log(playerType+"생성");

//    }
//    public  void OnEnter(GomokuGameLogic gameLogic)
//    {
//        gameLogic.demoCounter--;
//        if (gameLogic.demoCounter ==0)
//        {
//            //gameLogic.EndGame(this);
//            return;
//        }

//        Debug.Log($"{this} turn Enter");
//    }
//    public void HandleMove(GomokuGameLogic gameLogic, int index)
//    {
//        Debug.Log("HandleMove");

//    }
//    public void OnExit(GomokuGameLogic gameLogic)
//    {
//        Debug.Log("OnExit");
//    }
//    public void HandleNextTurn(GomokuGameLogic gameLogic)
//    {
//        Debug.Log("HandleNextTurn");
//        gameLogic.ChangeGameState();

//    }

//    public void ProcessMove(GomokuGameLogic gameLogic, Constants.PlayerType playerType, int inRow, int inCol)
//    {
//        Debug.Log("ProcessMove");
//        //룰 확인
//        if (gameLogic.PlaceMarker(playerType, inRow, inCol))
//        {
//            Constants.GameResult gameResult = gameLogic.CheckGameResult(playerType, inRow, inCol);

//            if (gameResult == Constants.GameResult.None)
//            {
//                HandleNextTurn(gameLogic);
//            }
//            else
//            {
//                gameLogic.EndGame(this, gameResult);
//            }
//        }
//    }

//}