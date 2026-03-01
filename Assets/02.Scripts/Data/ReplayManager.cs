using System;
using UnityEngine;
using static Constants;
using System.Collections.Generic;

public class ReplayManager : MonoBehaviour
{
    public static ReplayManager Instance { get; private set; }
    private ReplaySaveData currentRecord;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 1. 게임 시작 시 호출
    public void StartRecording(GameType gameType, string opponentName, string opponentRank, PlayerType myStoneType)
    {
        currentRecord = new ReplaySaveData();
        currentRecord.listRecordFrameData = new List<ReplayFrameData>(); 

        currentRecord.date = DateTime.Now.ToString("yyyy-MM-dd");
        currentRecord.time = DateTime.Now.ToString("HH:mm");
        currentRecord.gameType = gameType;
        currentRecord.nickName = opponentName; 
        currentRecord.rank = opponentRank;     
        currentRecord.myStoneType = myStoneType; 
    }

    // 2. 돌 놓을 때마다 호출
    public void AddMoveRecord(ReplayFrameData frameData)
    {
        if (currentRecord.listRecordFrameData != null)
            currentRecord.listRecordFrameData.Add(frameData);
    }

    // 3. 게임 종료 시 호출
    public ReplaySaveData GetFinalRecord(GameResult result, PlayerType winStoneType, GameResultType resultType)
    {
        currentRecord.result = result;
        currentRecord.winStoneType = winStoneType;
        currentRecord.resultType = resultType;
        currentRecord.totalStone = currentRecord.listRecordFrameData.Count;
        return currentRecord;
    }
}
