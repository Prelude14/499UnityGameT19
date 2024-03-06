using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class validateToken : MonoBehaviour
{

    public Text username;
    public Text token;

    public GameObject newPasswordInput;
    public GameObject resetTokenInput;

    public Text usernameHolder;

    public void CallValidate()
    {
        StartCoroutine(validate());
    }

    IEnumerator validate()
    {
        //make Form to take the user's input 
        WWWForm form = new WWWForm();
        form.AddField("username", username.text);
        form.AddField("token", token.text);

        usernameHolder.text = username.text;

        //connect to url of our database's php file, PASS FORM TO URL
        WWW www = new WWW("http://localhost/sqlconnect/validate.php", form);
        yield return www; //tell Unity to yield running the rest of the game till it gets this info from the url

        //Error check what our PHP file returned, index 0 should be the first character, 0 means everything worked perfectly
        if (www.text[0] == '0')
        {
            Debug.Log("Token Valid");
            newPasswordInput.SetActive(true);
            resetTokenInput.SetActive(false);
        }
        else
        {
            Debug.Log("Validation FAILED. Error Code: " + www.text);
            token.color = Color.red;
            token.text = "Invalid Token";
        }
    }
}