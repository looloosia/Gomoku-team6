using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ReplaySaveData
{
    public List<ReplayFrameData> listReplayFrameData;
    public ReplaySaveData(List<ReplayFrameData> listReplayFrameData)
    {
        this.listReplayFrameData = listReplayFrameData;
    }
}
