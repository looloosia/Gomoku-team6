using UnityEngine;

[System.Serializable]
public struct ReplayFrameData
{
    public BlockData[] blockDatas;
    public ReplayFrameData(BlockData[] blockDatas)
    {
        this.blockDatas = blockDatas;
    }
}
