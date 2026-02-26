using System.Collections.Generic;
using UnityEngine;
using static Constants;

[System.Serializable]
public struct ReplaySaveData
{
    [Header("기본 리플레이 정보")]
    public List<ReplayFrameData> listRecordFrameData;
    public string recordName;

    [Header("대국 상세 정보")]
    public string date;                 // 대국 날짜
    public string time;                 // 대국 시간
    public GameType gameType;           // 게임 종류

    [Header("상대방 정보")]
    public string nickName;             // 상대방 닉네임
    public int rank;                    // 상대방 급수

    [Header("결과 및 통계")]
    public GameResult result;           // 승패 판정
    public PlayerType winStoneType;     // 승리한 진영의 돌 종류
    public GameResultType resultType;   // 결과 종류
    public PlayerType myStoneType;      // 내가 사용한 돌의 종류
    public int totalStone;              // 총 놓인 돌의 갯수
}
