using UnityEngine;
using System;

[Serializable]
public class RankData
{
    public string id;
    public string nickname;
    public string rank;
}

[Serializable]
public class RankResponse
{
    public int cmd;    
    public string message;
    public RankData[] rankDatas;
}

