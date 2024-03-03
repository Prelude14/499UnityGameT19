using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class readInput : MonoBehaviour
{
    //======================================================================================================  LOGIN INPUT  =====================================================================================================
    //Need public string to store the user's input when it is entered into the login page's input fields
    public InputField username_email;
    public InputField user_pass;
    private bool loginFinished = false; //for test purposes
    public Button loginButton;
    public GameObject loginsButton;
    public GameObject confirmLogin;
    public GameObject createsButton;
    public GameObject confirmCreate;
    public GameObject createFailed;
    public Text email;
    public Text pass;
    public TMPro.TextMeshProUGUI loginText;

    public void CallLogin()
    {
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        //make Form to take the user's input 
        WWWForm form = new WWWForm();
        form.AddField("username", username_email.text);
        form.AddField("password", user_pass.text);

        //connect to url of our database's php file, PASS FORM TO URL
        WWW www = new WWW("http://localhost/sqlconnect/login.php", form);
        yield return www; //tell Unity to yield running the rest of the game till it gets this info from the url

        //Error check what our PHP file returned, index 0 should be the first character, 0 means everything worked perfectly
        if (www.text[0] == '0')
        {

            Debug.Log("User logged in successfully. Account Values: " + www.text);

            //Store user info in DBManager so Unity can display all the user info
            DBManager.username = username_email.text;
            //get datecreated from second index of output from login.php (decremented by tabs)
            DBManager.datecreated = www.text.Split('\t')[1];
            //get gamesplayed int from forms output (which is decremented by tabs) index 2, and need to convert from string to int so we can change it later
            DBManager.gamesplayed = int.Parse(www.text.Split('\t')[2]);
            DBManager.gameswon = int.Parse(www.text.Split('\t')[3]); //same for others
            DBManager.damagedealt = int.Parse(www.text.Split('\t')[4]);//get damage dealt

            loginsButton.SetActive(false);
            confirmLogin.SetActive(true);

            if (DBManager.gamesplayed != 0)
            {
                DBManager.wlratio = DBManager.gameswon / DBManager.gamesplayed; //win loss ratio might need to be caluclated (not stored in table?)
            }
            else
            {
                DBManager.wlratio = 0; //win loss ratio might need to be caluclated (not stored in table?)
            }
        }
        else
        {
            Debug.Log("User logged FAILED. Error Code: " + www.text);
            email.color = Color.red;
            pass.color = Color.red;
            loginText.text = "incorrect email or password";
        }
        //for test purposes:
        loginFinished = true;
    }

    public void VerifyInputsL() //login button won't even be clickable until each input field has at least 10 characters in each
    {
        loginButton.interactable = (username_email.text.Length >= 10 && user_pass.text.Length >= 10);
    }
    // Method to check if the login coroutine has completed FOR TESTING
    public bool IsLoginCoroutineCompleted()
    {
        return loginFinished;
    }



    //======================================================================================================  CREATE INPUT  =====================================================================================================
    public InputField c_username_email;
    public InputField c_user_pass;
    public InputField c_user_pass2;//confirm pass input field from create page
    private bool createFinished = false; //for test purposes

    public Button createButton; //create page button

    public void CallCreate()
    {
        StartCoroutine(Create());
    }

    IEnumerator Create()
    {
        //make Form to take the user's input 
        WWWForm formC = new WWWForm();
        formC.AddField("username", c_username_email.text);
        formC.AddField("password", c_user_pass.text);
        formC.AddField("password2", c_user_pass2.text); //get second pass from second pass field

        //connect to url of our database's php file, PASS FORM TO URL
        WWW wwwC = new WWW("http://localhost/sqlconnect/create.php", formC);
        yield return wwwC; //tell Unity to yield running the rest of the game till it gets this info from the url

        //Error check what our PHP file returned
        if (wwwC.text[0] == '0')
        {
            Debug.Log("User created and logged in successfully. Account Values: " + wwwC.text);

            //Store user info in DBManager so Unity can display all the user info
            DBManager.username = c_username_email.text;
            //get datecreated from second index of output from login.php (decremented by tabs)
            DBManager.datecreated = wwwC.text.Split('\t')[1];
            //get gamesplayed int from forms output (which is decremented by tabs) index 2, and need to convert from string to int so we can change it later
            DBManager.gamesplayed = int.Parse(wwwC.text.Split('\t')[2]);
            DBManager.gameswon = int.Parse(wwwC.text.Split('\t')[3]); //same for others
            DBManager.wlratio = int.Parse(wwwC.text.Split('\t')[4]); //win loss ratio might need to be caluclated (not stored in table?)

            createsButton.SetActive(false);
            confirmCreate.SetActive(true);
        }
        else
        {
            Debug.Log("User create FAILED. Error Code: " + wwwC.text);
            createFailed.SetActive(true);
            createsButton.SetActive(false);
        }
        //for test purposes:
        createFinished = true;
    }
    public void VerifyInputsC() //create button won't even be clickable until all three input fields have at least 10 characters in each, AND passwords match
    {
        bool longEnoughInputs = c_username_email.text.Length >= 10 && c_user_pass.text.Length >= 10 && c_user_pass2.text.Length >= 10; //true means all the inputs are long enough
        bool matchingPasswords = string.Equals(c_user_pass.text, c_user_pass2.text); //true means the passes match, false means they are different.
        createButton.interactable = (longEnoughInputs && matchingPasswords);
    }
    // Method to check if the create coroutine has completed FOR TESTING
    public bool IsCreateCoroutineCompleted()
    {
        return createFinished;
    }


}
