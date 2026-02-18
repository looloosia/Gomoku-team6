using TMPro;
using UnityEngine;

public class RegisterPopupController : MonoBehaviour
{
    [SerializeField] private RegisterPopupView view;

    private void Start()
    {
        BindButtons();
    }

    private void BindButtons()
    {
        view.idDuplicateBtn.onClick.AddListener(OnClickCheckDuplicate);
        view.registerBtn.onClick.AddListener(OnClickRegister);
        view.cancelBtn.onClick.AddListener(OnClickCancel);
    }

    // 이메일 중복 확인 버튼
    private void OnClickCheckDuplicate()
    {
        string id = view.idInput.text.Trim();

        if (string.IsNullOrEmpty(id))
        {
            view.SetIdMessage("이메일을 입력해주세요.", Color.red);
            return;
        }

        // if (AccountManager.Instance.CheckDuplicate(id))
        //     view.SetIdMessage("중복된 이메일입니다.", Color.red);
        // else
        //     view.SetIdMessage("사용 가능한 이메일입니다.", Color.green);
    }

    // 회원가입 버튼
    private void OnClickRegister()
    {
        string id = view.idInput.text.Trim();
        string pw = view.pwInput.text;
        string confirmPw = view.confirmPwInput.text;

        bool isValid = true;

        // 비밀번호 조건 검사
        // if (!AccountManager.Instance.IsValidPassword(pw))
        // {
        //     view.SetPasswordMessage("비밀번호는 영문자/숫자 포함 8자 이상 입력해주세요.");
        //     isValid = false;
        // }
        // else
        // {
        //     view.SetPasswordMessage("");
        // }

        // 재입력 검사
        if (pw != confirmPw)
        {
            view.SetConfirmPwMessage("비밀번호가 일치하지 않습니다.");
            isValid = false;
        }
        else
        {
            view.SetConfirmPwMessage("");
        }

        if (!isValid)
            return;

        // 최종 회원가입 시도
        // bool success = AccountManager.Instance.Register(id, pw);

        // if (!success)
        // {
        //     view.SetIdMessage("이메일 중복확인을 해주세요)", Color.red);
        //     return;
        // }

        // 성공 시 팝업 닫기 + 초기화
        view.ClearAll();
        view.Hide();
    }

    // 취소 버튼
    private void OnClickCancel()
    {
        view.ClearAll();
        view.Hide();
    }
}
