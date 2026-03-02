using System;
using UnityEngine;
using static Constants;
using static GomokuLibrary;

public class GomokuGameLogic
{
    private PlayerType[,] virtualBoard;
    public PlayerType[,] VirtualBoard { get { return virtualBoard; } }

    public Action<int, int> onBlockClicked;//������Ʈ�� ���� �׼�
    bool isStart = false;

    public BaseState playerAState;
    public BaseState playerBState;

    private Board gomokuBoard;

    private BaseState currentState;

    private TurnStateManager turnStateManager;


    private Coroutine counterRoutine = null;

    //�¸�, �й� ���� �� �ӽ� ī���� ����(��������)
    public int demoCounter = 5;


    //�浹���� �鵹���� ����(��: ��, ��: ��)
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
            Debug.LogError("��ȿ�� ���尡 �����ϴ�");
        }

        PlayerType otherPlayerType = playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black; //���� Ÿ��(��Ƽ�÷����� ��� �Խ�Ʈ)
        SoundManager.Instance.PlaySFX(SFX.JingE);
        //TODO: stoneonclick���� �� �׼��� ���� �Լ� ����, (state�� ������ �Լ�)

        switch (gameType)
        {
            case GameType.LocalDualPlay:
                playerAState = new PlayerState(playerType);
                playerBState = new PlayerState(otherPlayerType);
                ////�浹�� Player���� ����
                StartFirstState();
                break;

            case GameType.SinglePlay:
                playerAState = new PlayerState(playerType);
                playerBState = new _02.Scripts.States.AIState(otherPlayerType);

                //ù ���� Player���� ����
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

    //�� Ȥ�� ���°� �ٲ� �� ȣ��Ǵ� �޼���
    public void SetState(BaseState newState)
    {

        currentState?.OnExit(this); //���� ������Ʈ ��
        currentState = newState;
        currentState?.OnEnter(this);    //���ο� ������Ʈ ����

        Debug.Log($"{currentState.Type}: CURRENTONENTER");
        gomokuBoard.SetCurrentStone(currentState.Type);

        //�ݼ� �ڸ� ���� �� ���� üũ
        ClearForbiddenPositionCheck(virtualBoard);
        if (currentState.Type == PlayerType.Black)
            CheckForbiddenPostions(virtualBoard, currentState.Type, BOARD_SIZE);

        gomokuBoard.UpdateBlock(virtualBoard);
        isStart = true;

        turnStateManager.SetState(newState);
    }

    public bool PlaceMarker(PlayerType playerType, int inRow, int inCol)
    {
        if (virtualBoard[inRow, inCol] != Constants.PlayerType.None) //�����ΰ� �ִ� ���
        {
            if (virtualBoard[inRow, inCol] == Constants.PlayerType.Forbidden)
            {
                Debug.Log("�ݼ� �ڸ�");
            }
            else
                Debug.Log("�� ĭ�� ���� �����ּ���");
            return false;
        }
        //if (RuleChecker(playerType, inRow, inCol) != ForbiddenType.None)
        //    return false;

        virtualBoard[inRow, inCol] = playerType;
        return true;
    }

    //�� ����
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
        //�¸� ���� Ȯ�� ���� ����
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

        if (gameResult == Constants.GameResult.Win) //�¸�
        {
            colorOfWinner = playerState.Type == PlayerType.Black ? "��" : "��"; //�¸��� ��
            title = colorOfWinner + " �¸�!";

            ConfirmPopup popup = UIManager.Instance.OpenConfirmPopup();
            subText = title + "������ �ϼ��Ͽ� �¸��Ͽ����ϴ�.";
            popup.Show(title, "������ �ϼ��Ͽ� �¸��Ͽ����ϴ�.", "", null, "Ȯ��", () =>
            {
                GameManager.Instance.ChangeToLobbyScene();
            });
        }

        else if (gameResult == Constants.GameResult.Lose) //�й�(�ð� �ʰ� ��)
        {
            colorOfWinner = playerState.Type == PlayerType.Black ? "��" : "��"; //�й����� ���� �ݴ� ��
            title = colorOfWinner + " �¸�!";

            ConfirmPopup popup = UIManager.Instance.OpenConfirmPopup();
            subText = title + "�ð� �ʰ��� �¸��Ͽ����ϴ�.";
            popup.Show(title, "�ð� �ʰ��� �¸��Ͽ����ϴ�.", "", null, "Ȯ��", () =>
            {
                GameManager.Instance.ChangeToLobbyScene();
            });

        }

        // �⺸ ���� ���� �� ������ �Ѱ��ֱ� ���� �ڵ�
        ReplaySaveData finalRecord = ReplayManager.Instance.GetFinalRecord(
        gameResult,
        playerState.Type,
        Constants.GameResultType.ConnectFive // (�ӽ� �¸�����)
        );

        // ���� ���� ��� �˾�â���� ������ �ѱ�� ���� �ڵ�
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
        //Board�� Action ����.

        //GameManager.Instance.OpenConfirmPanel(resultString, () => { GameManager.Instance.ChangeToMainScene(); });
    }

    //public ForbiddenType RuleChecker(Constants.PlayerType playerType, int inRow, int inCol)
    //{
    //    //3���� ��� return ForbiddenType.Three;
    //    //4���� ��� return ForbiddenType.Four;
    //    //����� ��� return ForbiddenType.Long;


    //    return ForbiddenType.None;
    //}
}
