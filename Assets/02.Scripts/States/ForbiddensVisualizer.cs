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
    
    private Sprite _forbiddenSprite;
    private Constants.PlayerType[,] _board;
    
    GomokuGameLogic _gameLogic;
    Dictionary<(int, int), SpriteRenderer> _spriteRenderers = new Dictionary<(int, int), SpriteRenderer>();
    
    // 이 초기화 함수는 Board.cs의 Awake() 이후에 호출되어야 함.
    public void Init(Sprite forbiddenSprite, GomokuGameLogic gameLogic)
    {
        _gameLogic = gameLogic;
        
        if (_gameLogic == null)
        {
            Debug.Log("A: _gameLogic is null");
        }
        _board = _gameLogic.Board;
        

        if (_board == null)
        {
            Debug.Log("B: _board is null");
        }
        
        // TODO: Board.cs의 dicBlocks를 프로퍼티를 통해 가져오기
        // _dicBlocks = GameManager.Instance.Board.DicBlocks;
        
        _spriteRenderers = GetSpriteRenderers();
        _forbiddenSprite = forbiddenSprite;
    }

    private Dictionary<(int, int), SpriteRenderer> GetSpriteRenderers()
    {
        if (_dicBlocks == null || _dicBlocks.Count == 0)
        {
            return new Dictionary<(int, int), SpriteRenderer>();
        }
        
        Dictionary<(int, int), SpriteRenderer> spriteRenderers = new Dictionary<(int, int), SpriteRenderer>();
        foreach (var dicBlock in _dicBlocks)
        {
            int row = dicBlock.Key.Item2;
            int col = dicBlock.Key.Item1;
            Block block = dicBlock.Value;
            spriteRenderers[(row, col)] = block.gameObject.GetComponent<SpriteRenderer>();
        }
        return spriteRenderers;
    }

    // 금수 표시 기능
    public void VisualizeForbiddens(Constants.PlayerType currPlayerType)
    {
        
        if (_spriteRenderers == null || _spriteRenderers.Count == 0)
        {
            Debug.Log("<color=yellow>Cannot visualize forbiddens because _spriteRenderers is empty</color>");
            return;
        }
        
        foreach (var dicBlock in _dicBlocks)
        {
            int row = dicBlock.Key.Item2;
            int col = dicBlock.Key.Item1;
            
            if (_board[row, col] != Constants.PlayerType.None)
            {
                continue;
            }
            if (GomokuLibrary.IsForbidden(_gameLogic.Board, currPlayerType, row,
                    col, Constants.BOARD_SIZE) != Constants.ForbiddenType.None)
            {
                _spriteRenderers[(row, col)].sprite = _forbiddenSprite;
            }
        }
    }
    
    // 금수 표시 초기화 기능
    public void ClearForbiddens()
    {
        if (_spriteRenderers == null)
            return;
        
        foreach (var pair in _spriteRenderers)
        {
            pair.Value.sprite = null;
        }
    }
}