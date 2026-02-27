using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    public string id;
    public string pwHash;  // 비밀번호 원본이 아닌 암호화된 해시값 저장
    public string nickname;
    public int profileId; // 제공되는 프로필 이미지의 번호 (0,1,2 ...)

    public int coin;

    public int rank;      // 급수
    public int rankPoint; // 승급 포인트

    // 내 기보 리스트
    public List<ReplaySaveData> replayHistory = new List<ReplaySaveData>();

    public UserData() 
    {
        replayHistory = new List<ReplaySaveData>();
    }

    public UserData(string id, string pwHash, string nickname)
    {
        this.id = id;
        this.pwHash = pwHash;
        this.nickname = nickname;
        this.profileId = 0; // 기본 이미지 인덱스
        this.coin = 100;
        this.rank = 18;     // 18급부터 시작
        this.rankPoint = 0;
        this.replayHistory = new List<ReplaySaveData>();
    }
}
