using UnityEngine;

public class Block : MonoBehaviour
{
    private SpriteRenderer marker;
    private (int x, int y) boardPos;
    public (int x, int y) BoardPos => this.boardPos;

    public void SetBlockPosition(int x, int y)
    {
        this.boardPos = (x, y);
    }
}
