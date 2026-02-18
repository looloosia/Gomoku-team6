using TMPro;
using UnityEngine;

public class LoginPanelController : MonoBehaviour
{
    [SerializeField] private GameObject registerPopup;
    
    [SerializeField] private TMP_InputField idInput;
    [SerializeField] private TMP_InputField pwInput;
    [SerializeField] private TMP_Text loginErrorText;

    public void OnClickLogin()
    {
        // bool succes = AccountManager.Instance.Login(idInput.text, pwInput.text);

        // if (!success)
        // {
        //     loginErrorText.text = "아이디 또는 비밀번호가 일치하지 않습니다.";
        // }
        // else
        // {
        //     loginErrorText.text = "";
        //     // 씬 전환
        // }
    }
}
