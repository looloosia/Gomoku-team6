using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RecordContent : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text dateTxt;       // "2026.02.26\n00:00"
    [SerializeField] private TMP_Text modeTxt;       // "AI 전"
    [SerializeField] private TMP_Text opponentTxt;   // "vs AI (18급)"
    [SerializeField] private TMP_Text resultTxt;     // "승 (흑 5목)"
    [SerializeField] private Image myMarkerImg;       // 내 돌 색상 아이콘
    [SerializeField] private TMP_Text totalMovesTxt; // "120수"
    [SerializeField] private Button reviewBtn;       // "복기" 버튼
    [SerializeField] private Sprite blackMarkerSprite;
    [SerializeField] private Sprite whiteMarkerSprite;

    public void Init(ReplaySaveData data, Action<ReplaySaveData> onClickReview)
    {
        // 날짜 및 시간
        dateTxt.text = $"{data.date}\n{data.time}";

        // 게임 모드 (GameType Enum 활용)
        modeTxt.text = (data.gameType == Constants.GameType.SinglePlay) ? "AI 전" : "멀티 전";

        // 상대 정보
        opponentTxt.text = $"vs {data.nickName} ({data.rank}급)";

        // 결과 텍스트 (예: "승 (흑 기권)")
        string resultStr = (data.result == Constants.GameResult.Win) ? "승" :
                           (data.result == Constants.GameResult.Lose) ? "패" : "무승부";
        string winStoneStr = (data.winStoneType == Constants.PlayerType.Black) ? "흑" : "백";
        
        // (GameResultType은 '5목', '기권' 등의 Enum이라고 가정)
        resultTxt.text = $"{resultStr} ({winStoneStr} {data.resultType})";

        // 내 돌 색상 이미지
        myMarkerImg.sprite = (data.myStoneType == Constants.PlayerType.Black) ? blackMarkerSprite : whiteMarkerSprite;

        // 총 턴(수)
        totalMovesTxt.text = $"{data.totalStone}수";

        // 복기 버튼 이벤트 연결
        reviewBtn.onClick.RemoveAllListeners();
        reviewBtn.onClick.AddListener(() => onClickReview?.Invoke(data));
    }
}
