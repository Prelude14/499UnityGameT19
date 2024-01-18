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
    public GameObject playZone;
    public GameObject currentZone;
    public static int deckCount;

    public bool canBeSummoned;
    public bool isSummoned;



    // Start is called before the first frame update
    void Start()
    {
        deckCount = playerDeck.deckSize;
        displayList[0] = cardDatabase.cardList[displayId];
        this.id = displayList[0].id;

        canBeSummoned = false;
        isSummoned = false;

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

        playZone = GameObject.Find("playPanel");
        currentZone = this.transform.parent.gameObject;

        if (turnScript.currentMana >= cost && isSummoned == false)

        {
            canBeSummoned = true;
            Debug.Log(cardName + " Is now playable");
            if (currentZone == hand)
            {
                Debug.Log(cardName + " Is now playable");
            }
        }
        else
        {
            canBeSummoned = false;
        }

        if (canBeSummoned)
        {
            dragScript.isDraggable = true;
            Debug.Log(cardName + " is now dragable");
        }
        else
        {
            dragScript.isDraggable = false;
        }


        if (isSummoned == false && currentZone == playZone)
        {
            isSummoned = true;
            Debug.Log(cardName + " Summoned sucess | Cost: " + this.cost + " | Current zone: " + currentZone + " | play zone: " + playZone + " | Is summoned? " + isSummoned);

            turnScript.currentMana = turnScript.currentMana - this.cost;
            Debug.Log("Mana left: " + turnScript.currentMana);
        }


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
}
