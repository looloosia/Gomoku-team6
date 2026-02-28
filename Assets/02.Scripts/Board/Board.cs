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
    private Button btnSave;

    [SerializeField]
    private PlayerType currentType;

    //���� ��
    private Block currentBlock;

    //key: �� ��ġ(row, col)
    private Dictionary<(int, int), Block> dicBlocks = new Dictionary<(int, int), Block>();
    public Dictionary<(int, int), Block> DicBlocks =>  this.dicBlocks;
    
    private List<ReplayFrameData> listReplayFrame = new List<ReplayFrameData>();

    public UnityAction<Block> onPlaceStone;

    void Awake()
    {
        this.dicBlocks = this.boardGenerator.GenerateBoard();

        InitEvents();
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
    public void SetCurrentStone(PlayerType type)
    {
        this.currentType = type;
    }
    public void UpdateBlock(PlayerType[,] virtualBoard)
    {
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
                if(clickedBlock.GetBlockData().markerType !=PlayerType.None)
                {
                    return;
                }
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
        this.currentBlock.SetPlacementImage(this.currentType);
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

        if (this.currentType == PlayerType.Black)
            this.currentBlock.SetBlackStone();
        else if (this.currentType == PlayerType.White)
            this.currentBlock.SetWhiteStone();

        this.onPlaceStone?.Invoke(this.currentBlock);

        this.currentBlock = null;

        //����
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
            // [���� �̸�]
            listRecordFrameData = this.listReplayFrame,
            recordName = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss"),

            // [��¥]
            date = DateTime.Now.ToString("yyyy-MM-dd"),
            time = DateTime.Now.ToString("HH:mm"),

            // [���� ����]
            gameType = GameType.SinglePlay, 

            // [���� ����]
            nickName = "",              // ���� �г���
            rank = "",                   // ���� �޼�

            // [��� �� ���]
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

        Debug.Log("���� ���� �Ϸ�! ���: " + filePath);
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
            PlayerType type = block.GetBlockData().markerType;

            if (type == PlayerType.Black || type == PlayerType.White)
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
