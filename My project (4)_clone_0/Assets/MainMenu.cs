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

    //public variable to tell game scene what colour the user wants to play the game with.
    public playerDeck mainmenusPlayDeckScript;
    public string pickedColour = ""; //set empty to start


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

    // Start games with specific colour deck choosen by selecting a colour after clicking quick match
    
    //BLACK start button ======================================================         B  1
    public void StartBlackGame() {

        //set picked colour to black, in order for game scene to draw correct cards
        pickedColour = "BLACK";
        Debug.Log(pickedColour);

        //change variable inside playerDeck script so the other scene can change it
        playerDeck.playerColour = pickedColour;
        //now load scene
        SceneManager.LoadScene("SampleScene");

    }
    //RED start button ========================================================         R  2
    public void StartRedGame()
    {

        //set picked colour to red, in order for game scene to draw correct cards
        pickedColour = "R";
        Debug.Log(pickedColour);

        //change variable inside playerDeck script so the other scene can change it
        playerDeck.playerColour = pickedColour;
        //now load scene
        SceneManager.LoadScene("SampleScene");

    }
    //WHITE start button =======================================================         W  3 
    public void StartWhiteGame()
    {

        //set picked colour to white, in order for game scene to draw correct cards
        pickedColour = "W";
        Debug.Log(pickedColour);

        //change variable inside playerDeck script so the other scene can change it
        playerDeck.playerColour = pickedColour;
        //now load scene
        SceneManager.LoadScene("SampleScene");

    }
    //BLUE start button ========================================================         B   4
    public void StartBlueGame()
    {

        //set picked colour to blue, in order for game scene to draw correct cards
        pickedColour = "BLUE";
        Debug.Log(pickedColour);

        //change variable inside playerDeck script so the other scene can change it
        playerDeck.playerColour = pickedColour;
        //now load scene
        SceneManager.LoadScene("SampleScene");

    }

    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("TutorialScene"); // Make sure the scene name matches exactly
    }

    public void LoadBackToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu"); // Make sure the scene name matches exactly
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
            // userDisplayRESETmenu.text = DBManager.username;

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
