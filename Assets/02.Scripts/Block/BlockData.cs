using UnityEngine;
using static Constants;

[System.Serializable]
public struct BlockData
{
    public PlayerType markerType;
    public int row;
    public int col;
    public BlockData(PlayerType markerType, int row, int col)
    {
        this.markerType = markerType;
        this.row = row;
        this.col = col;
    }
}
