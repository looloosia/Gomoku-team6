using UnityEngine;

[System.Serializable]
public class ReplayFrameData
{
    public BlockData[] blockDatas;
    public ReplayFrameData(BlockData[] blockDatas)
    {
        this.blockDatas = blockDatas;
    }
}
