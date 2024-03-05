using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class loginFieldsUpdate : MonoBehaviour
{
    public Text email;
    public Text pass;
    public TMPro.TextMeshProUGUI loginText;
    public Text createEmail;
    public Text createPass;
    public Text createPass2;
    public TMPro.TextMeshProUGUI createText;

    public void resetFields()
    {
        email.color = Color.black;
        pass.color = Color.black;
        loginText.text = "Log in or sign up";
        createText.text = "Create your account";
        createEmail.color = Color.black;
        createPass.color = Color.black;
        createPass2.color = Color.black;
    }
}
