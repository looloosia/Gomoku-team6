using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer stone;
    private (int x, int y) boardPos;
    public (int x, int y) BoardPos => this.boardPos;

    private bool hasStone;
    public bool HasStone => this.hasStone;

    public void SetBlockPosition(int x, int y)
    {
        this.boardPos = (x, y);
    }
    public void SetStone(bool hasStone, Sprite sprite)
    {
        this.hasStone = true;
        this.stone.sprite = sprite;
    }
}
