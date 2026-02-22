using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static Constants;

public class Board : MonoBehaviour
{
    [SerializeField]
    private BoardGenerator boardGenerator;

    //temp
    [SerializeField]
    private PlayerChange playerChange;
    [SerializeField]
    private Replay replay;
    [SerializeField]
    private End end;
    
    //key: 블럭 위치
    private Dictionary<(int, int), Block> dicBlocks = new Dictionary<(int, int), Block>();
    
    private List<ReplayFrameData> listReplayFrame = new List<ReplayFrameData>();

    void Awake()
    {
        this.dicBlocks = this.boardGenerator.GenerateBoard();

        InitEvents();

        SaveReplayFrame();
    }
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            StoneOnClick();
        }
    }

    private void StoneOnClick()
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(screenPosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            Block clickedBlock = hit.collider.GetComponent<Block>();

            if (clickedBlock != null && clickedBlock.GetBlockData().markerType == PlayerType.None)
            {
                if(this.playerChange.Type == PlayerType.Black)
                    clickedBlock.SetBlackStone();
                else if(this.playerChange.Type == PlayerType.White)
                    clickedBlock.SetWhiteStone();
                SaveReplayFrame();
            }
        }
    }
    private void InitEvents()
    {
        this.end.SetReplayCallback(() =>
        {
            SaveReplayJson();
            BoardReset();
        });

        this.replay.SetReplayCallback(() =>
        {

        });
    }
    public int RandomStone()
    {
        int rand = Random.Range(1, 3);
        return rand;
    }

    private void SaveReplayJson()
    {
        ReplaySaveData data = new ReplaySaveData(this.listReplayFrame);
        string json = JsonUtility.ToJson(data, true);

        string folderPath = Application.dataPath + "/Replay";
        string filePath = folderPath + "/ReplayData_Test.json";
        File.WriteAllText(filePath, json);

        Debug.Log("파일 저장 완료! 경로: " + filePath);
    }
    private void SaveReplayFrame()
    {
        BlockData[] blocks = this.dicBlocks.Values.Select(x => x.GetBlockData()).ToArray();

        ReplayFrameData frameData = new ReplayFrameData(blocks);

        this.listReplayFrame.Add(frameData);
    }
    private void BoardReset()
    {
        foreach (Block block in this.dicBlocks.Values)
        {
            block.ResetStone();
        }
    }


    //public void DicTest()
    //{
    //    foreach(KeyValuePair<(int, int), Block> pair in this.dicBlocks)
    //    {
    //        Debug.Log($"BlockName: {pair.Value.name}, Block Position: {pair.Key.Item1}, {pair.Key.Item2}");
    //    }
    //}
}
