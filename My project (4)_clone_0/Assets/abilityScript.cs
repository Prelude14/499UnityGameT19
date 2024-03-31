using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;
using Mirror;
public class abilityScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int color;
    public int id = dbDisplay.staticID;
    public bool triggered = false;
    public PlayerManager PlayerManager;
    NetworkIdentity networkIdentity;
    void Update()
    {
        networkIdentity = NetworkClient.connection.identity;
         PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        color = dbDisplay.staticColour;
        id = dbDisplay.staticID;
        if (triggered == false && dbDisplay.staticSummoned == true)
        {
            Debug.Log("Ability triggered init " + color);

            switch (color)
            {
                case 1:
                    abilityListBlack(id);
                    break;
                case 2:
                    abilityListRed(id);
                    break;
                case 3:
                    abilityListWhite(id);
                    break;
                case 4:
                    abilityListBlack(id);
                    break;
                case 5:
                    abilityListColorless(id);
                    break;
                default:
                    break;
            }
            triggered = true;
        }
    }
    public void abilityListBlack(int id)
    {
        Debug.Log("black trigger id: " + id);
 
        switch (id)
       {
            case 0:
            case 1:
                // playerHealth.HPStatic -= 2;
                // Debug.Log("reduce health by 2");
                
                break;
            case 2:
            case 3:
                //destroy enemy mpinion not yet coded
                break;
            case 4:
            case 5:
                // playerHealth.HPStatic -= 3;
                // Debug.Log("reduce health by 3");
                break;
            case 6:
            case 7:
                //target enemy or minion targetting yet
                break;
            case 8:
            case 9:
                //turn start health
                // float startHealth = playerHealth.turnStartHealth;
                // float healthDiff = playerHealth.turnStartHealth - playerHealth.HPStatic;
                // if (healthDiff >= 0)
                // {
                //     playerHealth.HPStatic += healthDiff;
                //     turnScript.damageHealed += (int)healthDiff;
                // }
                // else
                // {
                //     playerHealth.HPStatic += 0;
                // }
                // Debug.Log("Healed for diff of " + healthDiff);
                // break;
            case 10:
            case 11:
                //noenemy targetting yet
                // float difInHealth = 30 - playerHealth.HPStatic;
                // int drawAmount = (int)Math.Floor(difInHealth / 3);
                // playerDeck.staticAmount = drawAmount;
                // playerDeck.drawStatic = true;
                // Debug.Log("Drawn: " + drawAmount);
                // turnScript.cardsDrawn += drawAmount;
                break;
            default:
                break;


        }
        // case 0:
        //ability 0: deal 2 damage to yourself
        triggered = true;

    }
    public void abilityListWhite(int id)
    {
        
        Debug.Log("TRIGGER WHITE ABILTIY");
        PlayerManager.CmdDraw(3, PlayerManager.clientDecks);
        Debug.Log("Drawn: " + 3);
        triggered = true;

        // case 0:
        //ability 0: deal 2 damage to yourself

    }
    public void abilityListBlue(int id)
    {

        // case 0:
        //ability 0: deal 2 damage to yourself

    }
    public void abilityListRed(int id)
    {

        // case 0:
        //ability 0: deal 2 damage to yourself

    }
    public void abilityListColorless(int id)
    {

        // case 0:
        //ability 0: deal 2 damage to yourself
        switch(id){
            case 0:
            case 1:
                triggered = true;
                boardWipe();
                break;
            case 2:
            case 3:
                PlayerManager.CmdDraw(1, PlayerManager.clientDecks);
                Debug.Log("Drawn: " + 1);
                triggered = true;
                boardWipe();
                break;
            case 4:
            case 5:
                //deal direct target damage to opponent using attack trigger?
                boardWipe();
                triggered = true;
                break;
            case 6:
            case 7:
                Debug.Log("Triggered board wipe");
                boardWipe();
                triggered = true;
                break;
            default:
                break;
        }

    }

    public void boardWipe() {
        GameObject playZone = GameObject.Find("playPanel");
        GameObject enemyZone = GameObject.Find("oppPlayPanel");
        //preparing to blow up the board
        foreach (Transform child in playZone.transform) {
            NetworkServer.Destroy(child.gameObject);
        }
        foreach (Transform child in enemyZone.transform) {
            NetworkServer.Destroy(child.gameObject);
        }
    }
}


