using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// forbiddensVisualizer 임시 주석처리
/// </summary>
public class PlayerState : BaseState
{
    // 턴 변경
    public PlayerState(Constants.PlayerType playerType) : base(playerType, Constants.ControllerType.Human)
    {
    }
    
    // 한 턴이 시작될 때
    public override void OnEnter(GomokuGameLogic gameLogic)
    {
        // Debug.Log("<color=red>OnEnter</color>");
        _gameLogic = gameLogic;
        _board = GameManager.Instance.Board;

        _gameLogic.onBlockClicked = OnStonePlace;
        
        _gamePlayerType = GameManager.Instance.GamePlayerType;
        // TODO: Turn UI 업데이트?
        // UIManager.Instance.SetGameTurn(_currentPlayerType);
    }

    public override void HandleNextTurn(GomokuGameLogic gameLogic)
    {
        gameLogic.ChangeGameState();
    }
    
    public override void HandleMove(GomokuGameLogic gameLogic, int inRow, int inCol)
    {
        ProcessMove(gameLogic, inRow, inCol);
    }

    // 한 턴이 끝날 때
    public override void OnExit(GomokuGameLogic gameLogic)
    { 
        
    }
}