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

    //public display of user's stats for account info page as well as stats page
    public Text userEmail;
    public Text userDateCreated;
    public Text userGamesPlayed;
    public Text userGamesWon;
    public Text userWLRatio;
    public Text userDamageDealt;


    /*// Start is called before the first frame update
    public void ExitButton() {
        Application.Quit();
        Debug.Log("Game closed by button.");

    }*/

    // Start is called before the first frame update
    public void StartGame() {
        SceneManager.LoadScene("SampleScene");

    }

    public void ActiveMainMenu()
    {
        if (!DBManager.LoggedIn) //if not logged in display guest menu
        {
            //set guest main menu to show up
            guestbackground.SetActive(true);
            
        }
        else if (DBManager.LoggedIn) //if LOGGED IN, display user menu and minimize either the create or login mennu the user was on when they clicked login or create
        {
            //set logged in main menu to show up from here on out
            guestbackground.SetActive(false); //turn off guest menu
            usersMMbackground.SetActive(true); //set user menu active
            //minimize the login or create menus upon successful login
            createandloginmenu.SetActive(false);
            loginmenu.SetActive(false);

            //display user email in top right of screen
            userDisplay.text = "Welcome, " + DBManager.username;

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
