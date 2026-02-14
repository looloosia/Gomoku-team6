using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Sprite whiteStone;
    [SerializeField]
    private Sprite blackStone;

    [SerializeField]
    private BoardGenerator boardGenerator;

    private Dictionary<(int, int), Block> dicBlocks = new Dictionary<(int, int), Block>();

    void Awake()
    {
        this.dicBlocks = this.boardGenerator.GenerateBoard();
    }
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            StoneOnClick();
        }
    }

    private void StoneOnClick()
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(screenPosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            Block clickedBlock = hit.collider.GetComponent<Block>();

            if (clickedBlock != null && !clickedBlock.HasStone)
            {
                //µ¹ »ý¼º
                clickedBlock.SetStone(true, RandomStone());
            }
        }
    }
    public Sprite RandomStone()
    {
        Sprite randomSprite = Random.Range(0, 2) == 0 ? this.blackStone : this.whiteStone;
        return randomSprite;
    }
    //public void DicTest()
    //{
    //    foreach(KeyValuePair<(int, int), Block> pair in this.dicBlocks)
    //    {
    //        Debug.Log($"BlockName: {pair.Value.name}, Block Position: {pair.Key.Item1}, {pair.Key.Item2}");
    //    }
    //}
}
