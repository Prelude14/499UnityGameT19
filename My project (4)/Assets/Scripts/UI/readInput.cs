using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class readInput : MonoBehaviour
{
    //======================================================================================================  LOGIN INPUT  =====================================================================================================
    //Need public string to store the user's input when it is entered into the login page's input fields
    public InputField username_email;
    public InputField user_pass;

    public Button loginButton;

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
            Debug.Log("User logged in successfully.");

            //Store user info in DBManager so Unity can display all the user info
            DBManager.username = username_email.text;
            //get datecreated from second index of output from login.php (decremented by tabs)
            DBManager.datecreated = www.text.Split('\t')[1];
            //get gamesplayed int from forms output (which is decremented by tabs) index 2, and need to convert from string to int so we can change it later
            DBManager.gamesplayed = int.Parse(www.text.Split('\t')[2]);
            DBManager.gameswon = int.Parse(www.text.Split('\t')[3]); //same for others
            DBManager.damagedealt = int.Parse(www.text.Split('\t')[4]);//get damage dealt

            if (DBManager.gamesplayed != 0)
            {
                DBManager.wlratio = DBManager.gameswon / DBManager.gamesplayed; //win loss ratio might need to be caluclated (not stored in table?)
            }
            else
            {
                DBManager.wlratio = 0; //win loss ratio might need to be caluclated (not stored in table?)
            }


            //NEED to add something to change menu display back to main menu when they log in******************
        }
        else
        {
            Debug.Log("User logged FAILED. Error Code: " + www.text);
        }
    }

    public void VerifyInputsL() //login button won't even be clickable until all three input fields have at least 10 characters in each
    {
        loginButton.interactable = (username_email.text.Length >= 10 && user_pass.text.Length >= 10);
    }




    //======================================================================================================  CREATE INPUT  =====================================================================================================
    public InputField c_username_email;
    public InputField c_user_pass;
    public InputField c_user_pass2;//confirm pass input field from create page

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
            Debug.Log("User created and logged in successfully.");

            //Store user info in DBManager so Unity can display all the user info
            DBManager.username = c_username_email.text;
            //get datecreated from second index of output from login.php (decremented by tabs)
            DBManager.datecreated = wwwC.text.Split('\t')[1];
            //get gamesplayed int from forms output (which is decremented by tabs) index 2, and need to convert from string to int so we can change it later
            DBManager.gamesplayed = int.Parse(wwwC.text.Split('\t')[2]);
            DBManager.gameswon = int.Parse(wwwC.text.Split('\t')[3]); //same for others
            DBManager.wlratio = int.Parse(wwwC.text.Split('\t')[4]); //win loss ratio might need to be caluclated (not stored in table?)
            //NEED to add something to change menu display back to main menu when they log in******************

        }
        else
        {
            Debug.Log("User create FAILED. Error Code: " + wwwC.text);
        }
    }
    public void VerifyInputsC() //create button won't even be clickable until all three input fields have at least 10 characters in each
    {
        createButton.interactable = (c_username_email.text.Length >= 10 && c_user_pass.text.Length >= 10 && c_user_pass2.text.Length >= 10);
    }


    /* //LOGIN INPUTS
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
     }*/

}
