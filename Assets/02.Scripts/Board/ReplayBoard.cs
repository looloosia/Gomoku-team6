using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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
    void Awake()
    {
        this.dicBlocks = this.boardGenerator.GenerateBoard();
        LoadReplayJson();

        this.btnPrev.onClick.AddListener(PrevFrame);
        this.btnNext.onClick.AddListener(NextFrame);
    }
    private void LoadReplayJson()
    {
        string folderPath = Application.dataPath + "/Replay";
        string filePath = folderPath + "/ReplayData_Test.json"; // 예시 파일명

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("저장된 리플레이 파일이 없습니다! 경로를 확인해주세요: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        ReplaySaveData loadData = JsonUtility.FromJson<ReplaySaveData>(json);
        List<ReplayFrameData> data = loadData.listReplayFrameData;
        this.listReplayFrame = data;

        Debug.Log("Json 로드 완료");
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
