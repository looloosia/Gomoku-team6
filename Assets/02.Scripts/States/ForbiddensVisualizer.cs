using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 금수를 X로 표시하는 기능
/// </summary>
public class ForbiddensVisualizer : MonoBehaviour
{
    // temp
    private static Queue forbiddenPositions = new Queue();
    // public Dictionary<(int, int), Block> DicBlocks = new Dictionary<(int, int), Block>();
    private Dictionary<(int, int), Block> _dicBlocks = new Dictionary<(int, int), Block>();
    // ---
    
    private Sprite _forbiddenSprite;
    
    GomokuGameLogic _gameLogic;
    Constants.PlayerType _playerType;
    Dictionary<(int, int), SpriteRenderer> _spriteRenderers = new Dictionary<(int, int), SpriteRenderer>();
    
    public void Init(Sprite forbiddenSprite)
    {
        _gameLogic = GameManager.Instance.GameLogic;
        _dicBlocks = GameManager.Instance.Board.DicBlocks;
        _spriteRenderers = GetSpriteRenderers();
        _forbiddenSprite = forbiddenSprite;
    }

    private Dictionary<(int, int), SpriteRenderer> GetSpriteRenderers()
    {
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

    public void VisualizeForbiddens()
    {
        foreach (var dicBlock in _dicBlocks)
        {
            int row = dicBlock.Key.Item2;
            int col = dicBlock.Key.Item1;
            Block block = dicBlock.Value;
            
            if (GomokuLibrary.IsForbidden(_gameLogic.Board, _playerType, row, 
                    col, _dicBlocks.Count) != Constants.ForbiddenType.None)
            {
                _spriteRenderers[(row, col)].sprite = _forbiddenSprite;
            }
        }
    }
}