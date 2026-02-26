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

        float scaleFactor = 0.75f;
        float scaledSpacing = this.spacing * scaleFactor;

        for (int col = 0; col < Constants.BOARD_SIZE; col++)
        {
            for (int row = 0; row < Constants.BOARD_SIZE; row++)
            {
                float posX = startPos.x + (row * scaledSpacing);
                float posY = startPos.y - (col * scaledSpacing);

                Vector2 spawnPos = new Vector2(posX, posY);

                // 블럭 생성
                GameObject objBlock = Instantiate(this.blockPrefab, spawnPos, Quaternion.identity);

                objBlock.transform.SetParent(this.blockParentPos);
                objBlock.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

                // 블럭 세팅
                Block block = objBlock.GetComponent<Block>();
                BlockData data = new BlockData(Constants.PlayerType.None, col, row);
                block.SetBlockData(data);

                dicBlocks.Add((col, row), block);

                objBlock.name = $"Block_{(col, row)}";
            }
        }
        return dicBlocks;
    }
}