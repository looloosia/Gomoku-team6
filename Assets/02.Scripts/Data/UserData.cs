using System;
using UnityEngine;

[Serializable]
public class UserData
{
    public string id;
    public string pw;
    public int coin;
    public int rank;

    public UserData(string id, string pw)
    {
        this.id = id;
        this.pw = pw;
        this.coin = 0;
        this.rank = 0;
    }
}
