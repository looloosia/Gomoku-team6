using UnityEngine;
using UnityEngine.UI;

public abstract class BasePopup : MonoBehaviour
{
    [Header("Base Popup UI")]
    [SerializeField] protected Button closeBtn;

    // TODO : [애니메이션]
    // [SerializeField] protected RectTransform panelTransform; 
    // [SerializeField] protected CanvasGroup canvasGroup;

    // 팝업이 숨겨질 때 실행할 델리게이트 선언
    public delegate void PopupHideDelegate();

    protected virtual void Start()
    {
        // 닫기 버튼이 연결 되어있다면 자동으로 Hide() 메소드 연결
        if (closeBtn != null)
            closeBtn.onClick.AddListener(() => Hide());

        Init();
    }

    protected virtual void Init() {}

    public virtual void Show()
    {
        // TODO: [애니메이션]
        /*
        canvasGroup.alpha = 0;
        panelTransform.localScale = Vector3.zero;
        canvasGroup.DOFade(1, 0.3f).SetEase(Ease.Linear);
        panelTransform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        */
    }

    public virtual void Hide(PopupHideDelegate onComplete = null)
    {
        // TODO: [애니메이션]
        /*
        canvasGroup.DOFade(0, 0.3f).SetEase(Ease.Linear);
        panelTransform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            onComplete?.Invoke();
            Destroy(gameObject);
        });
        */
        // 애니메이션 없이 바로 콜백 실행 후 팝업 파괴
        // 애니메이션 추가시 아래 두 줄은 삭제
        onComplete?.Invoke(); 
        Destroy(gameObject);
    }

    // 핵심 추가: 자식(ConfirmPopup)이 이 함수의 역할을 덮어쓸 수 있게 만듦
    protected virtual void OnClickClosePopup()
    {
        // 기본 동작은 그냥 닫기 (SettingPopup 같은 애들은 이거 그대로 씀)
        Hide(); 
    }
}
