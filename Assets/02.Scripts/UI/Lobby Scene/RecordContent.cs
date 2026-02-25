using System;
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

    private ReplaySaveData replaySaveData;

    public void Init(Action<ReplaySaveData> action)
    {
        this.btnRecordPlay.onClick.AddListener(() =>
        {
            action?.Invoke(this.replaySaveData);
        });
        Setting();
    }

    public void Setting()
    {
        this.txtRecordName.text = this.replaySaveData.recordName;
        this.txtRecordPlayer1.text = this.replaySaveData.player1Name;
        this.txtRecordPlayer2.text = this.replaySaveData.player2Name;
        ResultImageSetting(this.replaySaveData.gameType);
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
        return this.replaySaveData.listRecordFrameData;
    }
    public void SetFrameData(ReplaySaveData replaySaveData)
    {
        this.replaySaveData = replaySaveData;
    }
}
