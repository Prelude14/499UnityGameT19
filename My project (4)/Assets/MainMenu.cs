using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //need public variable that will track if the user logged in or not
    public GameObject guestbackground;
    public GameObject usersMMbackground;
    public GameObject loginmenu;
    public GameObject createandloginmenu;

    //public display of user's email in middle of menus
    public Text userDisplay;
    //AND FOR PASS RESET MENU
    public Text userDisplayRESETmenu;

    //public display of user's stats for account info page as well as stats page
    public Text userEmail;
    public Text userDateCreated;
    public Text userGamesPlayed;
    public Text userGamesWon;
    public Text userWLRatio;
    public Text userDamageDealt;


    // logout button of the user menu background calls this to log out the user and go back to the guest menu background
    public void LogOutButton()
    {
        DBManager.username = null; //set dmanager's username object to null, which should trigger its boolean variable "LoggedIn" to now return false

        //set guest main menu to show up
        guestbackground.SetActive(true);
        usersMMbackground.SetActive(false); //DEACTIVATE USER MENU IF LOGGED OUT
        //userDisplay.SetActive(false);//get rid of user specific name display, don't need this, the userbackground being deactivated gets rid of it for us.

        Debug.Log("User logged out by button.");

    }

    // Start is called before the first frame update
    public void StartGame() {
        SceneManager.LoadScene("SampleScene");

    }


    // login menu background or the create user and login menu background calls this to display the user menu background and minimize the guest menu background when the user clicks login or create
    public void ActiveMainMenu()
    {
        if (!DBManager.LoggedIn) //if not logged in display guest menu by default
        {
            //set guest main menu to show up
            guestbackground.SetActive(true);
            
        }
        else if (DBManager.LoggedIn) //if LOGGED IN, display user menu and minimize either the create or login menu the user was on when they clicked login or create
        {
            //set logged in main menu to show up from here on out
            guestbackground.SetActive(false); //turn off guest menu
            usersMMbackground.SetActive(true); //set user menu active
            //minimize the login or create menus upon successful login
            createandloginmenu.SetActive(false);
            loginmenu.SetActive(false);

            //display user email in top right of screen
            userDisplay.text = "Welcome, " + DBManager.username;
            //set reset pass menu's email text to equal the username as well
            userDisplayRESETmenu.text = DBManager.username;

            //assign values from DBManager to all of the account info and stat page's text items
            userEmail.text = "" + DBManager.username;
            userDateCreated.text = "" + DBManager.datecreated;
            userGamesPlayed.text = "" + DBManager.gamesplayed;
            userGamesWon.text = "" + DBManager.gameswon;
            userWLRatio.text = "" + DBManager.wlratio;
            userDamageDealt.text = "" + DBManager.damagedealt;
        }
    }
}
