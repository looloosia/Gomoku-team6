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
        Debug.Log("RecordListPopup의 Show() 함수가 호출되었습니다!");
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
        NetworkManager.Instance.Res("/user", (json) =>
        {
            RankResponse response = JsonUtility.FromJson<RankResponse>(json);
            RankData[] rankDatas = response.rankDatas;

            foreach(RankData data in rankDatas)
            {

                GameObject obj = Instantiate(this.objPrefab, this.parent);
                RankContent content = obj.GetComponent<RankContent>();

                content.Init(data.id, data.nickname, data.rank);
            }
        });
    }

}
