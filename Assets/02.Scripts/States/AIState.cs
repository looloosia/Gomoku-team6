using UnityEngine;

/// <summary>
/// AI 턴 관리
/// 현재 Constants(PlayerType), BlockController, GameLogic, GameManager(SetGameTurn()) 필요
/// </summary>
public class AIState : BaseState
{
    private Constants.PlayerType _playerType;
    
    public AIState(PlayerType playerType)
    {
        _playerType = isFirstPlayer ? Constants.PlayerType.Player1 : Constants.PlayerType.Player2;
    }
    
    public override void HandleMove(int index)
    {
        ProcessMove(gameLogic, index, _playerType);
    }
    
    public override void HandleNextTurn(GameLogic gameLogic)
    {
        gameLogic.ChangeGameState();
    }
    
    public override void OnEnter(GameLogic gameLogic)
    {        
        // 턴 UI 업데이트
        GameManager.Instance.SetGameTurn(_playerType);
    
        var board = gameLogic.Board;
        var result = TicTacToeAI.GetBestMove(board);
    
        if (result.HasValue)
        {
            int row = result.Value.row;
            int col = result.Value.col;
            int index = row * Constants.BOARD_SIZE + col;
    
            HandleMove(index);
        } 
    }
    
    public override void OnExit(GameLogic gameLogic)
    {
    }
}
