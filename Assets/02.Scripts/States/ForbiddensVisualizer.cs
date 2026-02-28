using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 금수를 X로 표시하는 기능
/// </summary>
public class ForbiddensVisualizer : MonoBehaviour
{
    private Dictionary<(int, int), Block> _dicBlocks = new Dictionary<(int, int), Block>();
    private Constants.PlayerType[,] _virtualBoard;
    
    GomokuGameLogic _gameLogic;
    
    // 이 초기화 함수는 Board.cs의 Awake() 이후에 호출되어야 함.
    public void Init(Sprite forbiddenSprite)
    {
        _virtualBoard = GameManager.Instance.GameLogic.VirtualBoard;
        if (_virtualBoard == null)
        {
            Debug.LogError("ForbiddenVisualizer: _board is null");
        }
        
        _dicBlocks = GameManager.Instance.Board.DicBlocks;
    }

    // 금수 표시 기능
    public void VisualizeForbiddens(Constants.PlayerType currPlayerType, GomokuGameLogic gameLogic)
    {
        _virtualBoard = gameLogic.VirtualBoard;
        if (_virtualBoard == null)
        {
            Debug.LogError("ForbiddenVisualizer: _virtualBoard is null");
            return;
        }
        if (_dicBlocks == null || _dicBlocks.Count == 0)
        {
            if (GameManager.Instance.Board != null)
            {
                _dicBlocks = GameManager.Instance.Board.DicBlocks;
            }
            if (_dicBlocks == null)
            {
                Debug.LogError("ForbiddenVisualizer: _dicBlocks is null");
                return;
            }
        }
        foreach (var dicBlock in _dicBlocks)
        {
            int row = dicBlock.Key.Item1;
            int col = dicBlock.Key.Item2;

            if (_virtualBoard[row, col] != Constants.PlayerType.None)
            {
                continue;
            }

            if (GomokuLibrary.IsForbidden(gameLogic.VirtualBoard, currPlayerType, row,
                    col, Constants.BOARD_SIZE) != Constants.ForbiddenType.None)
            {
                dicBlock.Value.SetForbiddenStone();
            }
        }
    }

    // 금수 표시 초기화 기능
    public void ClearForbiddens()
    {
        foreach (var pair in _dicBlocks.Values)
        {
            pair.ResetStone();
        }
    }
}