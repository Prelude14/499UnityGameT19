using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class readInput : MonoBehaviour
{   

    //Need public string to store the user's input when it is entered into the login page
    public Text username;
    public Text password;

    //LOGIN INPUTS
    public void ReadStringUsernameL(string usernameInputString)
    {
        username.text = usernameInputString; //store the entered text as the user's username
        //PlayerPrefs.SetString("uname", username.text);
        Debug.Log(username);
    }
    public void ReadStringPasswordL(string passwordInputString)
    {
        password.text = passwordInputString; //store the entered text as the user's password
        //PlayerPrefs.SetString("upass", password.text);
        Debug.Log(password);
    }

    //Need public string to store the user's input when it is entered into the Create Account page
    public Text usernameCA;
    public Text passwordCA_1;
    public Text passwordCA_2;

    //CREATE ACCOUNTS INPUTS
    public void ReadStringUsernameCA(string usernameInputStringCA)
    {
        usernameCA.text = usernameInputStringCA; //store the entered text as the user's username
        Debug.Log(usernameCA);
    }
    public void ReadStringPasswordCA_1(string passwordInputStringCA1)
    {
        passwordCA_1.text = passwordInputStringCA1; //store the entered text as the user's password they want to use
        Debug.Log(passwordCA_1);
    }
    public void ReadStringPasswordCA_2(string passwordInputStringCA2)
    {
        passwordCA_2.text = passwordInputStringCA2; //store the entered text as the "confirm password" password
        Debug.Log(passwordCA_2);
    }

}
