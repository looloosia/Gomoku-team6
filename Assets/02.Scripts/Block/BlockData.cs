using UnityEngine;
using static Constants;

[System.Serializable]
public struct BlockData
{
    public PlayerType markerType;
    public Vector2Int boardPos;
    public BlockData(PlayerType markerType, Vector2Int boardPos)
    {
        this.markerType = markerType;
        this.boardPos = boardPos;
    }
}
