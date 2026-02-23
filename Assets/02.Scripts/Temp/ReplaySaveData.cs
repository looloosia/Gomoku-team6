using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ReplaySaveData
{
    public List<ReplayFrameData> listReplayFrameData;
    public string replayName;
    public string player1Name;
    public string player2Name;
    public Constants.GameType gameType;
    public ReplaySaveData(List<ReplayFrameData> listReplayFrameData, string replayName, string player1Name,
                            string player2Name, Constants.GameType gameType)
    {
        this.listReplayFrameData = listReplayFrameData;
        this.replayName = replayName;
        this.player1Name = player1Name;
        this.player2Name = player2Name;
        this.gameType = gameType;
    }
}
