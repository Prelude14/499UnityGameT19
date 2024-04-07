using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class DBManager
{
    //======================================================================================================  LOGIN INPUT  =====================================================================================================
    //Need public string to store the user's input when it is entered into the login page's input fields
    public static string username;
    public static string datecreated; //might need to change to a date type
    public static int gamesplayed;
    public static int gameswon;
    public static int wlratio;
    public static int damagedealt;

    //track if user logged in successfully
    public static bool LoggedIn
    {
        //if the username has not been set yet (readInput.cs hasn't succeeded in logging in or creating an account), then return false, but if they have, then return true
        get { return username != null; }
    } //THIS WILL REPLACE CHECK LOGGED IN IN THE MAIMMENU SCRIPT************

    //if user logs out we change the username back to null
    public static void LogOut()
    {
        username = null;
    }

    
}