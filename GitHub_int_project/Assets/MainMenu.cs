using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    //need public variable that will track if the user logged in or not
    public GameObject guestbackground;
    public GameObject usersMMbackground;
    public bool logged_In = false; //set player to guest as default

    //public DateTime dateCreated;
    //public DateTime dateCurrent; //gets current time and stores it 

    //Need public string to store the user's input when it is entered into the login page
    //public Text username;
    public string username_email = "Here the user can see the email associated with their account.";
    public string user_pass = "This will display how old the account is.";
    //public Text password;
    //PlayerPrefs.SetString("uname", 0);
    //PlayerPrefs.SetString("upass", 0);


    // Start is called before the first frame update
    public void ExitButton() {
        Application.Quit();
        Debug.Log("Game closed by button.");

    }

    public void resetPassButton()
    {
        Debug.Log("Reset Password Token sent.");

    }

    public void StartGame() {
        SceneManager.LoadScene("SampleScene");

    }

    public void LoginButt()
    {
        logged_In = true;
        PlayerPrefs.SetInt("logged_In", 1);

       // username_email = username;
        //user_pass = password;

        Debug.Log(username_email);
        Debug.Log(user_pass);
        Debug.Log(logged_In);
    }

   /* public void CreateAccButt()
    {
        dateCurrent = DateTime.Now; //gets current time and stores it
        PlayerPrefs.SetString("createDate", dateCurrent.toString());

        logged_In = true;
        PlayerPrefs.SetInt("logged_In", 1);

        username_email.text = PlayerPrefs.GetString("uname", 0);
        user_pass.text = PlayerPrefs.GetString("upass", 0);

        Debug.Log(logged_In);
    }
   */
    public void ActiveMainMenu()
    {
        if (logged_In == false)
        {
            //set guest main menu to show up
            guestbackground.SetActive(!logged_In);
        }
        else if (logged_In == true)
        {
            //set logged in main menu to show up from here on out
            usersMMbackground.SetActive(logged_In);
        }
    }
}
