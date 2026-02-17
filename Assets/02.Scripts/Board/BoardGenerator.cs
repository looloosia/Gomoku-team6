using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField]
    public GameObject blockPrefab;
    [SerializeField]
    private Transform blockParentPos;

    [SerializeField]
    private Transform startPos;     //첫 블럭 위치

    private float spacing = 0.6f;   //블럭 간격


    public Dictionary<(int, int), Block> GenerateBoard()
    {
        Dictionary<(int, int), Block> dicBlocks = new Dictionary<(int, int), Block>();

        Vector2 startPos = this.startPos.position;

        for (int y = 0; y < Constants.BOARD_SIZE; y++)
        {
            for (int x = 0; x < Constants.BOARD_SIZE; x++)
            {
                //블럭 위치
                float posX = startPos.x + (x * this.spacing);
                float posY = startPos.y - (y * this.spacing);
                Vector2 spawnPos = new Vector2(posX, posY);
                
                //블럭 생성
                GameObject newBlock = Instantiate(this.blockPrefab, spawnPos, Quaternion.identity, this.transform);
                newBlock.transform.SetParent(this.blockParentPos);

                //블럭 세팅
                Block block = newBlock.GetComponent<Block>();
                block.SetBlockPosition(x, y);

                //블럭 딕셔너리 추가
                dicBlocks.Add(block.BoardPos, block);

                newBlock.name = $"Block_{block.BoardPos}";
            }
        }
        return dicBlocks;
    }
}