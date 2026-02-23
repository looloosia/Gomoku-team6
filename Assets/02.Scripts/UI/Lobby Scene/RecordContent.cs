using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RecordContent : MonoBehaviour
{
    [SerializeField]
    private Image imageDraw;
    [SerializeField]
    private Image imageWin;
    [SerializeField]
    private Image imageLose;

    [SerializeField]
    private TMP_Text txtRecordName;

    //왼쪽 플레이어
    [SerializeField]
    private TMP_Text txtRecordPlayer1;
    [SerializeField]
    private Image imageResult1;

    //오른쪽 플레이어
    [SerializeField]
    private TMP_Text txtRecordPlayer2;
    [SerializeField]
    private Image imageResult2;

    [SerializeField]
    private Button btnRecordPlay;

    private List<ReplayFrameData> frameData;

    public void Init(UnityAction action)
    {
        this.btnRecordPlay.onClick.AddListener(action);
    }

    public void Setting(string replayName, string player1Name, string player2Name, Constants.GameResult gameType)
    {
        this.txtRecordName.text = replayName;
        this.txtRecordPlayer1.text = player1Name;
        this.txtRecordPlayer2.text = player2Name;
        ResultImageSetting(gameType);
    }
    private void ResultImageSetting(Constants.GameResult gameType)
    {
        switch(gameType)
        {
            case Constants.GameResult.None:
                this.imageResult1 = this.imageDraw;
                this.imageResult2 = this.imageDraw;
                break;
            case Constants.GameResult.Win:
                this.imageResult1 = this.imageWin;
                this.imageResult2 = this.imageLose;
                break;
            case Constants.GameResult.Lose:
                this.imageResult1 = this.imageLose;
                this.imageResult2 = this.imageWin;
                break;
        }
    }
    public List<ReplayFrameData> GetFrameData()
    {
        return this.frameData;
    }
    public void SetFrameData(List<ReplayFrameData> frameData)
    {
        this.frameData = frameData;
    }
}
