using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class updatePass : MonoBehaviour
{
    public Text password;
    public Text username;
    public GameObject newPasswordInput;
    public GameObject confirmPasswordUpdate;
    public Button updateButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
      public void CallUpdate()
    {
        StartCoroutine(updatepass());
    }

    IEnumerator updatepass()
    {
        //make Form to take the user's input 
        WWWForm formU = new WWWForm();
        formU.AddField("username", username.text);
        formU.AddField("password", password.text);

        //connect to url of our database's php file, PASS FORM TO URL
        using (WWW wwwU= new WWW("http://localhost/sqlconnect/update.php", formU))
        {
            yield return wwwU; //tell Unity to yield running the rest of the game till it gets this info from the url

            if (wwwU.error == null)
            {

                //Error check what our PHP file returned, index 0 should be the first character, 0 means everything worked perfectly
                if (wwwU.text[0] == '0')
                {
                    Debug.Log("Pass Updated Successfully.");
                    confirmPasswordUpdate.SetActive(true);
                    newPasswordInput.SetActive(false);
                }
                else
                {
                    Debug.Log("update FAILED. Error Code: " + wwwU.text);
                    password.color = Color.red;
                }
            }
        }
    }

    public void VerifyInputL() //button won't be clickable until input field has at least 8 characters
    {
        updateButton.interactable = (password.text.Length >= 10);
    }
}
