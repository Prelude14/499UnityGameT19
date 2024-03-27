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
        WWWForm formV = new WWWForm();
        formV.AddField("username", username.text);
        formV.AddField("token", token.text);

        usernameHolder.text = username.text;

        //connect to url of our database's php file, PASS FORM TO URL
        using (WWW wwwV = new WWW("http://localhost/sqlconnect/validate.php", formV))
        {
            yield return wwwV; //tell Unity to yield running the rest of the game till it gets this info from the url

            if (wwwV.error == null)
            {

                //Error check what our PHP file returned, index 0 should be the first character, 0 means everything worked perfectly
                if (wwwV.text[0] == '0')
                {
                    Debug.Log("Token Valid");
                    newPasswordInput.SetActive(true);
                    resetTokenInput.SetActive(false);
                }
                else
                {
                    Debug.Log("Validation FAILED. Error Code: " + wwwV.text);
                    token.color = Color.red;
                    token.text = "Invalid Token";
                }
            }
        }
    }
}