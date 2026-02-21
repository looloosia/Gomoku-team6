using UnityEngine;

public abstract class BaseState
{
    protected Constants.PlayerType _playerType;
    public Constants.PlayerType Type => _playerType;

    protected Board _board;

    public abstract void OnEnter(GomokuGameLogic gameLogic);
    public abstract void HandleMove(GomokuGameLogic gameLogic, int inRow, int inCol);
    public abstract void OnExit(GomokuGameLogic gameLogic);
    public abstract void HandleNextTurn(GomokuGameLogic gameLogic);

    public void ProcessMove(GomokuGameLogic gameLogic, int inRow, int inCol)
    {
        if (gameLogic.PlaceMarker(_playerType, inRow, inCol))
        {
            // board 스크립트의 돌 생성 관련 함수 호출
            // 예. _board.PlaceStone(inRow, inCol, _playerType);
            var gameResult = gameLogic.CheckGameResult(_playerType, inRow, inCol);

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