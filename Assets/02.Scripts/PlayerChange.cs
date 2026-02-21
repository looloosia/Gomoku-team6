using UnityEngine;
using UnityEngine.UI;

public class PlayerChange : MonoBehaviour
{
    [SerializeField]
    private Button btnPlayerChange;

    private Constants.PlayerType type = Constants.PlayerType.Black;
    public Constants.PlayerType Type => this.type;

    void Awake()
    {
        this.btnPlayerChange.onClick.AddListener(() =>
        {
            this.type = (this.type == Constants.PlayerType.Black) ? Constants.PlayerType.White : Constants.PlayerType.Black;
        });
    }
}
