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

    // 한 턴이 시작될 때
    public override void OnEnter(GomokuGameLogic gameLogic)
    {
        Debug.Log("OnEnter() 실행됨");
        _gameLogic = gameLogic;
        _board = GameManager.Instance.Board;
        _forbiddensVisualizer = GameManager.Instance.ForbiddensVisualizer;
        
        // TODO: Board.cs 머지되면 주석해제
        _board.onPlaceStone += OnStonePlace;
        
        _gamePlayerType = GameManager.Instance.GamePlayerType;
        
        // 흑돌일 경우 금수 표시
        if (_currentPlayerType == Constants.PlayerType.Black && _gamePlayerType == Constants.PlayerType.Black)
            _forbiddensVisualizer.VisualizeForbiddens(_currentPlayerType);

        // Turn UI 업데이트
        GameManager.Instance.SetGameTurn(_currentPlayerType);
    }

    // 블록이 놓아질 때 처리할 로직
    void OnStonePlace(Block block)
    {
        Debug.Log("OnStonePlace() 실행됨");
        int row = block.GetBlockData().col;
        int col = block.GetBlockData().col;
        
        // 타이머 멈추기
        GameManager.Instance.TurnStateManager.StopCounterRoutine();
        
        HandleMove(_gameLogic, row, col);
    }
    
    public override void HandleMove(GomokuGameLogic gameLogic, int inRow, int inCol)
    {
        ProcessMove(gameLogic, inRow, inCol);
    }

    // 한 턴이 끝날 때
    public override void OnExit(GomokuGameLogic gameLogic)
    { 
        _forbiddensVisualizer.ClearForbiddens();
        
        // TODO: Board.cs 머지되면 주석해제
        _board.onPlaceStone -= OnStonePlace;
    }
    
    
}