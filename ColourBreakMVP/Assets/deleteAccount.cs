using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class deleteAccount : MonoBehaviour
{
    public void CallDelete()
    {
        StartCoroutine(delete());
    }

    IEnumerator delete()
    {
        //make Form to take the user's input 
        WWWForm formD = new WWWForm();
        string username = DBManager.username;
        formD.AddField("username", username);

        //connect to url of our database's php file, PASS FORM TO URL
        using (WWW wwwD= new WWW("http://localhost/sqlconnect/delete.php", formD))
        {
            yield return wwwD; //tell Unity to yield running the rest of the game till it gets this info from the url

            if (wwwD.error == null)
            {

                //Error check what our PHP file returned, index 0 should be the first character, 0 means everything worked perfectly
                if (wwwD.text[0] == '0')
                {
                    Debug.Log("Account deleted Successfully.");
                }
                else
                {
                    Debug.Log("Account delete failed. Error Code: " + wwwD.text);
                }
            }
        }

    }
}