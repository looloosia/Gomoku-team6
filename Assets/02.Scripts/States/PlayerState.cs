using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : BaseState
{
    private GomokuGameLogic _gameLogic;
    // 턴 변경
    public PlayerState(Constants.PlayerType playerType) : base(playerType, Constants.ControllerType.Human)
    {
    }

    public override void HandleNextTurn(GomokuGameLogic gameLogic)
    {
        gameLogic.ChangeGameState();
    }

    public override void OnEnter(GomokuGameLogic gameLogic)
    {
        _gameLogic = GameManager.Instance.GameLogic;
        _board = GameManager.Instance.Board;
        _board.onPlaceStone += OnStonePlace;
        
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

    
    void OnStonePlace(/*, Block block */)
    {
        // 블록이 놓아질 때 처리할 로직
        // TODO: 주석해제
        // Vector2Int pos = block.GetBlockData().boardPos;
        
        // temp
        int tempRow = 1;
        int tempCol = 1;
        
        // 타이머 멈추기
        GameManager.Instance.TurnStateManager.StopCounterRoutine();
        
        HandleMove(_gameLogic, tempRow, tempCol/*pos.y, pos.x*/);
    }
    
    public override void HandleMove(GomokuGameLogic gameLogic, int inRow, int inCol)
    {
        ProcessMove(gameLogic, inRow, inCol);
    }

    public override void OnExit(GomokuGameLogic gameLogic)
    { 
        _board.onPlaceStone -= OnStonePlace;
    }
    
    
}