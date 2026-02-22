using System.Collections.Generic;
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
    

    private Dictionary<(int, int), Block> dicBlocks = new Dictionary<(int, int), Block>();

    void Awake()
    {
        this.dicBlocks = this.boardGenerator.GenerateBoard();

        InitEvents();
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
            }
        }
    }
    private void InitEvents()
    {
        this.end.SetReplayCallback(BoardReset);

        this.replay.SetReplayCallback(Replay);
    }
    public int RandomStone()
    {
        int rand = Random.Range(1, 3);
        return rand;
    }

    private void Replay()
    {

    }
    private void SaveReplay()
    {
        foreach (Block block in this.dicBlocks.Values)
        {
            
        }
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
