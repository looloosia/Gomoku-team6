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

    // 게임 시작 시 호출
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

    // 돌 놓을 때마다 호출
    public void AddMoveRecord(ReplayFrameData frameData)
    {
        if (currentRecord.listRecordFrameData != null)
            currentRecord.listRecordFrameData.Add(frameData);
    }

    // 게임 종료 시 호출
    public ReplaySaveData GetFinalRecord(GameResult result, PlayerType winStoneType, GameResultType resultType)
    {
        currentRecord.result = result;
        currentRecord.winStoneType = winStoneType;
        currentRecord.resultType = resultType;
        currentRecord.totalStone = currentRecord.listRecordFrameData.Count;
        return currentRecord;
    }

    // // 현재까지 놓인 돌의 개수(Count) 반환
    // public int GetCurrentStoneCount()
    // {
    //     if (currentRecord.listRecordFrameData != null)
    //     {
    //         return currentRecord.listRecordFrameData.Count;
    //     }
    //     return 0;
    // }

    // 현재까지 기록된 기보 리스트 싹 비우기 (초기화 용도)
    public void ClearMoveRecord()
    {
        // 안전장치: 리스트가 존재하고, 데이터가 2개 이상일 때만 작동
        // (안 그러면 RemoveRange 돌릴 때 에러 터짐)
        if (currentRecord.listRecordFrameData != null && currentRecord.listRecordFrameData.Count > 1)
        {
            // 1번 인덱스부터, (전체 개수 - 1)개 만큼 삭제
            int countToRemove = currentRecord.listRecordFrameData.Count - 1;
            currentRecord.listRecordFrameData.RemoveRange(1, countToRemove);
        }
    }
}
