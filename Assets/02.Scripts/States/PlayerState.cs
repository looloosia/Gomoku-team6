using UnityEngine;

public class PlayerState : BaseState
{
    public PlayerState(Constants.PlayerType playerType)
    {
        _playerType = playerType;
    }

    // 턴 변경
    public override void HandleNextTurn( /*GomokuGameLogic gameLogic */)
    {
        // gameLogic.ChangeGameState();
    }

    public override void OnEnter( /*GomokuGameLogic gameLogic */ )
    {
        Debug.Log("OnEnter");
        _board = GameManager.Instance.Board;
        
        // TODO: 카운터사용
        // demoCounter 기반 임시 테스트용
        //gameLogic.demoCounter--;
        // if (gameLogic.demoCounter == 0)
        // {
        //     gameLogic.EndGame(this, Constants.GameResult.Win);
        //     return;
        // }
        Debug.Log($"{this} turn Enter");
        
        // 상태 진입 시 로직 구현
        _board.onPlaceStone = () =>
        {
            // 블록이 클릭되었을 때 처리할 로직
            // HandleMove(gameLogic, inRow, inCol);
            Debug.Log("OnPlaceStone");
        };

        // Turn UI 업데이트
        GameManager.Instance.SetGameTurn(_playerType);
    }

    public override void HandleMove( /*GomokuGameLogic gameLogic, */ int inRow, int inCol)
    {
        ProcessMove(/*gameLogic, */ inRow, inCol);
    }

    public override void OnExit( /*GomokuGameLogic gameLogic */ )
    {
    }
}