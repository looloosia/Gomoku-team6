using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankListPopup : BasePopup
{
    [SerializeField]
    private GameObject objPrefab;
    [SerializeField]
    private Transform parent;

    public override void Show()
    {
        Debug.Log("RankListPopup의 Show() 함수가 호출되었습니다!");
        base.Show();
        LoadAllRankJson();
    }

    public override void Hide(PopupHideDelegate onComplete = null)
    {
        onComplete?.Invoke();
        gameObject.SetActive(false);
    }

    private void LoadAllRankJson()
    {
        foreach (Transform child in this.parent)
        {
            Destroy(child.gameObject);
        }

        NetworkManager.Instance.Res("rank", (json) =>
        {
            RankResponse response = JsonUtility.FromJson<RankResponse>(json);
            RankData[] rankDatas = response.rankDatas;
            int num = 1;
            foreach (RankData data in rankDatas)
            {
                string rankNum = "";
                if (num <= 3)
                    rankNum = num.ToString();
                else
                    rankNum = "";

                GameObject obj = Instantiate(this.objPrefab, this.parent);
                RankContent content = obj.GetComponent<RankContent>();

                content.Init(rankNum, data.nickname, data.rank);

                num++;
            }
        });
    }
}
