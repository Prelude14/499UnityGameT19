using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class turnScript : MonoBehaviour
{
    public static bool isMyTurn;
    public int myTurn;
    public int isTheirTurn;
    public Text turnText;
    public bool disable;
    public static int totalSummons;

    public int maxMana;

    public static int currentMana;
    public Text manaText;

    public static bool turnStart;

    public static int turnCount = 0;
    public GameObject playArrows;
    public GameObject attackArrows;

    // Start is called before the first frame update
    void Start()
    {
        //game start your turn set mana to 1
        isMyTurn = true;
        myTurn = 1;
        isTheirTurn = 0;

        maxMana = 1;
        currentMana = 1;
        turnStart = false;
        disable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMyTurn == true)
        {
            turnText.text = "Your turn";
        }
        else
        {
            turnText.text = "Opponent's turn";
        }
        manaText.text = currentMana + "/" + maxMana;

        if (turnCount == 0)
        {
            playArrows.active = true;

        }
        else
        {
            playArrows.active = false;
            attackArrows.active = false;
        }

        if (totalSummons == 1 && disable == false)
        {
            attackArrows.active = true;
        }
        else
        {
            attackArrows.active = false;
        }

    }
    public void endTurn()
    {
        isMyTurn = false;
        isTheirTurn = 1;
        turnCount++;
        playArrows.active = false;
        attackArrows.active = false;
        disable = true;
    }
    public void endOpponentTurn()
    {
        isMyTurn = true;
        myTurn = 1;
        maxMana += 1;
        currentMana = maxMana;
        turnStart = true;
        dbDisplay.hasAttacked = false;
        if (totalSummons < 1)
        {
            disable = false;
        }
    }


}
