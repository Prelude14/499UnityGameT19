using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class turnScript : MonoBehaviour
{
    public bool isMyTurn;
    public int myTurn;
    public int isTheirTurn;
    public Text turnText;

    public int maxMana;

    public int currentMana;
    public Text manaText;


    // Start is called before the first frame update
    void Start()
    {
        //game start your turn set mana to 1
        isMyTurn = true;
        myTurn = 1;
        isTheirTurn = 0;

        maxMana = 1;
        currentMana = 1;


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

    }
    public void endTurn()
    {
        isMyTurn = false;
        isTheirTurn = 1;

    }
    public void endOpponentTurn()
    {
        isMyTurn = true;
        myTurn = 1;
        maxMana += 1;
        currentMana = maxMana;

    }
}
