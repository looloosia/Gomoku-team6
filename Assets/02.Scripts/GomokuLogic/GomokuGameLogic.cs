using System;
using UnityEngine;
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
            Debug.LogError("Valid board doesn't exist.");
        }

        PlayerType otherPlayerType = playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black; //상대 타입
        SoundManager.Instance.PlaySFX(SFX.JingE);

        switch (gameType)
        {
            case GameType.LocalDualPlay:
                playerAState = new PlayerState(playerType);
                playerBState = new PlayerState(otherPlayerType);
                //흑돌인 Player먼저 시작
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
        //Debug.Log("ONBLOCKCLICKED");
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
        turnStateManager.SetState(currentState);
        currentState?.OnEnter(this);    //새로운 스테이트 시작

        //Debug.Log($"{currentState.Type}: CURRENTONENTER");

        gomokuBoard.SetCurrentStone(currentState.Type);

        //금수 자리 해제 및 새로 체크
        ClearForbiddenPositionCheck(virtualBoard);
        if (currentState.Type == PlayerType.Black)
            CheckForbiddenPostions(virtualBoard, currentState.Type, BOARD_SIZE);

        //현 블록 상태 표시
        gomokuBoard.UpdateBlock(virtualBoard);
        isStart = true;

    }
    public void CaptureFrame()
    {
        gomokuBoard.UpdateBlock(virtualBoard);
        gomokuBoard.SaveReplayFrame();
    }

    public bool PlaceMarker(PlayerType playerType, int inRow, int inCol)
    {
        if (virtualBoard[inRow, inCol] != Constants.PlayerType.None) //무엇인가 있는 경우
        {
            if (virtualBoard[inRow, inCol] == Constants.PlayerType.Forbidden)
            {
                //Debug.Log("금수 자리");
            }
            else
            {
                //Debug.Log("빈 칸에 돌을 놓아주세요");
            }
            return false;
        }

        virtualBoard[inRow, inCol] = playerType;
        return true;
    }

    //턴 변경
    public void ChangeGameState()
    {
        SoundManager.Instance.PlaySFX(SFX.baduck_button_click);
        if (currentState == playerAState)
        {
            SetState(playerBState);
        }
        else
        {
            SetState(playerAState);
        }
    }

    public Constants.GameResult CheckGameResult(Constants.PlayerType playerType, int inRow, int inCol) //��ǲ: �ٵϵ� ���� ��ǥ 
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
        SoundManager.Instance.PlaySFX(SFX.JingE);
        string colorOfWinner = "";
        string title = "";
        string subText = "";
        PlayerType winnerType;
        Constants.GameResultType resultType = GameResultType.ConnectFive;
        if (gameResult == Constants.GameResult.Win) //승리
        {
            colorOfWinner = playerState.Type == PlayerType.Black ? "흑" : "백"; //승리자 색
            winnerType = playerState.Type;
            title = colorOfWinner + " 승리!";
            subText = title + "오목을 완성하여 승리하였습니다.";
            //gomokuBoard.UpdateBlock(virtualBoard);
           // gomokuBoard.SetCurrentStone(winnerType);
            resultType = GameResultType.ConnectFive;
        }

        else if (gameResult == Constants.GameResult.Lose) //패배(시간 초과 등)
        {
            colorOfWinner = playerState.Type == PlayerType.Black ? "백" : "흑"; //패배자의 상태 반대 색
            winnerType = playerState.Type == PlayerType.Black ? PlayerType.White : PlayerType.Black;
            title = colorOfWinner + " 승리!";

            if (playerState.ControllerType != ControllerType.AI)
            {
                subText = title + "시간 초과로 승리하였습니다.";
                //gomokuBoard.UpdateBlock(virtualBoard);
                //gomokuBoard.SetCurrentStone(winnerType);
                resultType = GameResultType.TimeOut;
            }
            else
            {
                subText = title + "AI가 포기하였습니다!";
               // gomokuBoard.UpdateBlock(virtualBoard);
               // gomokuBoard.SetCurrentStone(winnerType);
                resultType = GameResultType.Surrender;
            }
        }

        // 기보 최종 포장 및 데이터 넘겨주기 위한 코드
        ReplaySaveData finalRecord = ReplayManager.Instance.GetFinalRecord(
        gameResult,
        playerState.Type,
        resultType
        );

        // 기존 승패 결과 팝업창에게 정보를 넘기기 위한 코드
        GameResultController resultController = UnityEngine.Object.FindFirstObjectByType<GameResultController>();
        if (resultController != null)
        {
            resultController.StartResultFlow(title, subText, finalRecord);
        }
        else
        {
            GameManager.Instance.ChangeToLobbyScene();
        }


        turnStateManager.onEndGame -= EndGame;
        gomokuBoard.onPlaceStone -= OnBlockClicked;

    }


}