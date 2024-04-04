using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutTurnScript : MonoBehaviour
{
    public static bool isMyTurn;
    public int myTurn;
    public int isTheirTurn;
    public Text turnText;
    public bool disable;
    public static int totalSummons;

    public int maxMana;
    public static int staticMaxMana;
    public static int actionPoints;

    public int actionPointsTotal;
    public static int currentMana;
    public Text manaText;

    public static bool turnStart;
    public static int damageHealed = 0;

    public static int turnCount = 0;
    public GameObject playArrows;
    public GameObject attackArrows;
    public static int cardsDrawn;
    public Text actionText;

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
        //actionPointsTotal = 1;
        //actionPoints = 1;
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
        //actionText.text = actionPoints + "/" + actionPointsTotal;
        if (turnCount == 0)
        {
            playArrows.SetActive(true);

        }
        else
        {
            playArrows.SetActive(false);
            attackArrows.SetActive(false);
        }

        if (totalSummons == 1 && disable == false)
        {
            attackArrows.SetActive(true);
        }
        else
        {
            attackArrows.SetActive(false);
        }
        staticMaxMana = maxMana;
    }
    public void endTurn()
    {
        isMyTurn = false;
        isTheirTurn = 1;
        turnCount++;
        playArrows.SetActive(false);
        attackArrows.SetActive(false);
        disable = true;

    }
    public void endOpponentTurn()
    {
        isMyTurn = true;
        myTurn = 1;
        maxMana += 1;
        currentMana = maxMana;
        turnStart = true;
        tutDbDisplay.hasAttacked = false;
        if (totalSummons < 1)
        {
            disable = false;
        }
        //actionUpdate();
        tutPlayerHealth.turnStartHealth = tutPlayerHealth.HPStatic;
        cardsDrawn = 0;
    }

    public void actionUpdate()
    {
        if (turnCount % 2 != 0 && turnCount <= 10)
        {
            //actionPointsTotal++; //adds total action points
        }
        //actionPoints = actionPointsTotal;
    }

}
