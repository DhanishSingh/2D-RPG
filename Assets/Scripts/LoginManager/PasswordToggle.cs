using TMPro;
using UnityEngine;

public class PasswordToggle : MonoBehaviour
{
    public TMP_InputField passwordInput;
    public TMP_Text buttonText;

    bool isHidden = true;

    public void TogglePassword()
    {
        if (isHidden)
        {
            passwordInput.contentType = TMP_InputField.ContentType.Standard;
            buttonText.text = "Hide";
            isHidden = false;
        }
        else
        {
            passwordInput.contentType = TMP_InputField.ContentType.Password;
            buttonText.text = "Show";
            isHidden = true;
        }

        passwordInput.ForceLabelUpdate();
    }
}