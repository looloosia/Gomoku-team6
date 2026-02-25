using System;
using UnityEngine;
using UnityEngine.UI;

public class MarkerSelectPopup : BasePopup
{
    [SerializeField] private Button blackStoneBtn;
    [SerializeField] private Button whiteStoneBtn;

    // 돌 선택시 해당 선택한 돌이 플레이어 정보에 반영
    // 돌 선택 후 해당 팝업은 꺼지며 게임 캔버스가 떠야함
}
