using TMPro;
using UnityEngine;

public class RankContent : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtRanking;
    [SerializeField]
    private TMP_Text txtNickname;
    [SerializeField]
    private TMP_Text txtRank;

    public void Init(string ranking, string nickname, string rank)
    {
        this.txtRanking.text = ranking;
        this.txtNickname.text = nickname;
        this.txtRank.text = rank;
    }
}
