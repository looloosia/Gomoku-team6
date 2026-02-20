using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignupPopupView : MonoBehaviour
{
    [Header("InputField")]
    public TMP_InputField idInput;
    public TMP_InputField pwInput;
    public TMP_InputField confirmPwInput;
    
    [Header("Error Text")]
    public TMP_Text idCheckText;
    public TMP_Text pwCheckText;
    public TMP_Text confirmPwText;

    [Header("Buttons")]
    public Button idDuplicateBtn;
    public Button registerBtn;
    public Button cancelBtn;

    public void SetIdMessage(string msg, Color color)
    {
        idCheckText.text = msg;
        idCheckText.color = color;
    }

    public void SetPasswordMessage(string msg)
    {
        pwCheckText.text = msg;
    }

    public void SetConfirmPwMessage(string msg)
    {
        confirmPwText.text = msg;
    }

    public void ClearAll()
    {
        idInput.text = "";
        pwInput.text = "";
        confirmPwInput.text = "";

        idCheckText.text = "";
        pwCheckText.text = "";
        confirmPwText.text = "";
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

}
