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
        WWWForm formR = new WWWForm();
        formR.AddField("username",  username.text);

        //connect to url of our database's php file, PASS FORM TO URL
        using (WWW wwwR = new WWW("http://localhost/sqlconnect/reset.php", formR))
        {
            yield return wwwR; //tell Unity to yield running the rest of the game till it gets this info from the url

            if (wwwR.error == null)
            {

                //Error check what our PHP file returned, index 0 should be the first character, 0 means everything worked perfectly
                if (wwwR.text[0] == '0')
                {
                    Debug.Log("Token successfully created:  " + wwwR.text);
                    resetemailInput.SetActive(false);
                    resetTokenInput.SetActive(true);
                }
                else
                {
                    Debug.Log("Password reset failed. Error Code: " + wwwR.text);
                    username.color = Color.red;
                    username.text = "Invalid Username";
                }
            }
        }
    }
}
