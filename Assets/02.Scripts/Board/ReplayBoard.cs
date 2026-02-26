using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.Events;

public class ReplayBoard : MonoBehaviour
{
    [SerializeField]
    private BoardGenerator boardGenerator;

    [SerializeField]
    private Button btnPrev;
    [SerializeField]
    private Button btnNext;

    //key: 블럭 위치
    private Dictionary<(int, int), Block> dicBlocks = new Dictionary<(int, int), Block>();

    private List<ReplayFrameData> listRecordFrame = new List<ReplayFrameData>();

    private int currentReplayframe = 0;

    public UnityAction<List<ReplayFrameData>> onLoadReplayData;

    public event UnityAction OnPrevMoveEvent;
    public event UnityAction OnNextMoveEvent;
    public event UnityAction OnFirstMoveEvent;
    public event UnityAction OnLastMoveEvent;

    void Awake()
    {
        this.dicBlocks = this.boardGenerator.GenerateBoard();

        this.onLoadReplayData = LoadReplayData;

        EventsInit();
    }
    private void LoadReplayData(List<ReplayFrameData> frameData)
    {
        this.listRecordFrame = frameData;
    }
    private void SetBoardFromReplayData(ReplayFrameData frameData)
    {
        foreach(BlockData blockData in frameData.blockDatas)
        {
            (int r, int c) key = (blockData.row, blockData.row);

            this.dicBlocks[key].SetBlockData(blockData);
        }
    }
    private void NextFrame()
    {
        if (this.currentReplayframe >= this.listRecordFrame.Count - 1)
            return;

        this.currentReplayframe++;
        ReplayFrameData frameToPlay = this.listRecordFrame[this.currentReplayframe];
        SetBoardFromReplayData(frameToPlay);
    }
    private void PrevFrame()
    {
        if (this.currentReplayframe <= 0)
            return;

        this.currentReplayframe--;
        ReplayFrameData frameToPlay = this.listRecordFrame[this.currentReplayframe];
        SetBoardFromReplayData(frameToPlay);
    }
    private void FirstFrame()
    {
        if (this.listRecordFrame == null || this.listRecordFrame.Count == 0)
            return;

        this.currentReplayframe = 0; // 인덱스를 0으로 초기화
        ReplayFrameData frameToPlay = this.listRecordFrame[this.currentReplayframe];
        SetBoardFromReplayData(frameToPlay);
    }
    private void LastFrame()
    {
        if (this.listRecordFrame == null || this.listRecordFrame.Count == 0)
            return;

        this.currentReplayframe = this.listRecordFrame.Count - 1; // 인덱스를 마지막으로 설정
        ReplayFrameData frameToPlay = this.listRecordFrame[this.currentReplayframe];
        SetBoardFromReplayData(frameToPlay);
    }
    private void EventsInit()
    {
        this.OnFirstMoveEvent += FirstFrame;
        this.OnLastMoveEvent += LastFrame;
        this.OnPrevMoveEvent += PrevFrame;
        this.OnNextMoveEvent += NextFrame;
    }
}
