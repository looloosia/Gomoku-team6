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
        // dateTxt.textStyle = data.replayName
        // modeTxt
        // opponentTxt
        // resultTxt
        // myMarkerImg
        // totalMovesTxt
        // reviewBtn
    }
}
