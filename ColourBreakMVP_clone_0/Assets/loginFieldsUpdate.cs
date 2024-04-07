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

    public void resetFields()
    {
        email.color = Color.black;
        pass.color = Color.black;
        loginText.text = "Log in or sign up";
    }
}
