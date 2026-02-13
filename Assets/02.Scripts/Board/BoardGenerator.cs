using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [Header("설정")]
    public GameObject blockPrefab;
    [SerializeField]
    private Transform blockParentPos;
    [SerializeField]
    private Transform startPos;     //첫 블럭 위치
    private int boardSize = 15;     //오목판 사이즈 (15x15)
    private float spacing = 0.6f;   //블럭 간격

    void Awake()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        Vector2 startPos = this.startPos.position;

        for (int y = 0; y < this.boardSize; y++)
        {
            for (int x = 0; x < this.boardSize; x++)
            {
                
                float posX = startPos.x + (x * this.spacing);
                float posY = startPos.y - (y * this.spacing);
                Vector2 spawnPos = new Vector2(posX, posY);
                
                //블럭 생성
                GameObject newBlock = Instantiate(this.blockPrefab, spawnPos, Quaternion.identity, this.transform);
                newBlock.transform.SetParent(this.blockParentPos);

                Block block = newBlock.GetComponent<Block>();
                block.SetBlockPosition(x, y);

                newBlock.name = $"Block_{x}_{y}";
            }
        }


    }
}