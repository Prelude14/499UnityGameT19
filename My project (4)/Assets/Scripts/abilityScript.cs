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
                break;
            default:
                break;
        }
        // case 0:
        //ability 0: deal 2 damage to yourself

    }
    public void abilityListWhite(int id)
    {

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

    }
}
