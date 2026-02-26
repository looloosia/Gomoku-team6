using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// forbiddensVisualizer 임시 주석처리
/// </summary>
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
        // _forbiddensVisualizer = GameManager.Instance.ForbiddensVisualizer;

        _gameLogic.onBlockClicked = OnStonePlace;
        
        _gamePlayerType = GameManager.Instance.GamePlayerType;
        
        // 흑돌일 경우 금수 표시
        // if (_currentPlayerType == Constants.PlayerType.Black && _gamePlayerType == Constants.PlayerType.Black)
        //     _forbiddensVisualizer.VisualizeForbiddens(_currentPlayerType);

        // Turn UI 업데이트
        GameManager.Instance.SetGameTurn(_currentPlayerType);
    }

    // 블록이 놓아질 때 처리할 로직
    void OnStonePlace(int row, int col)
    {
        Debug.Log("OnStonePlace() 실행됨");
        
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
        // _forbiddensVisualizer.ClearForbiddens();
    }
    
    
}