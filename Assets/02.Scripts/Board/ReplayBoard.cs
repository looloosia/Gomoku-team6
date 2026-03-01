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
    private RecordPanelController recordPanelCR;


    //key: ���� ��ġ(row, col)
    private Dictionary<(int, int), Block> dicBlocks = new Dictionary<(int, int), Block>();

    private List<ReplayFrameData> listRecordFrame = new List<ReplayFrameData>();

    private int currentReplayframe = 0;

    public UnityAction<List<ReplayFrameData>> onLoadReplayData;


    void Awake()
    {
        this.dicBlocks = this.boardGenerator.GenerateBoard();

        this.onLoadReplayData = LoadReplayData;

        EventsInit();

        //Test
        TestReadJson();
    }
    //Test
    private void TestReadJson()
    {
        string fileName = "Replay_26-02-26_22-51-34";
        string folderPath = Application.dataPath + "/Replay";
        string filePath = folderPath + $"/{fileName}.json";

        string json = File.ReadAllText(filePath);
        ReplaySaveData loadData = JsonUtility.FromJson<ReplaySaveData>(json);
        
        LoadReplayData(loadData.listRecordFrameData);
    }
    private void LoadReplayData(List<ReplayFrameData> frameData)
    {
        this.listRecordFrame = frameData;
    }
    private void SetBoardFromReplayData(ReplayFrameData frameData)
    {
        foreach(BlockData blockData in frameData.blockDatas)
        {
            (int r, int c) key = (blockData.row, blockData.col);

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

        this.currentReplayframe = 0; // �ε����� 0���� �ʱ�ȭ
        ReplayFrameData frameToPlay = this.listRecordFrame[this.currentReplayframe];
        SetBoardFromReplayData(frameToPlay);
    }
    private void LastFrame()
    {
        if (this.listRecordFrame == null || this.listRecordFrame.Count == 0)
            return;

        this.currentReplayframe = this.listRecordFrame.Count - 1; // �ε����� ���������� ����
        ReplayFrameData frameToPlay = this.listRecordFrame[this.currentReplayframe];
        SetBoardFromReplayData(frameToPlay);
    }
    private void EventsInit()
    {
        this.recordPanelCR.OnFirstMoveEvent += FirstFrame;
        this.recordPanelCR.OnLastMoveEvent += LastFrame;
        this.recordPanelCR.OnNextMoveEvent += NextFrame;
        this.recordPanelCR.OnPrevMoveEvent += PrevFrame;
    }
}
