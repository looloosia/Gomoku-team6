using UnityEngine;

public class GameResultController : MonoBehaviour
{
    //  게임 종료 시 시작점
    public void StartResultFlow(bool isWin, ReplaySaveData finalRecord)
    {
        string title = isWin ? "승리!" : "패배!";
        string subText = isWin ? "축하합니다! 승리하셨습니다." : "아쉽게도 패배하셨습니다.";

        ConfirmPopup popup = UIManager.Instance.OpenConfirmPopup();
        popup.Show(title, subText, "", null, "확인", () => AskSaveRecord(finalRecord));
    }

    // 저장하시겠습니까? (기보 데이터만 들고 옴)
    private void AskSaveRecord(ReplaySaveData recordToSave)
    {
        ConfirmPopup popup = UIManager.Instance.OpenConfirmPopup();
        
        popup.Show("기보 저장", "대국 결과를 저장하시겠습니까?", 
            "취소", () => {GameManager.Instance.ChangeToLobbyScene();} , 
            "저장", () => OnClickSaveRecord(recordToSave)
        );
    }

    // 저장했을 때
    private void OnClickSaveRecord(ReplaySaveData recordToSave)
    {
        AccountManager.Instance.AddReplayDataAndSave(recordToSave);

        ConfirmPopup popup = UIManager.Instance.OpenConfirmPopup();
        popup.Show("대국 결과가 저장되었습니다.", "", "", null, "확인", () => {GameManager.Instance.ChangeToLobbyScene();});
    }
}
