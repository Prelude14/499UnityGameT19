using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class abilityScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int color = dbDisplay.staticCardColor;
    public int id = dbDisplay.staticID;
    public bool triggered = false;

    void Update()
    {
        color = dbDisplay.staticCardColor;
        id = dbDisplay.staticID;
        if (triggered == false && dbDisplay.staticSummoned == true)
        {
            Debug.Log("Ability triggered init");
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
            
        }
    }
    public void abilityListBlack(int id)
    {
        Debug.Log("black trigger id: " + id);
        switch (id)
        {
            case 0:
            case 1:
                playerHealth.HPStatic -= 2;
                Debug.Log("reduce health by 2");
                
                break;
            case 2:
            case 3:
                //destroy enemy mpinion not yet coded
                break;
            case 4:
            case 5:
                playerHealth.HPStatic -= 3;
                Debug.Log("reduce health by 3");
                break;
            case 6:
            case 7:
                //target enemy or minion targetting yet
                break;
            case 8:
            case 9:
                //turn start health
                float startHealth = playerHealth.turnStartHealth;
                float healthDiff = playerHealth.turnStartHealth - playerHealth.HPStatic;
                if (healthDiff >= 0)
                {
                    playerHealth.HPStatic += healthDiff;
                    turnScript.damageHealed += (int)healthDiff;
                }
                else
                {
                    playerHealth.HPStatic += 0;
                }
                Debug.Log("Healed for diff of " + healthDiff);
                break;
            case 10:
            case 11:
                //noenemy targetting yet
                float difInHealth = 30 - playerHealth.HPStatic;
                int drawAmount = (int)Math.Floor(difInHealth / 3);
                playerDeck.staticAmount = drawAmount;
                playerDeck.drawStatic = true;
                Debug.Log("Drawn: " + drawAmount);
                turnScript.cardsDrawn += drawAmount;
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
    switch(id){
        case 0:
        case 1:
            playerHealth.HPStatic += 2;
            Debug.Log("increase health by 2");
            turnScript.damageHealed += 2;
            break;
        case 2:
        case 3:
            playerHealth.HPStatic += 2;
            Debug.Log("increase health by 2");
            //deal 2 
            enemyHealth.HPStatic -= 2;
            Debug.Log("ping enemy 2");
            turnScript.damageHealed += 2;
            break;
        case 4:
        case 5:
            //draw card
            playerDeck.staticAmount = 1;
            playerDeck.drawStatic = true;
            //heal equal to card cost
            playerHealth.HPStatic += 5;
            turnScript.damageHealed += 5;
            turnScript.cardsDrawn += 1;
            break;
        case 6:
        case 7:
            //Heal target no target yet
            break;
        case 8:
        case 9:

            //copy card
            break;
        case 10:
        case 11:
                //noenemy targetting yet
                enemyHealth.HPStatic -= turnScript.damageHealed;
                break;
        default:
            break;
    }
    triggered = true;
    }
    public void abilityListBlue(int id)
    {

        switch(id){
        case 0:
        case 1:
            //
            if(turnScript.cardsDrawn > 0){
                //targetminion
            }
            break;
        case 2:
        case 3:
            //
            //targetminion
            break;
        case 4:
        case 5:
             playerDeck.staticAmount = 1;
            playerDeck.drawStatic = true;
            turnScript.actionPoints++;
            turnScript.cardsDrawn += 1;
            break;
        case 6:
        case 7:
            //Opponent hand size
            GameObject gameObj = GameObject.Find("enemyHand");
            int enemyHand = gameObj.transform.childCount;
            GameObject hand = GameObject.Find("hand");
            int myHand = hand.transform.childCount;
            
            if(enemyHand - myHand > 0) {
                playerDeck.staticAmount = enemyHand - myHand;
                playerDeck.drawStatic = true;
                turnScript.cardsDrawn += enemyHand - myHand;
            }else {
                //do nth
            }
            break;
        case 8:
        case 9:
            //discard?
           
            break;
        case 10:
        case 11:
            //
            if(turnScript.cardsDrawn > 0){
                enemyHealth.HPStatic -= turnScript.cardsDrawn;
            }

            break;
        default:
            break;
    }
        triggered = true;
    }
    public void abilityListRed(int id)
    {

        GameObject go = GameObject.Find("playPanel");
        int creatureCount =0;
        creatureCount = go.transform.childCount;
        // case 0:
        //ability 0: deal 2 damage to yourself

        switch(id){
        case 0:
        case 1:
            if(dbDisplay.hasAttacked == true){
                enemyHealth.HPStatic -= 2;
                triggered = true;
            }else{
                break;
            }
            break;
        case 2:
        case 3:
            if(dbDisplay.hasAttacked == true){
                int healAmount = dbDisplay.staticPow;
                playerHealth.HPStatic += healAmount;
                turnScript.damageHealed += healAmount;
                triggered = true;
            }else{
                break;
            }
            break;
        case 4:
        case 5:
 
            playerDeck.staticAmount = creatureCount;
            playerDeck.drawStatic = true;
            triggered = true;
            turnScript.cardsDrawn += creatureCount;
            break;
        case 6:
        case 7:
         
            if(creatureCount > turnScript.staticMaxMana){
                turnScript.currentMana = turnScript.staticMaxMana;
            }else if (creatureCount + turnScript.currentMana > turnScript.staticMaxMana){
                turnScript.currentMana = turnScript.staticMaxMana;
            }else{
                turnScript.currentMana += creatureCount;
            }
            triggered = true;
            break;
        case 8:
        case 9:
            enemyHealth.HPStatic -= 4;
            triggered = true;
            break;
        case 10:
        case 11:
            enemyHealth.HPStatic -= creatureCount;
            triggered = true;
            break;
        default:
            break;
    }

    }
    public void abilityListColorless(int id)
    {

        // case 0:
        //ability 0: deal 2 damage to yourself

    }
}
