using UnityEngine;
using UnityEngine.UI;

public class GameOptionPopup : BasePopup
{
    [SerializeField] private Button playAiBtn;
    [SerializeField] private Button playMultiBtn;

    protected override void Init()
    {
        playAiBtn.onClick.AddListener(OnClickPlayAi);
        playMultiBtn.onClick.AddListener(OnClickPlayMulti);
    }

    private void OnClickPlayAi()
    {
        // 게임 씬 이동
    }

    private void OnClickPlayMulti()
    {
        // 게임 씬 이동
    }
}
