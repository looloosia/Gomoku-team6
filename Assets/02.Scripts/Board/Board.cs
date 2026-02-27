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
    [SerializeField]
    private GamePanelController panel;
    
    //temp
    [SerializeField]
    private PlayerChange stoneChange;
    [SerializeField]
    private Button btnSave;

    //착수 블럭
    private Block currentBlock;

    //key: 블럭 위치(row, col)
    private Dictionary<(int, int), Block> dicBlocks = new Dictionary<(int, int), Block>();
    
    private List<ReplayFrameData> listReplayFrame = new List<ReplayFrameData>();

    public UnityAction<Block> onPlaceStone;

    void Awake()
    {
        this.dicBlocks = this.boardGenerator.GenerateBoard();

        InitEvents();
        BlockInit();
        SaveReplayFrame();

        //test
        this.btnSave.onClick.AddListener(() =>
        {
            SaveReplayJson();
            BoardReset();
        });
    }
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            StoneOnClick();
        }
    }
    public void BlockInit()
    {
        foreach (Block block in this.dicBlocks.Values)
        {
            block.Init(() =>
            {
                this.onPlaceStone?.Invoke(block);
            });
        }
    }
    public void UpdateBlock()
    {
        var virtualBoard = GameManager.Instance.GameLogic.VirtualBoard;
        int rows = virtualBoard.GetLength(0);
        int cols = virtualBoard.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                PlayerType type = virtualBoard[row, col];

                if (this.dicBlocks.TryGetValue((row, col), out Block block))
                {
                    BlockData data = block.GetBlockData();

                    data.markerType = type;

                    block.SetBlockData(data);
                }
            }
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

            if (clickedBlock != null)
            {
                if (this.currentBlock != null)
                    this.currentBlock.ResetStone();

                OnClick(clickedBlock);
            }
        }
    }
    public void OnClick(Block block)
    {
        this.panel.OnStoneTemporarilyPlaced();
        this.currentBlock = block;
        this.currentBlock.SetPlacementImage(this.stoneChange.Type);
    }
    private void InitEvents()
    {
        this.panel.OnConfirmMoveEvent += PutStone;
        this.panel.OnReturnMoveEvent += Return;

        this.panel.OnResignEvent += SaveReplayJson;
        this.panel.OnResignEvent += BoardReset;
    }
    private void PutStone()
    {
        if (this.currentBlock == null)
            return;

        if (this.stoneChange.Type == PlayerType.Black)
            this.currentBlock.SetBlackStone();
        else if (this.stoneChange.Type == PlayerType.White)
            this.currentBlock.SetWhiteStone();

        this.onPlaceStone?.Invoke(this.currentBlock);

        this.currentBlock = null;

        //저장
        SaveReplayFrame();
    }
    private void Return()
    {
        if (this.currentBlock == null)
            return;

        this.currentBlock.ResetStone();
        this.currentBlock = null;
    }
    private void SaveReplayJson()
    {
        string fileName = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss");
        string replayName = DateTime.Now.ToString("(yy/MM/dd) HH:mm:ss");
        
        ReplaySaveData data = new ReplaySaveData
        {
            // [파일 이름]
            listRecordFrameData = this.listReplayFrame,
            recordName = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss"),

            // [날짜]
            date = DateTime.Now.ToString("yyyy-MM-dd"),
            time = DateTime.Now.ToString("HH:mm"),

            // [게임 종류]
            gameType = GameType.SinglePlay, 

            // [상대방 정보]
            nickName = "",              // 상대방 닉네임
            rank = "",                   // 상대방 급수

            // [결과 및 통계]
            result = GameResult.None,         
            resultType = GameResultType.None, 

            winStoneType = PlayerType.None,   
            myStoneType = PlayerType.None,    

            totalStone = TotalStoneCount()
        };
        
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
    private int TotalStoneCount()
    {
        int count = 0;

        foreach (Block block in this.dicBlocks.Values)
        {
            Constants.PlayerType type = block.GetBlockData().markerType;

            if (type == Constants.PlayerType.Black || type == Constants.PlayerType.White)
            {
                count++;
            }
        }

        return count;
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
}
