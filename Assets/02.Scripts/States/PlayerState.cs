using System.Collections.Generic;
using UnityEngine;

public class PlayerState : BaseState
{
    // 턴 변경
    public PlayerState(Constants.PlayerType playerType) : base(playerType)
    {
    }

    public override void HandleNextTurn(GomokuGameLogic gameLogic)
    {
        gameLogic.ChangeGameState();
    }

    public override void OnEnter(GomokuGameLogic gameLogic)
    {
        Debug.Log("OnEnter");
        _board = GameManager.Instance.Board;
        _board.onPlaceStone += OnBlockClicked;
        
        // demoCounter 기반 임시 테스트용
        gameLogic.demoCounter--;
         if (gameLogic.demoCounter == 0)
         {
             gameLogic.EndGame(this, Constants.GameResult.Win);
             return;
         }
        Debug.Log($"{this} turn Enter");
        
        // 상태 진입 시 로직 구현
        

        // Turn UI 업데이트
        GameManager.Instance.SetGameTurn(_playerType);
    }

    void OnBlockClicked(Block block)
    {
        // TODO: 주석해제) 블록이 클릭되었을 때 처리할 로직
        // Vector2Int pos = block.GetBlockData().boardPos;
        //
        // HandleMove( /*gameLogic, */pos.y, pos.x);
    }
    
    public override void HandleMove(GomokuGameLogic gameLogic, int inRow, int inCol)
    {
        ProcessMove(gameLogic, inRow, inCol);
    }

    public override void OnExit(GomokuGameLogic gameLogic)
    { 
        _board.onPlaceStone -= OnBlockClicked;
    }
}