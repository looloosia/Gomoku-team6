using System.Collections;
using UnityEngine;
using static Constants;
using static GomokuLibrary;

public class GomokuGameLogic
{
    //public BlockController blockController;
    private PlayerType[,] board;

    public BaseState playerAState;
    public BaseState playerBState;

    private BaseState currentState;
    public BaseState CurrentState => currentState;

    private Board _tBoard;

    private TurnStateManager turnStateManager;

    public PlayerType[,] Board { get { return board; } }

    private Coroutine counterRoutine = null;

    //�¸�, �й� ���� �� �ӽ� ī���� ����(��������)
    public int demoCounter = 5;
    
    
    //�浹���� �鵹���� ����(��: ��, ��: ��)
    public GomokuGameLogic(GameType gameType, PlayerType playerType, Board tBoard,TurnStateManager turnStateManager)
    {
        _tBoard = tBoard;
        board = new PlayerType[BOARD_SIZE, BOARD_SIZE];

        this.turnStateManager = turnStateManager;
        this.turnStateManager.onEndGame += EndGame;

        PlayerType otherPlayerType = playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black; //���� Ÿ��(��Ƽ�÷����� ��� �Խ�Ʈ)
        
        switch (gameType)
        {
            case GameType.LocalDualPlay:
                playerAState = new PlayerState(playerType);
                playerBState = new PlayerState(otherPlayerType);
                ////�浹�� Player���� ����
                StartFirstState();               
                break;

            //case GameType.SinglePlay:
            //    playerAState = new PlayerState(playerType);
            //    playerBState = new AIState(otherPlayerType);

                ////ù ���� Player���� ����
                //StartFirstState();
                //break;

        }
    }


    private void StartFirstState()
    {
        Debug.Log("StartFirstState");
        if (playerAState.Type == Constants.PlayerType.Black)
        {
            SetState(playerAState);
        }
        else
        {
            SetState(playerBState);
        }
    }

    //�� Ȥ�� ���°� �ٲ� �� ȣ��Ǵ� �޼���
    public void SetState(BaseState newState)
    {
        currentState?.OnExit(this); //���� ������Ʈ ��
        currentState = newState;
        currentState?.OnEnter(this);    //���ο� ������Ʈ ����

        turnStateManager.SetState(newState);
    }

    public bool PlaceMarker(PlayerType playerType, int inRow, int inCol)
    {
        if (board[inRow, inCol] != Constants.PlayerType.None) //�����ΰ� �ִ� ���
        {
            Debug.Log("�� ĭ�� ���� �����ּ���");
            return false;
        }
        if (RuleChecker(playerType, inRow, inCol) != ForbiddenType.None)
            return false;
        
        //blockController.PlaceMarker(index, playerType);
        board[inRow, inCol] = playerType;
        return true;
    }

    //�� ����
    public void ChangeGameState()
    {
        Debug.Log("ChangeGameState");
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
        //�¸� ���� Ȯ�� ���� ����
        //if (CheckGameWin(board, PlayerType.Black,inRow, inCol))
        //{
        //    return Constants.GameResult.Win;
        //}

        //if (CheckGameWin(board, PlayerType.White,  inRow, inCol))
        //{
        //    return Constants.GameResult.Lose;
        //}
        
        return GameResult.None;
    }
    public void EndGame(BaseState playerState, Constants.GameResult gameResult) 
    {
        
        if(gameResult == Constants.GameResult.Win) //�¸�
        {
            if(playerState == playerAState)
            {
                Debug.Log("A �¸�");
                Debug.Log("B �й�");
            }
            else
            {
                Debug.Log("B �¸�");
                Debug.Log("A �й�");
            }
        }

        else if (gameResult == Constants.GameResult.Lose) //�й�(�ð� �ʰ� ��)
        {
            if (playerState == playerAState)
            {
                Debug.Log("B �¸�");
                Debug.Log("A �й�");
            }
            else
            {
                Debug.Log("A �¸�");
                Debug.Log("B �й�");
            }
        }
        
        //TurnStateManager���� �ƿ� �� �������� �Լ� ȣ��
        turnStateManager.onEndGame -= EndGame;

        //GameManager.Instance.OpenConfirmPanel(resultString, () => { GameManager.Instance.ChangeToMainScene(); });
    }

    public ForbiddenType RuleChecker(Constants.PlayerType playerType, int inRow, int inCol)
    {
        //3���� ��� return ForbiddenType.Three;
        //4���� ��� return ForbiddenType.Four;
        //����� ��� return ForbiddenType.Long;


        return ForbiddenType.None;
    }
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
//        Debug.Log(playerType+"����");

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
//        //�� Ȯ��
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