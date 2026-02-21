using UnityEngine;
using static Constants;

public class Block : MonoBehaviour
{
    public Constants.PlayerType markerType = Constants.PlayerType.None;

    [SerializeField]
    private SpriteRenderer stone;
    [SerializeField]
    private Sprite whiteStone;
    [SerializeField]
    private Sprite blackStone;

    private (int x, int y) boardPos;
    public (int x, int y) BoardPos => this.boardPos;


    public void SetBlockPosition(int x, int y)
    {
        this.boardPos = (x, y);
    }
    public void SetStone(PlayerType markerType)
    {
        this.markerType = markerType;
        switch (markerType)
        {
            case PlayerType.White:
                this.stone.sprite = this.whiteStone;
                break;
            case PlayerType.Black:
                this.stone.sprite = this.blackStone;
                break;
            case PlayerType.None:
                this.stone.sprite = null;
                break;
        }
    }
}
