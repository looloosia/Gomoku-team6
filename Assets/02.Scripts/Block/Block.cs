using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static Constants;

public class Block : MonoBehaviour
{
    private BlockData blockData;

    [SerializeField]
    private SpriteRenderer stone;
    [SerializeField]
    private Sprite whiteStone;
    [SerializeField]
    private Sprite standbyWhiteStone;
    [SerializeField]
    private Sprite blackStone;
    [SerializeField]
    private Sprite standbyBlackStone;

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
    public void SetPlacementImage(PlayerType markerType)
    {
        switch (markerType)
        {
            case PlayerType.White:
                this.stone.sprite = this.standbyWhiteStone;
                break;
            case PlayerType.Black:
                this.stone.sprite = this.standbyBlackStone;
                break;
            case PlayerType.None:
                this.stone.sprite = null;
                break;
        }
    }
    public void SetBlockData(BlockData blockData)
    {
        this.blockData = blockData;
        UpdateBlock();
    }
    public BlockData GetBlockData()
    {
        return this.blockData;
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
    public void UpdateBlock()
    {
        SetStone(this.blockData.markerType);
    }
}
