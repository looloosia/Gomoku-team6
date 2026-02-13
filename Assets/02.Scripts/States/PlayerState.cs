using UnityEngine;

/// <summary>
/// 플레이어 턴 관리
/// 현재 Constants(PlayerType), BlockController, GameLogic, GameManager(SetGameTurn()) 필요
/// </summary>
public class PlayerState : BaseState
{
    private TurnManager turnManager;
    
    public PlayerState(bool isFirstPlayer)
    {
        // _playerType = isFirstPlayer ? Constants.PlayerType.Player1 : Constants.PlayerType.Player2;
    }
    
    // 턴 변경
    public override void HandleNextTurn(GameLogic gameLogic)
    {
        gameLogic.ChangeGameState();
    }
    
    public override void OnEnter(GameLogic gameLogic)
    {
        // 상태 진입 시 로직 구현
        gameLogic.blockController.onBlockClicked = (blockIndex) =>
        {
            // 블록이 클릭되었을 때 처리할 로직
            HandleMove(gameLogic, blockIndex);
        };
    
        // 턴 표시 UI 업데이트
        GameManager.Instance.SetGameTurn(_playerType);
    }
    
    public override void HandleMove(GameLogic gameLogic, int index)
    {
        ProcessMove(gameLogic, index, _playerType);
    }
    
    public override void OnExit(GameLogic gameLogic)
    {
    }
}
