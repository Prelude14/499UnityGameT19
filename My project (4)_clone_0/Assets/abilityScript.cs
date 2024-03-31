using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;
using UnityEngine.UI;
using Mirror;
public class abilityScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int color;
    public int id = dbDisplay.staticID;
    public bool triggered = false;
    public PlayerManager PlayerManager;
    public static bool attacked = false;
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
        int damage;
        NetworkIdentity networkAttackIdentity = NetworkClient.connection.identity;
        PlayerManager = networkAttackIdentity.GetComponent<PlayerManager>();
        switch (id)
       {
            case 0:
            case 1:
                // playerHealth.HPStatic -= 2;
                // Debug.Log("reduce health by 2");
                damage = 2; 
                PlayerManager.CmdSendSelfDamage(damage, networkAttackIdentity);
                break;
            case 2:
            case 3:
                //destroy enemy mpinion not yet coded
                break;
            case 4:
            case 5:
                // playerHealth.HPStatic -= 3;
                // Debug.Log("reduce health by 3");
                damage = 3; 
                PlayerManager.CmdSendSelfDamage(damage, networkAttackIdentity);
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
                
                float currentHealth;
                int whosTurn = SharedVarManager.staticTurn; // 1 is p1 2 is p2
                if(whosTurn == 1){
                    //send p1.heal
                    currentHealth  = SharedVarManager.p1HP;
                    PlayerManager.CmdHealDamage(currentHealth, networkAttackIdentity);

                }else {
                    //send p2.heal
                    currentHealth = SharedVarManager.p2HP;
                    PlayerManager.CmdHealDamage(currentHealth, networkAttackIdentity);
                }
                break;
            case 10:
            case 11:
                //noenemy targetting yet
                // float difInHealth = 30 - playerHealth.HPStatic;
                // int drawAmount = (int)Math.Floor(difInHealth / 3);
                // playerDeck.staticAmount = drawAmount;
                // playerDeck.drawStatic = true;
                // Debug.Log("Drawn: " + drawAmount);
                // turnScript.cardsDrawn += drawAmount;
                if(SharedVarManager.staticTurn == 1){
                    //if its p1 who played this 
                    int diff = 30 - (int)SharedVarManager.p1HP;
                    PlayerManager.CmdDraw(diff, PlayerManager.clientDecks);
                }
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
        // PlayerManager.CmdDraw(3, PlayerManager.clientDecks);
        // Debug.Log("Drawn: " + 3);
        // triggered = true;

        // case 0:
        //ability 0: deal 2 damage to yourself
        int healed;
        NetworkIdentity networkAttackIdentity = NetworkClient.connection.identity;
        PlayerManager = networkAttackIdentity.GetComponent<PlayerManager>();
        int damage;
         switch(id){
        case 0:
        case 1:
            // playerHealth.HPStatic += 2;
            // Debug.Log("increase health by 2");
            // turnScript.damageHealed += 2;
            healed = 2; 
            PlayerManager.CmdSendHealing(healed, networkAttackIdentity);
            break;
        case 2:
        case 3:
            // playerHealth.HPStatic += 2;
            // Debug.Log("increase health by 2");
            // //deal 2 
            // enemyHealth.HPStatic -= 2;
            // Debug.Log("ping enemy 2");
            // turnScript.damageHealed += 2;
            //heal 2 deal 2
            healed = 2; 
            PlayerManager.CmdSendHealing(healed, networkAttackIdentity);
            damage = 2; 
            PlayerManager.CmdSendSelfDamage(damage, networkAttackIdentity);
            break;
        case 4:
        case 5:
            //draw card & heal 5
            PlayerManager.CmdDraw(1, PlayerManager.clientDecks);
                Debug.Log("Drawn: " + 1);
                triggered = true;
                //heal 5 dmg
                 healed = 5; 
            PlayerManager.CmdSendHealing(healed, networkAttackIdentity);
            break;
        case 6:
        case 7:
            //Heal target no target yet
            break;
        case 8:
        case 9:
            //copy card
            //Change?
            break;
        case 10:
        case 11:
               
            if(SharedVarManager.staticTurn == 1){
                damage = (int)SharedVarManager.p1TotalHeal; //deal damage equal to healing 
                //target damage
                PlayerManager.CmdPingDamage(damage, networkAttackIdentity);
            }
                break;
        default:
            break;
    }
    triggered = true;

    }
    public void abilityListBlue(int id)
    {
        // case 0:
        //ability 0: deal 2 damage to yourself
         NetworkIdentity networkAttackIdentity = NetworkClient.connection.identity;
        PlayerManager = networkAttackIdentity.GetComponent<PlayerManager>();
       
        GameObject enemyZone = GameObject.Find("oppPlayPanel");
        GameObject enemyHand = GameObject.Find("oppHand");
        GameObject yourHand = GameObject.Find("hand");
        int myHandCount = yourHand.transform.childCount;
        switch(id){
        case 0:
        case 1:
            //
           
            break;
        case 2:
        case 3:
            //
            //destroy random enemy
            
            int childCount = enemyZone.transform.childCount;
            int randomChild = Random.Range(0, childCount);
            NetworkServer.Destroy(transform.GetChild(randomChild).gameObject);
            break;
        case 4:
        case 5:
            //draw 1 reduce cost 1?
             PlayerManager.CmdDraw(1, PlayerManager.clientDecks);
            break;
        case 6:
        case 7:
            int enemyHandCount = enemyHand.transform.childCount;
            
            int toDraw = enemyHandCount - myHandCount;
            if(toDraw <= 0){
                return;
            }else {
                PlayerManager.CmdDraw(toDraw, PlayerManager.clientDecks);
            }
            break;
        case 8:
        case 9:
            //discard?
           int handplus1 = myHandCount + 1;
           foreach (Transform child in yourHand.transform) {
            NetworkServer.Destroy(child.gameObject);
            }
            PlayerManager.CmdDraw(handplus1, PlayerManager.clientDecks);
            break;
        case 10:
        case 11:
            if(SharedVarManager.staticTurn == 1){
                PlayerManager.CmdPingDamage(SharedVarManager.p1TotalDraw * 2, networkAttackIdentity);
            }else {
                PlayerManager.CmdPingDamage(SharedVarManager.p2TotalDraw * 2, networkAttackIdentity);
            }
            break;
        default:
            break;
    }
        
        triggered = true;
    }
    public void abilityListRed(int id)
    {
        NetworkIdentity networkAttackIdentity = NetworkClient.connection.identity;
        PlayerManager = networkAttackIdentity.GetComponent<PlayerManager>();
       GameObject playPanel = GameObject.Find("playPanel");
       int creatureCount = playPanel.transform.childCount;
       switch(id){
        case 0:
        case 1:
            if(attacked == true){
                // PlayerManager.CmdPingDamage(SharedVarManager.p2TotalDraw * 2, networkAttackIdentity);
                PlayerManager.CmdPingDamage(2, networkAttackIdentity);
                attacked = false;
            }
            break;
        case 2:
        case 3:
            //no ability
            //just a big stat stick
            break;
        case 4:
        case 5:
            
            PlayerManager.CmdDraw(creatureCount, PlayerManager.clientDecks);
            break;
        case 6:
        case 7:
            //mana restoration? Todo:
            int toRefill = playPanel.transform.childCount;
            break;
        case 8:
        case 9:
            
                // PlayerManager.CmdPingDamage(SharedVarManager.p2TotalDraw * 2, networkAttackIdentity);
                PlayerManager.CmdPingDamage(4, networkAttackIdentity);
            
            break;
        case 10:
        case 11:
            //
            int damage = creatureCount * 2;
              PlayerManager.CmdPingDamage(damage, networkAttackIdentity);
            break;
        default:
            break;
    }
        triggered = true;
    }
    public void abilityListColorless(int id)
    {   
        NetworkIdentity networkAttackIdentity = NetworkClient.connection.identity;
        PlayerManager = networkAttackIdentity.GetComponent<PlayerManager>();
        // case 0:
        //ability 0: deal 2 damage to yourself
        switch(id){
            case 0:
            case 1:
                triggered = true;
                
                break;
            case 2:
            case 3:
                PlayerManager.CmdDraw(1, PlayerManager.clientDecks);
                Debug.Log("Drawn: " + 1);
                triggered = true;
                
                break;
            case 4:
            case 5:
                //deal direct target damage to opponent using attack trigger?
                PlayerManager.CmdPingDamage(5, networkAttackIdentity);
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
       //destroyCardDupeClone
       GameObject cardDupe = GameObject.Find("cardDupe (clone)");
       Destroy(cardDupe);
    }
}


