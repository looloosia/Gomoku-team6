using UnityEngine;
using UnityEngine.UI;

public class GameOptionPopup : BasePopup
{
    [SerializeField] private Button playAiBtn;
    [SerializeField] private Button playMultiBtn;

    protected override void Init()
    {
        BindButtons();
    }
    
    public override void Show()
    {
        base.Show();
    }

    public override void Hide(PopupHideDelegate onComplete = null)
    {
        onComplete?.Invoke();
        gameObject.SetActive(false);
    }

    private void BindButtons()
    {
        playAiBtn.onClick.AddListener(OnClickPlayAi);
        playMultiBtn.onClick.AddListener(OnClickPlayMulti);
    }

    private void OnClickPlayAi()
    {
        GameManager.Instance.ChangeToGameScene(Constants.GameType.SinglePlay);
    }

    private void OnClickPlayMulti()
    {
        GameManager.Instance.ChangeToGameScene(Constants.GameType.LocalDualPlay);
    }
}
