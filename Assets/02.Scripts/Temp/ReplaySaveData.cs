using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReplaySaveData
{
    public List<ReplayFrameData> listReplayFrameData;
    public ReplaySaveData(List<ReplayFrameData> listReplayFrameData)
    {
        this.listReplayFrameData = listReplayFrameData;
    }
}
