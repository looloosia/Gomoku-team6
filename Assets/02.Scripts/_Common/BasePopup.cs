using UnityEngine;
using UnityEngine.UI;

public abstract class BasePopup : MonoBehaviour
{
    [Header("Base Popup UI")]
    [SerializeField] protected Button closeBtn;

    // 팝업이 숨겨질 때 실행할 델리게이트 선언
    public delegate void PopupHideDelegate();

    protected virtual void Start()
    {
        // 닫기 버튼이 연결 되어있다면 자동으로 Hide() 메소드 연결
        if (closeBtn != null)
            closeBtn.BindEventWithSound(() => Hide());

        Init();
    }

    protected virtual void Init() {}

    public virtual void Show() {}

    public virtual void Hide(PopupHideDelegate onComplete = null)
    {
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
