using TMPro;
using UnityEngine;

public class SignupPopupController : MonoBehaviour
{
    [SerializeField] private SignupPopupView view;

    private bool isIdChecked = false;
    private string lastCheckedId = "";

    private void Start()
    {
        BindButtons();
        view.idInput.onValueChanged.AddListener(OnIdChanged);
    }

    private void BindButtons()
    {
        view.idDuplicateBtn.BindEventWithSound(OnClickCheckDuplicate);
        view.registerBtn.BindEventWithSound(OnClickRegister);
        view.cancelBtn.BindEventWithSound(OnClickCancel);
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

        if (AccountManager.Instance.CheckDuplicate(id))
        {
            view.SetIdMessage("중복된 이메일입니다.", Color.red);
            isIdChecked = false;
        }
        else
        {
            view.SetIdMessage("사용 가능한 이메일입니다.", Color.green);
            isIdChecked = true;
            lastCheckedId = id;
        }
            
    }

    // 회원가입 버튼
    private void OnClickRegister()
    {
        string id = view.idInput.text.Trim();
        string pw = view.pwInput.text;
        string confirmPw = view.confirmPwInput.text;

        bool isValid = true;

        // 이메일 중복확인 여부
        if (!isIdChecked || lastCheckedId != id)
        {
          view.SetIdMessage("이메일 중복확인을 해주세요.", Color.red);
          return;
        }

        // 비밀번호 조건 검사
        if (!AccountManager.Instance.IsValidPassword(pw))
        {
            view.SetPasswordMessage("비밀번호는 영문자/숫자 포함 8자 이상 입력해주세요.");
            isValid = false;
        }
        else
        {
            view.SetPasswordMessage("");
        }

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
        bool success = AccountManager.Instance.Signup(id, pw);

        if (!success)
        {
            view.SetIdMessage("이메일 중복확인을 해주세요.", Color.red);
            return;
        }

        // 성공 시 팝업 닫기 + 초기화
        view.ClearAll();
        view.Hide();
    }

    private void OnIdChanged(string _)
    {
        isIdChecked = false;
    }

    // 취소 버튼
    private void OnClickCancel()
    {
        view.ClearAll();
        view.Hide();
    }
}
