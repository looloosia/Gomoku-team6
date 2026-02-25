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

        // 오른쪽 위부터 생성
        for (int y = 0; y < Constants.BOARD_SIZE; y++)
        {
            for (int x = 0; x < Constants.BOARD_SIZE; x++)
            {
                float posX = startPos.x + (x * scaledSpacing);
                float posY = startPos.y - (y * scaledSpacing);
                Vector2 spawnPos = new Vector2(posX, posY);

                // 블럭 생성
                GameObject objBlock = Instantiate(this.blockPrefab, spawnPos, Quaternion.identity);

                objBlock.transform.SetParent(this.blockParentPos);
                objBlock.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

                // 블럭 세팅
                Block block = objBlock.GetComponent<Block>();
                BlockData data = new BlockData(Constants.PlayerType.None, new Vector2Int(x, y));
                block.SetBlockData(data);

                // 블럭 딕셔너리 추가
                dicBlocks.Add((x, y), block);

                objBlock.name = $"Block_{(x, y)}";
            }
        }
        return dicBlocks;
    }
}