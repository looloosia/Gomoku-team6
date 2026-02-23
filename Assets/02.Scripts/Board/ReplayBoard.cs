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

    private List<ReplayFrameData> listReplayFrame = new List<ReplayFrameData>();

    private int currentReplayframe = 0;

    public UnityAction<List<ReplayFrameData>> onLoadReplayData;
    void Awake()
    {
        this.dicBlocks = this.boardGenerator.GenerateBoard();
        this.onLoadReplayData = LoadReplayData;

        this.btnPrev.onClick.AddListener(PrevFrame);
        this.btnNext.onClick.AddListener(NextFrame);
    }
    private void LoadReplayData(List<ReplayFrameData> frameData)
    {
        this.listReplayFrame = frameData;
    }
    private void SetBoardFromReplayData(ReplayFrameData frameData)
    {
        foreach(BlockData blockData in frameData.blockDatas)
        {
            Vector2Int pos = blockData.boardPos;
            (int x, int y) key = (pos.x, pos.y);

            this.dicBlocks[key].SetBlockData(blockData);
        }
    }
    private void NextFrame()
    {
        if (this.currentReplayframe >= this.listReplayFrame.Count - 1)
            return;

        this.currentReplayframe++;
        ReplayFrameData frameToPlay = this.listReplayFrame[this.currentReplayframe];
        SetBoardFromReplayData(frameToPlay);
    }
    private void PrevFrame()
    {
        if (this.currentReplayframe <= 0)
            return;

        this.currentReplayframe--;
        ReplayFrameData frameToPlay = this.listReplayFrame[this.currentReplayframe];
        SetBoardFromReplayData(frameToPlay);
    }
}
