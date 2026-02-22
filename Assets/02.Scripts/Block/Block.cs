using UnityEngine;
using static Constants;

public class Block : MonoBehaviour
{
    private BlockData blockData;

    [SerializeField]
    private SpriteRenderer stone;
    [SerializeField]
    private Sprite whiteStone;
    [SerializeField]
    private Sprite blackStone;



    private void SetStone(PlayerType markerType)
    {
        SetBlockType(markerType);
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
    public BlockData GetBlockData()
    {
        return this.blockData;
    }
    public void SetBlockPosition(int x, int y)
    {
        this.blockData.boardPos = (x, y);
    }
    public void SetBlockType(PlayerType markerType)
    {
        this.blockData.markerType = markerType;
    }
    public void ResetStone()
    {
        SetStone(PlayerType.None);
    }
    public void SetWhiteStone()
    {
        SetStone(PlayerType.White);
    }
    public void SetBlackStone()
    {
        SetStone(PlayerType.Black);
    }
}
