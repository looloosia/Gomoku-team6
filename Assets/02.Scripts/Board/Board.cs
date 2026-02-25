using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Constants;

public class Board : MonoBehaviour
{
    [SerializeField]
    private BoardGenerator boardGenerator;

    //temp
    [SerializeField]
    private PlayerChange playerChange;
    [SerializeField]
    private End end;

    [SerializeField]
    private Button btnPut;
    [SerializeField]
    private Button btnCancel;

    //착수 블럭
    private Block tempBlock;

    //key: 블럭 위치
    private Dictionary<(int, int), Block> dicBlocks = new Dictionary<(int, int), Block>();
    
    private List<ReplayFrameData> listReplayFrame = new List<ReplayFrameData>();

    public UnityAction onPlaceStone;

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
                //착수 블럭
                if (this.tempBlock != null)
                    this.tempBlock.ResetStone();
                this.tempBlock = clickedBlock;
                this.tempBlock.SetPlacementImage(this.playerChange.Type);
            }
        }
    }
    private void InitEvents()
    {
        this.btnPut.onClick.AddListener(PutStone);
        this.btnCancel.onClick.AddListener(CancelStone);

        this.onPlaceStone = StoneOnClick;

        //temp
        this.end.SetReplayCallback(() =>
        {
            SaveReplayJson();
            BoardReset();
        });
    }
    private void PutStone()
    {
        if (this.tempBlock == null)
            return;
        if (this.playerChange.Type == PlayerType.Black)
            this.tempBlock.SetBlackStone();
        else if (this.playerChange.Type == PlayerType.White)
            this.tempBlock.SetWhiteStone();
        this.tempBlock = null;

        SaveReplayFrame();
    }
    private void CancelStone()
    {
        if (this.tempBlock == null)
            return;
        this.tempBlock.ResetStone();
    }
    private void SaveReplayJson()
    {
        string fileName = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss");
        string replayName = DateTime.Now.ToString("(yy/MM/dd) HH:mm:ss");
        string player1NickName = "Player1";
        string player2NickName = "Player2";
        GameResult result =  GameResult.Win;

        ReplaySaveData data = new ReplaySaveData(this.listReplayFrame, replayName, player1NickName, 
                                                    player2NickName, result);

        string json = JsonUtility.ToJson(data, true);

        string folderPath = Application.dataPath + "/Replay";
        string filePath = folderPath + $"/Replay_{fileName}.json";
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

        if (this.listReplayFrame.Count > 1)
        {
            this.listReplayFrame.RemoveRange(1, this.listReplayFrame.Count - 1);
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
