using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class updatePass : MonoBehaviour
{
    public InputField r_userpassField;
    public InputField r_userpass2Field;
    public Text password;
    public Text password2;
    public Text username;
    public GameObject newPasswordInput;
    public GameObject confirmPasswordUpdate;
    public Button updateButton;
    public Text u_errorMessage;
    public GameObject u_errorMessag_GO;
    public GameObject cross;
    public GameObject cross1;
    public GameObject check;
    public GameObject check1;


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
                    password2.color = Color.red;

                   if (wwwU.text[0] == '5'){
                        u_errorMessage.text = "ERROR: There is a problem with your account. Please try again later!";
                    }
                    else if (wwwU.text[0] == '9'){
                        u_errorMessage.text = "ERROR: This password has already been updated.";
                    }
                    else if (wwwU.text[0] == '1'){
                        u_errorMessage.text = "ERROR: Could not change your password at the moment. Please try again later!";
                    }

                    u_errorMessag_GO.SetActive(true);
                }
            }
        }
    }

        public void VerifyInputsUpdate() //create button won't even be clickable until all three input fields have at least 10 characters in each, AND passwords match
    {
        bool longEnoughInputs = true;//password.text.Length >= 10 && password2.text.Length >= 10; //true means all the inputs are long enough
        bool matchingPasswords = string.Equals(r_userpassField.text, r_userpass2Field.text); //true means the passes match, false means they are different.
        updateButton.interactable = (longEnoughInputs && matchingPasswords);
        check.SetActive(longEnoughInputs && matchingPasswords); check1.SetActive(longEnoughInputs && matchingPasswords);
        cross.SetActive(!(longEnoughInputs && matchingPasswords)); cross1.SetActive(!(longEnoughInputs && matchingPasswords));
    }
}

