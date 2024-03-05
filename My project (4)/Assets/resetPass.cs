using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class resetPass : MonoBehaviour
{
    public Text username;

    public GameObject resetTokenInput;

    public GameObject resetemailInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

       public void CallResetToken()
    {
        StartCoroutine(sendResetToken());
    }

    IEnumerator sendResetToken()
    {
        //make Form to take the user's input 
        WWWForm form = new WWWForm();
        form.AddField("username",  username.text);

        //connect to url of our database's php file, PASS FORM TO URL
        WWW www = new WWW("http://localhost/sqlconnect/reset.php", form);
        yield return www; //tell Unity to yield running the rest of the game till it gets this info from the url

        //Error check what our PHP file returned, index 0 should be the first character, 0 means everything worked perfectly
        if (www.text[0] == '0')
        {
            Debug.Log("Token successfully created:  " + www.text);
            resetemailInput.SetActive(false);
            resetTokenInput.SetActive(true);
        }
        else
        {
            Debug.Log("Password reset failed. Error Code: " + www.text);
            username.color = Color.red;
            username.text = "Invalid Username";
        }
    }

    // public void VerifyInputL() //button won't be clickable until input field has at least 8 characters
    // {
    //     resetPassButton.interactable = (DBManager.username.text.Length >= 8);
    // }

}
