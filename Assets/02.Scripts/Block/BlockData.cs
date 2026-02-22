using UnityEngine;
using static Constants;

[System.Serializable]
public class BlockData
{
    public PlayerType markerType = PlayerType.None;
    public Vector2Int boardPos;
    public BlockData(PlayerType markerType, Vector2Int boardPos)
    {
        this.markerType = markerType;
        this.boardPos = boardPos;
    }
}
