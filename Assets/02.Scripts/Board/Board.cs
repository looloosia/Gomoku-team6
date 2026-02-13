using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private BoardGenerator boardGenerator;

    private Dictionary<(int, int), Block> dicBlocks = new Dictionary<(int, int), Block>();

    void Awake()
    {
        this.dicBlocks = this.boardGenerator.GenerateBoard();
        
    }

    //public void DicTest()
    //{
    //    foreach(KeyValuePair<(int, int), Block> pair in this.dicBlocks)
    //    {
    //        Debug.Log($"BlockName: {pair.Value.name}, Block Position: {pair.Key.Item1}, {pair.Key.Item2}");
    //    }
    //}
}
