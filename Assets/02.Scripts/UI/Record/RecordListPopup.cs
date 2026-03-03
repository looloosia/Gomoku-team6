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
        Debug.Log("RecordListPopup의 Show() 함수가 호출되었습니다!");
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
        // if (me == null || me.replayHistory == null || me.replayHistory.Count == 0)
        //     return;

        if (me == null) 
            Debug.LogError("[오류] 로그인된 유저가 없습니다! 로그인부터 다시 해보세요.");
        else if (me.replayHistory == null) 
            Debug.LogError("[오류] 기보 리스트(replayHistory)가 아예 생성되지 않았습니다");
        else 
            Debug.Log($"[정상] 팝업 열림! 현재 저장된 내 기보 개수: {me.replayHistory.Count}개");

        // 최신 데이터가 위로 오게 정렬 (리스트 뒤집기)
        // 만약 날짜순 정렬이 필요하면 me.replayList.OrderByDescending(...)사용
        List<ReplaySaveData> sortedList = me.replayHistory.AsEnumerable().Reverse().ToList();

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
        // 매니저 존재 확인
        if (MySceneManager.Instance == null)
        {
            Debug.LogError("[Null] MySceneManager.Instance가 존재하지 않습니다.");
            return;
        }
        
        MySceneManager.Instance.LoadSceneWithCallback<ReplayBoard>("Record", (replayBoard) =>
        {
            // 2. 타겟 스크립트 확인 (복기 씬에 ReplayBoard가 없는 경우)
            if (replayBoard == null)
            {
                Debug.LogError("[Null] 'Record' 씬에서 ReplayBoard 컴포넌트를 찾을 수 없습니다");
                return;
            }
            
            // 로직 스크립트(ReplayBoard)에 프레임 데이터 전달
            replayBoard.onLoadReplayData?.Invoke(data.listRecordFrameData);

            // UI 세팅
            RecordPanelController recordController = FindAnyObjectByType<RecordPanelController>();

            if (recordController != null)
            {
                UserData me = AccountManager.Instance.CurrentUser;

                // 3. 유저 정보 확인 (로그인이 끊겼거나 정보가 없는 경우)

                if (me == null)
                {
                    Debug.LogError("[Null] CurrentUser가 null입니다. 닉네임을 불러올 수 없습니다.");
                    return;
                }
                
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

                Debug.Log("복기 씬 UI 세팅 완료!");
            }
            else
            {
                Debug.LogWarning("RecordPanelController를 씬에서 찾을 수 없습니다.");
            }
        });
    }
}
