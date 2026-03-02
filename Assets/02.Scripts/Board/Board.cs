using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Constants;
using static UnityEngine.Audio.ProcessorInstance;

public class Board : MonoBehaviour
{
    [SerializeField]
    private BoardGenerator boardGenerator;
    [SerializeField]
    private GamePanelController panel;

    private PlayerType currentType;

    //���� ��
    private Block currentBlock;

    //key: �� ��ġ(row, col)
    private Dictionary<(int, int), Block> dicBlocks = new Dictionary<(int, int), Block>();


    public UnityAction<Block> onPlaceStone;

    void Awake()
    {
        this.dicBlocks = this.boardGenerator.GenerateBoard();
        SaveReplayFrame();
        InitEvents();
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
        //SaveReplayFrame();
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
                if (clickedBlock.GetBlockData().markerType != PlayerType.None)
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

        this.panel.OnResignEvent += BoardReset;
    }
    private void PutStone()
    {
        if (this.currentBlock == null)
            return;

        this.onPlaceStone?.Invoke(this.currentBlock);

        this.currentBlock = null;
    }
    private void Return()
    {
        if (this.currentBlock == null)
            return;

        this.currentBlock.ResetStone();
        this.currentBlock = null;
    }

    public void SaveReplayFrame()
    {
        BlockData[] blocks = this.dicBlocks.Values.Select(x =>
        {
            BlockData originalData = x.GetBlockData();

            if (originalData.markerType == PlayerType.Forbidden)
            {
                return new BlockData()
                {
                    markerType = PlayerType.None,
                    row = originalData.row,
                    col = originalData.col
                };
            }

            return originalData;
        }).ToArray();

        ReplayFrameData frameData = new ReplayFrameData(blocks);

        ReplayManager.Instance.AddMoveRecord(frameData);
    }
    public int TotalStoneCount()
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

        ReplayManager.Instance.ClearMoveRecord();
    }
}
