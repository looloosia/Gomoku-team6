using UnityEngine;

public abstract class BaseState
{
    protected Constants.PlayerType _currentPlayerType;
    protected Constants.PlayerType _gamePlayerType;
    public Constants.PlayerType Type => _currentPlayerType;
    protected Constants.ControllerType _controllerType;
    protected GomokuGameLogic _gameLogic;
    public  Constants.ControllerType ControllerType => _controllerType;

    protected Board _board;

    protected BaseState(Constants.PlayerType playerType, Constants.ControllerType controllerType)
    {
        _currentPlayerType = playerType;
        _controllerType = controllerType;
    }

    public abstract void OnEnter(GomokuGameLogic gameLogic);
    public abstract void HandleMove(GomokuGameLogic gameLogic, int inRow, int inCol);
    public abstract void OnExit(GomokuGameLogic gameLogic);
    public abstract void HandleNextTurn(GomokuGameLogic gameLogic);

    public void ProcessMove(GomokuGameLogic gameLogic, int inRow, int inCol)
    {
        if (gameLogic.PlaceMarker(_currentPlayerType, inRow, inCol))
        {
            var gameResult = gameLogic.CheckGameResult(_currentPlayerType, inRow, inCol);
        
            if (gameResult == Constants.GameResult.None)
            {
                HandleNextTurn(gameLogic);
            }
            else
            {
                gameLogic.EndGame(this, gameResult);
            }
        }
    }
    
    // 블록이 놓아질 때 처리할 로직
    protected void OnStonePlace(int row, int col)
    {
        // 타이머 멈추기
        GameManager.Instance.TurnStateManager.StopCounterRoutine();
        
        HandleMove(_gameLogic, row, col);
    }
}