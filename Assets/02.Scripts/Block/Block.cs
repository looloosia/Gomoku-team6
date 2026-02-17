using UnityEngine;
using static Constants;

public class Block : MonoBehaviour
{
    public Constants.eMarkerType markerType = Constants.eMarkerType.None;

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
    public void SetStone(eMarkerType markerType)
    {
        this.markerType = markerType;
        switch (markerType)
        {
            case eMarkerType.White:
                this.stone.sprite = this.whiteStone;
                break;
            case eMarkerType.Black:
                this.stone.sprite = this.blackStone;
                break;
            case eMarkerType.None:
                this.stone.sprite = null;
                break;
        }
    }
}
