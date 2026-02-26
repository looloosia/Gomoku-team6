using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ReplaySaveData
{
    public List<ReplayFrameData> listRecordFrameData;
    public string recordName;
    public string player1Name;
    public string player2Name;
    public Constants.GameResult gameType;
    public ReplaySaveData(List<ReplayFrameData> listRecordFrameData, string recordName, string player1Name,
                            string player2Name, Constants.GameResult gameType)
    {
        this.listRecordFrameData = listRecordFrameData;
        this.recordName = recordName;
        this.player1Name = player1Name;
        this.player2Name = player2Name;
        this.gameType = gameType;
    }
}
