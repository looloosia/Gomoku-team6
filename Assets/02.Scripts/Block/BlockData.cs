using UnityEngine;
using static Constants;

[System.Serializable]
public struct BlockData
{
    public PlayerType markerType;
    public int col;
    public int row;
    public BlockData(PlayerType markerType, int col, int row)
    {
        this.markerType = markerType;
        this.col = row;
        this.row = col;
    }
}
