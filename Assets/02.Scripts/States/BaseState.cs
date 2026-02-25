using UnityEngine;

public abstract class BaseState
{
    protected Constants.PlayerType _currentPlayerType;
    protected Constants.PlayerType _gamePlayerType;
    public Constants.PlayerType Type => _currentPlayerType;
    protected Constants.ControllerType _controllerType;
    public  Constants.ControllerType ControllerType => _controllerType;

    protected Board _board;
    protected ForbiddensVisualizer _forbiddensVisualizer;

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
    
}