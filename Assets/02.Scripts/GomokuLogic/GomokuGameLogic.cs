using System.Collections;
using UnityEngine;
using static Constants;
using static GomokuLibrary;
public class GomokuGameLogic
{
    //public BlockController blockController;
    private PlayerType[,] board;

    //public BaseState playerAState;
    //public BaseState playerBState;

    //private BaseState currentState;

    public enum GameResult { None, Win, Lose, Draw };

    public Constants.PlayerType[,] Board { get { return board; } }

    //1p 2p가 흑돌인지 백돌인지 선택
    public GomokuGameLogic(Constants.GameType gameType/*, BlockController blockController*/)
    {
        //this.blockController = blockController;
        board = new PlayerType[BOARD_SIZE, BOARD_SIZE];

        switch (gameType)
        {
            case GameType.SinglePlay:
                //playerAState = new PlayerState(true);
                //playerBState = new PlayerState(false);

                ////첫 턴인 Player먼저 시작
                //SetState(playerAState);
                break;
            case GameType.LocalDualPlay:
                //playerAState = new PlayerState(true);
                //playerBState = new AIState(false);

                ////첫 턴인 Player먼저 시작
                //SetState(playerAState);
                break;

        }
    }
    //턴 혹은 상태가 바뀔 때 호출되는 메서드
    //public void SetState(BaseState newState)
    //{
    //    currentState?.OnExit(this);//기존 스테이트는 끝
    //    currentState = newState;
    //    currentState?.OnEnter(this);//새로운 스테이트 시작

    //}

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
    //public void ChangeGameState()
    //{
    //    if (currentState == playerAState)
    //    {
    //        SetState(playerBState);
    //    }
    //    else
    //    {
    //        SetState(playerAState);
    //    }
    //}

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

    public void EndGame(GameResult gameResult, PlayerType playerType)
    {

        string resultString = "";

        switch (gameResult)
        {
            case GameResult.Win:
                resultString = "Player1 승리";
                break;
            case GameResult.Lose:
                resultString = "Player2 승리";
                break;
        }
        //GameManager.Instance.OpenConfirmPanel(resultString, () => { GameManager.Instance.ChangeToMainScene(); });
    }

    //IEnumerator Counter(PlayerType playerType)
    //{
    //    while()
    //    {

    //    }
    //}
}
