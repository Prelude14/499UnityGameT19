using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{
    public Button main; 

    public GameResultsData gameResultsData;

    private int myDamage; 

    private char result; 

    
    // Start is called before the first frame update
    void Start()
    {
        // main.onClick.AddListener(buttonOnclick);
        // string username = DBManager.username;

        // if (PlayerManager.isPlayerOne == true && PlayerManager.isPlayerTwo == false) //if I'm player ONE 
        // {
        //     int myDamage = gameResultsData.p1Damage.p1Damage;
        //     result = gameResultsData.p1Damage.p1Result;
        // }
        // else if (PlayerManager.isPlayerTwo == true && PlayerManager.isPlayerOne == false) //if I'm player TWO 
        // {
        //     int myDamage = gameResultsData.p1Damage.p2Damage;
        //     result = gameResultsData.p1Damage.p2Result;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buttonOnclick(){
        SceneManager.LoadScene("MainMenu");
        // Debug.Log("button clicked");
    }
      public void updateStats()
    {
        StartCoroutine(gameEnd());
    }

    IEnumerator gameEnd()
    {
        string username = DBManager.username;
        //make Form to take the user's input 
        WWWForm formE = new WWWForm();
        // formE.AddField("username", username);
        // formE.AddField("result", result);
        // formE.AddField("damageDealt", myDamage);

        //connect to url of our database's php file, PASS FORM TO URL
        using (WWW wwwE= new WWW("http://localhost/sqlconnect/gameOver.php", formE))
        {
             yield return wwwE; //tell Unity to yield running the rest of the game till it gets this info from the url

            if (wwwE.error == null)
            {
                //Error check what our PHP file returned, index 0 should be the first character, 0 means everything worked perfectly
                if (wwwE.text[0] == '0')
                {
                    Debug.Log("Stats successfully updated:  " + wwwE.text);
                }
                else
                {
                      Debug.Log("ERROR: Stats update failed:  " + wwwE.text);
                }
            }
        }
          
    }

}

