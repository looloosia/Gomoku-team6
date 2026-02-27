using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ConfirmPopup : BasePopup
{
    [Header("Texts")]
    [SerializeField] private TMP_Text msgText;
    [SerializeField] private TMP_Text submsgText; // 필요 없으면 꺼질 텍스트

    [Header("Buttons")]
    [SerializeField] private Button confirmBtn;
    [SerializeField] private TMP_Text confirmBtnText;

    [SerializeField] private Button cancelBtn; // 필요 없으면 꺼질 버튼
    [SerializeField] private TMP_Text cancelBtnText;

    // 현재 팝업이 1버튼(알림) 모드인지 기억하는 변수
    private bool isOneButtonMode = false; 

    public delegate void OnConfirmButtonClicked();
    private OnConfirmButtonClicked onConfirm;
    private OnConfirmButtonClicked onCancel;

    protected override void Init()
    {
        base.Init(); // 닫기(X) 버튼은 부모가 알아서 처리
        
        confirmBtn.onClick.AddListener(OnClickConfirm);
        if (cancelBtn != null)
            cancelBtn.onClick.AddListener(OnClickCancel);
    }

    // 외부(GameManager 등)에서 이 팝업을 띄울 때 부르는 함수, 만능 팝업창의 텍스트를 입맛대로 변경 가능
    public void Show(string msg, string submsg = "", string cancelStr = "취소", OnConfirmButtonClicked _onCancel = null, string confirmStr = "확인", OnConfirmButtonClicked _onConfirm = null)
    {
        // 1. 텍스트 세팅
        msgText.text = msg;

        if (string.IsNullOrEmpty(msg))
            msgText.gameObject.SetActive(false);
        else
        {
            msgText.gameObject.SetActive(true);
            msgText.text = msg;
        }

        // 부제목이 비어있으면(null이거나 "") 오브젝트 끄기
        if (string.IsNullOrEmpty(submsg))
            submsgText.gameObject.SetActive(false);
        else
        {
            submsgText.gameObject.SetActive(true);
            submsgText.text = submsg;
        }

        // 2. 확인 버튼 세팅
        confirmBtnText.text = confirmStr;
        onConfirm = _onConfirm;

        // 3. 취소 버튼 세팅 (취소 텍스트가 비어있으면 버튼 1개짜리 팝업으로 변신)
        if (string.IsNullOrEmpty(cancelStr))
        {
            cancelBtn.gameObject.SetActive(false);
            isOneButtonMode = true; // 1버튼 모드
        }
        else
        {
            cancelBtn.gameObject.SetActive(true);
            cancelBtnText.text = cancelStr;
            onCancel = _onCancel;
            isOneButtonMode = false; // 2버튼 모드
        }
        
        base.Show(); // 부모의 Show 호출
    }

    // 부모(BasePopup)의 [X] 버튼 동작을 가로채서(override) 내 입맛대로 바꿈
    protected override void OnClickClosePopup()
    {
        if (isOneButtonMode)
            OnClickConfirm(); 
        else
            OnClickCancel();
    }
    
    private void OnClickCancel()
    {
        Hide(() => onCancel?.Invoke());
    }

    private void OnClickConfirm()
    {
        Hide(() => onConfirm?.Invoke());
    }
}
