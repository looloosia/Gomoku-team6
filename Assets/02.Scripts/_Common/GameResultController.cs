using UnityEngine;

public class GameResultController : MonoBehaviour
{
    // 1. 승패 팝업
    public void StartResultFlow(string title, string subText, ReplaySaveData finalRecord)
    {
        ConfirmPopup popup = UIManager.Instance.OpenConfirmPopup();
        popup.Show(title, subText, "", null, "확인", () => AskSaveRecord(finalRecord));
    }

    // 2. 저장 확인 팝업
    private void AskSaveRecord(ReplaySaveData recordToSave)
    {
        ConfirmPopup popup = UIManager.Instance.OpenConfirmPopup();
        popup.Show("기보 저장", "대국 결과를 저장하시겠습니까?", 
            "취소", () => { GameManager.Instance.ChangeToLobbyScene(); }, 
            "저장", () => OnClickSaveRecord(recordToSave)
        );
    }

    // 3. 실제 저장 처리
    private void OnClickSaveRecord(ReplaySaveData recordToSave)
    {
        AccountManager.Instance.AddReplayDataAndSave(recordToSave);

        ConfirmPopup popup = UIManager.Instance.OpenConfirmPopup();
        popup.Show("", "대국 결과가 저장되었습니다.", "", null, "확인", () => { GameManager.Instance.ChangeToLobbyScene(); });
    }
}
