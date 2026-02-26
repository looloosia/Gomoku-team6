using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecordListPopup : BasePopup
{
    [SerializeField] private Transform contents;
    [SerializeField] private GameObject recordPrefab;

    private List<RecordContent> listRecordContent = new List<RecordContent>();

    public override void Show()
    {
        base.Show();
        LoadAllReplayJson();
    }

    public override void Hide(PopupHideDelegate onComplete = null)
    {
        onComplete?.Invoke();
        gameObject.SetActive(false);
    }

    private void LoadAllReplayJson()
    {
        //기존에 생성된 UI 리스트 제거 (중복 생성 방지)
        foreach (Transform child in contents)
            Destroy(child.gameObject);

        // 2. AccountManager에서 내 데이터 가져오기
        UserData me = AccountManager.Instance.CurrentUser;
        if (me == null || me.replayList.Count == 0)
            return;

        // 최신 데이터가 위로 오게 정렬 (리스트 뒤집기)
        // 만약 날짜순 정렬이 필요하면 me.replayList.OrderByDescending(...)사용
        List<ReplaySaveData> sortedList = me.replayList.AsEnumerable().Reverse().ToList();

        // 프리팹 생성 및 데이터 주입
        foreach (ReplaySaveData data in sortedList)
        {
            GameObject obj = Instantiate(recordPrefab, contents);
            RecordContent content = obj.GetComponent<RecordContent>();

            // 데이터와 씬 전환 함수를 함께 넘겨줌.
            content.Init(data, RecordScene);
        }
    }

    // 복기 버튼 눌렀을 때 실행될 함수
    private void RecordScene(ReplaySaveData data)
    {
        MySceneManager.Instance.LoadSceneWithCallback<ReplayBoard>("Record", (replayBoard) =>
        {
            // replayBoard.onLoadReplayData(data.listReplayFrameData);
            RecordPanelController reocordController = FindAnyObjectByType<RecordPanelController>();

            if (reocordController != null)
        {
            // 내 데이터 가져오기
            UserData me = AccountManager.Instance.CurrentUser;
            
            // 데이터에 기록된 돌 색상 확인 (예: 내가 흑돌이었는지)
            // (ReplaySaveData에 돌 색상 정보가 없다면 임시로 true/false 세팅)
            // bool isMyStoneBlack = true; 

            // 우리가 만든 UI 함수에 데이터 꽂아주기!
            // reocordController.SetupProfiles(
            //     p1Name: data.player1Name, // 상대방 이름
            //     p1Rank: "18급",            // (임시) 상대방 급수
            //     p1IsBlack: !isMyStoneBlack, 
            //     p2Name: me.nickname,      // 내 이름
            //     p2Rank: $"{me.rank}급",   // 내 급수
            //     p2IsBlack: isMyStoneBlack
            // );
        }
        });
    }
}
