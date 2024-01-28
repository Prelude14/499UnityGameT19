using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; //important --> use this for db method;

public class dbDisplay : MonoBehaviour
{
    public List<Card1> displayList = new List<Card1>();
    public int displayId;

    public int id;
    public int hp;
    public int pow;
    public int cost;

    public string cardName;
    public string txt;

    public Text nameText;
    public Text descriptionText;
    public Image artworkImage;

    public Text costText;
    public Text powText;
    public Text hpText;


    public bool cardBack;
    public static bool staticCardBack;

    public GameObject hand;
    public static int deckCount;


    // Start is called before the first frame update
    void Start()
    {
        deckCount = playerDeck.deckSize;
        displayList[0] = cardDatabase.cardList[displayId];
        this.id = displayList[0].id;

    }

    // Update is called once per frame
    void Update()
    {

        displayCard();
        hand = GameObject.Find("hand");
        //if this parent is the same as the hands parent 
        if (this.transform.parent == hand.transform.parent)
        {
            cardBack = false;
        }
        staticCardBack = cardBack;
        cloneDraw();


    }
    private void displayCard()
    {

        this.cardName = displayList[0].cardName;
        this.pow = displayList[0].pow;
        this.hp = displayList[0].hp;
        this.txt = displayList[0].txt;
        this.cost = displayList[0].cost;

        nameText.text = " " + this.cardName.ToString();
        descriptionText.text = " " + this.txt.ToString();
        costText.text = " " + this.cost.ToString();
        powText.text = " " + this.pow.ToString();
        hpText.text = " " + this.hp.ToString();


    }
    private void cloneDraw()
    {
        //clone cards for draw
        if (this.tag == "clone")
        {
            //
            displayList[0] = playerDeck.staticDeck[deckCount - 1];
            //
            deckCount -= 1;
            playerDeck.deckSize -= 1;
            cardBack = false;
            this.tag = "Untagged";
        }
    }
<<<<<<< Updated upstream:My project (4)/Assets/Scripts/dbDisplay.cs
=======

    public Color renderCardColour(int cardColour)
    {
        //need to check card1's colour int to see what colour to make the border of the card
        if (cardColour == 1)          // 1 == BLACK DECK
        {
            return Color.black;
        }
        else if (cardColour == 2)          // 2 == RED DECK
        {
            return Color.red;
        }
        else if (cardColour == 3)          // 3 == WHITE DECK
        {
            return Color.white;
        }
        else if (cardColour == 4)          // 4 == BLUE DECK
        {
            return Color.blue;
        }
        else
        {
            return Color.yellow; //placeholder for neutral
        }
    }








>>>>>>> Stashed changes:My project (4)/Assets/dbDisplay.cs
}
