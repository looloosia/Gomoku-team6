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
        if (me == null || me.replayList == null || me.replayList.Count == 0)
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
            // 1. 로직 스크립트(ReplayBoard)에 프레임 데이터 전달
            replayBoard.onLoadReplayData?.Invoke(data.listRecordFrameData);

            // 2. UI 세팅
            RecordPanelController recordController = FindAnyObjectByType<RecordPanelController>();

            if (recordController != null)
            {
                UserData me = AccountManager.Instance.CurrentUser;
                
                // 데이터에 저장된 '내 돌 색상'이 흑돌인지 판별
                bool isMyStoneBlack = (data.myStoneType == Constants.PlayerType.Black);

                // UI 컨트롤러에 데이터 꽂아주기
                recordController.SetupProfiles(
                    p1Name: data.nickName,               // 상대방 닉네임
                    p1Rank: $"{data.rank}급",            // 상대방 급수
                    p1IsBlack: !isMyStoneBlack,          // 상대방 돌 색상 (내 돌의 반대)
                    p2Name: me.nickname,                 // 내 닉네임
                    p2Rank: $"{me.rank}급",              // 내 급수
                    p2IsBlack: isMyStoneBlack            // 내 돌 색상
                );
            }
        });
    }
}
