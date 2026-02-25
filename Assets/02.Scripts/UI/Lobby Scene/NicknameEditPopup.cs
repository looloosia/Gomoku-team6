using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NicknameEditPopup : BasePopup
{
    [Header("Input & Error UI")]
    [SerializeField] private TMP_InputField nicknameInput; 
    [SerializeField] private TMP_Text errorText;           

    [Header("Buttons")]
    [SerializeField] private Button confirmBtn;

    private Action onNicknameChangedCallback;
    
    protected override void Init()
    {
        BindButtons();
    }
    
    public void Show(Action onSuccess)
    {
        base.Show();
        onNicknameChangedCallback = onSuccess;
        
        nicknameInput.text = ""; 
        errorText.text = ""; 
    }

    public override void Hide(PopupHideDelegate onComplete = null)
    {
        onComplete?.Invoke();
        gameObject.SetActive(false);
    }

    private void BindButtons()
    {
        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
    }

    private void OnClickConfirmBtn()
    {
        string inputName = nicknameInput.text;
        string resultMsg; 

        // AccountManager에게 닉네임 쓸 수 있는지 물어보기
        bool isSuccess = AccountManager.Instance.TryChangeNickname(inputName, out resultMsg);

        if (isSuccess)
        {
            // 성공: 창 닫고 콜백(프로필 창 새로고침) 실행
            gameObject.SetActive(false);
            onNicknameChangedCallback?.Invoke();

            // 만능 알림창 띄워서 "성공했습니다!" 확인 도장 찍어주기
            ConfirmPopup popup = GameManager.Instance.OpenConfirmPopup();
            popup.Show("닉네임 변경이 완료되었습니다!", "", "", null, "확인", null);
        }
        else
        {
            // 3. 실패: 창 유지하고 텍스트만 빨간색 경고로 변경
            errorText.text = resultMsg; 
            errorText.color = Color.red;
        }
    }
}
