using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror; //need this script to be outside of script folder in order for it to use mirror for some reason 


public class playerHealth : NetworkBehaviour
{
    // Start is called before the first frame update
    public static float maxHp;
    public static float HPStatic;
    public float hp;
    public Image health;
    public Text hpText;
    public float fillAmount;

    //need access to player manager script that is unique to each client
    public PlayerManager PlayerManager;
    public float myHealth = 31; //default val for my health inside setUpMyHealth()
    public static int turnStartHealth;


    void Start()
    {
        maxHp = 30;
        HPStatic = 30;

    }

    // Update is called once per frame
    void Update()
    {
        SetUpMyHealth(); //find out how much health to give the my player -- assigns hp to match server's values

        //Fill health bar to this much
        if (hp < 0)
        {
            hp = 0;
        }
        fillAmount = fillHealth(hp);
        health.fillAmount = fillAmount; //percentage of fill amount
        if (hp >= maxHp)
        {
            hp = maxHp;
        }
        hpText.text = hp.ToString();

    }
    public void SetUpMyHealth()
    {
        //find sharedvar game object in scene at runtime, CHECK for player's health
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");
        SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();

        //locate the PlayerManager in the Client, need to check if it is player 1 or 2
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();

        if (PlayerManager.isPlayerOne == true && PlayerManager.isPlayerTwo == false) //if I'm player ONE 
        {
            myHealth = sharedVarManager.p1Health; //set my health to equal that in the sharedVarManager script
        }
        else if (PlayerManager.isPlayerTwo == true && PlayerManager.isPlayerOne == false) //if I'm player TWO 
        {
            myHealth = sharedVarManager.p2Health; //set my health to equal that in the sharedVarManager script
        }
        else
        {
            myHealth = hp; //if players aren't assigned yet, just assign it to default 30 until update works
        }
        hp = myHealth; //update the playerHealth script's hp to actually match that of SharedVarManager(the server's value) 
    }
    public float getHealth()
    {
        //return current hp
        return hp;
    }
    public float fillHealth(float x)
    {
        return x / maxHp;
    }
    public void setHealth(float x)
    {
        this.hp = x;
    }
    public float getFillAmount()
    {
        return fillAmount;
    }
}
