using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelController : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject SignupPopup;
    
    [Header("TMP")]
    [SerializeField] private TMP_InputField idInput;
    [SerializeField] private TMP_InputField pwInput;
    [SerializeField] private TMP_Text loginErrorText;

    [Header("Buttons")]
    [SerializeField] private Button signupBtn;
    [SerializeField] private Button loginBtn;
    [SerializeField] private Button settingBtn;

    void Start()
    {
        BindButtons();
    }

    private void BindButtons()
    {
        signupBtn.BindEventWithSound(OnClickShowSignupPopup);
        loginBtn.BindEventWithSound(OnClickLogin);
        settingBtn.BindEventWithSound(() => {UIManager.Instance.OpenSettingPopup();});
    }

    private void OnClickShowSignupPopup()
    {
        SignupPopup.SetActive(true);
    }

    public void OnClickLogin()
    {
        bool success = AccountManager.Instance.Login(idInput.text, pwInput.text);

        if (!success)
        {
            loginErrorText.text = "아이디 또는 비밀번호가 일치하지 않습니다.";
        }
        else
        {
            loginErrorText.text = "";
            GameManager.Instance.ChangeToLobbyScene();
        }
    }
}
