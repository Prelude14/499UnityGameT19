using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror; //need this script to be outside of script folder in order for it to use mirror for some reason 

public class enemyHealth : NetworkBehaviour
{
    // Start is called before the first frame update
    public static float maxHp;
    public static float HPStatic;
    public float hp; // actual health point count
    public Image health;
    public Text hpText; //display enemy health
    public GameObject glow; //orange glow around enemy's health 

    //need access to player manager script that is unique to each client
    public PlayerManager PlayerManager;
    public float opponentsHealth = 31; //default val for opponent's health inside setUpOppHealth()

    void Start()
    {
        maxHp = 30;
        HPStatic = 30;
    }

    // Update is called once per frame
    void Update()
    {
        glow.SetActive(false); //keep glow off

        SetUpOppHealth(); //find out how much health to give the opponent -- assigns HPStatic to match server's values
        
        //Fill health bar to this much
        hp = HPStatic; //this should be assigned by the server in setUpOppHealth
        
        health.fillAmount = hp / maxHp; //percentage of fill amount
        if (hp >= maxHp)
        {
            hp = maxHp;
        }
        hpText.text = hp.ToString();
        fillHealth();
        if (dbDisplay.attackDragging == true)
        {
            glow.SetActive(true);
        }
        else
        {
            glow.SetActive(false);
        }

    }
    public void SetUpOppHealth()
    {
        //find sharedvar game object in scene at runtime, CHECK for player's health
        GameObject sharedVarManagerObj = GameObject.Find("SharedVarManager");
        SharedVarManager sharedVarManager = sharedVarManagerObj.GetComponent<SharedVarManager>();

        //locate the PlayerManager in the Client, need to check if it is player 1 or 2
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();

        if (PlayerManager.isPlayerOne == true && PlayerManager.isPlayerTwo == false) //if I'm player ONE 
        {
            opponentsHealth = sharedVarManager.p2Health; //set opp's health to equal that in the sharedVarManager script
        }
        else if (PlayerManager.isPlayerTwo == true && PlayerManager.isPlayerOne == false) //if I'm player TWO 
        {
            opponentsHealth = sharedVarManager.p1Health; //set opp's health to equal that in the sharedVarManager script
        }
        else
        {
            opponentsHealth = HPStatic; //if players aren't assigned yet, just assign it to default 30 until update works
        }
        HPStatic = opponentsHealth; //update the opponentHealth script's hp to actually match that of SharedVarManager(the server's value) 
    }
    public float getHealth()
    {
        //return current hp
        return hp;
    }
    public void fillHealth()
    {

    }
}
