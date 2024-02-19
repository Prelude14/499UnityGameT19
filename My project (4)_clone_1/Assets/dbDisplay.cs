using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; //important --> use this for db method;

public class dbDisplay : MonoBehaviour
{
    public List<Card1> displayList = new List<Card1>();
    public int displayId;

    public int colour; //for each card's colour
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

    public GameObject cardInHand;
    public GameObject hand;
    public GameObject playZone;
    public GameObject currentZone;
    public static int deckCount;

    public bool canBeSummoned;
    public bool isSummoned;

    public bool canAttack;
    public bool cantAttack;
    public static bool hasAttacked;

    public bool targeting;
    public bool targetingEnemy;
    public static bool staticTargeting;
    public static bool staticTargetingEnemy;
    public bool onlyThisCardAttack;
    public GameObject Target;
    public GameObject Enemy;
    public static bool staticSummoned;
    public bool currentlyDraggable;

    public static GameObject currentLoc;
    public static GameObject pz;

    // Start is called before the first frame update (Find each card's info to be displayed)
    void Start()
    {
        Debug.Log("dbDisplay started...");
        deckCount = playerDeck.deckSize;

        
        displayList[0] = playerDeck.staticDeck[displayId]; //get card from playerDeck's static deck PLACEHOLDER********

        this.id = displayList[0].id;

        isSummoned = false;
        Enemy = GameObject.Find("enemyHealth");
        canAttack = false;
        targeting = false;
        targetingEnemy = false;
        canBeSummoned = true;
    }

    // Update is called once per frame
    void Update()
    {
        staticSummoned = isSummoned;
        Debug.Log(staticSummoned + " " + cardName);
        displayCard(); //sets the card's information and colour up to be rendered INTO PLACEHOLDER***
        hand = GameObject.Find("hand");
        //if this parent is the same as the hands parent 
        Debug.Log("this.transform.parent = "+ this.transform.parent);
        //if (this.transform.parent.gameObject == hand) //if this script's parent's gameobject is equal to the hand object, then show face of card to us
        //{
        //    cardBack = false;
        //}
        staticCardBack = cardBack; //make sure that the card doesn't show the back
        cloneDraw(); //this updates the card to be the last card in the static deck (which is updated to have one less each time a card is drawn)

        //currentZone = this.transform.parent; //supposed to grab each card's parent game object (hand, oppHand, playPanel, or oppPlayPanel)

        if (this.transform.parent != null)
        {
            currentZone = this.transform.parent.gameObject;
        }
        else
        {
            Debug.Log("Parent transform is null for object: " + this.name);
        }

        currentLoc = currentZone;

        playZone = GameObject.Find("playPanel");
        pz = playZone;
        //summoning logic and cost logic
        if (isSummoned == false)
        {
            Debug.Log(cardName + " Is summoned false");
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
                Debug.Log(cardName + " is now " + currentlyDraggable);
            }
            else
            {
                dragScript.isDraggable = false;
            }


            if (isSummoned == false && currentZone == playZone)
            {
                isSummoned = true;
                Debug.Log(cardName + " Summoned sucess | Cost: " + this.cost + " | Current zone: " + currentZone + " | play zone: " + playZone + " | Is summoned? " + isSummoned);
                GetComponent<dragScript>().enabled = false;
                turnScript.currentMana = turnScript.currentMana - this.cost;
                Debug.Log("Mana left: " + turnScript.currentMana);
            }
        }
        else
        {
            dragScript.isDraggable = false;
        }
        currentlyDraggable = dragScript.isDraggable;

        //decide attackers
        if (turnScript.isMyTurn == true && isSummoned == true && hasAttacked == false && currentZone == playZone)
        {
            cantAttack = false;
            Debug.Log(cardName + " ready to attack");
        }

        if (turnScript.isMyTurn == true && cantAttack == false)
        {
            canAttack = true;
        }
        else
        {
            cantAttack = true;
            canAttack = false;
        }
        targeting = staticTargeting;
        targetingEnemy = staticTargetingEnemy;

        if (targetingEnemy == true)
        {
            Target = Enemy;
        }
        else
        {
            Target = null;
        }

        if (targeting == true && targetingEnemy == true && onlyThisCardAttack == true)
        {
            Attack();
        }

    }

    private void Attack()
    {
        if (canAttack == true && isSummoned)
        {
            if (Target != null)
            {
                if (Target == Enemy)
                {
                    enemyHealth.HPStatic -= pow;
                    targeting = false;
                    cantAttack = true;
                    hasAttacked = true;
                }

                if (Target.name == "cardInHand(Clone)")
                {
                    canAttack = true;
                }
            }
        }
    }

    public void UntargetEnemy()
    {
        staticTargetingEnemy = false;
    }
    public void TargetEnemy()
    {
        staticTargetingEnemy = true;
    }
    public void StartAttack()
    {
        staticTargeting = true;
    }
    public void StopAttack()
    {
        staticTargeting = false;
    }
    public void OneCardAttack()
    {
        onlyThisCardAttack = true;
    }
    public void OneCardAttackStop()
    {
        onlyThisCardAttack = false;
    }
    private void displayCard()
    {
        this.id = displayList[0].id;//update ids to match real id of card
        this.cardName = displayList[0].cardName;
        this.colour = displayList[0].colour;//new
        this.pow = displayList[0].pow;
        this.hp = displayList[0].hp;
        this.txt = displayList[0].txt;
        this.cost = displayList[0].cost;

        nameText.text = " " + this.cardName.ToString();
        descriptionText.text = " " + this.txt.ToString();
        costText.text = " " + this.cost.ToString();
        powText.text = " " + this.pow.ToString();
        hpText.text = " " + this.hp.ToString();

        //trying to get the border of the card drwan to change colour to match the card's colour int
        Color border = renderCardColour(colour);//get what colour the border should be
        cardInHand.GetComponent<Image>().color = border; //then render the correct colour

    }
    private void cloneDraw()
    {
        Debug.Log("Made it to clone draw...");
        //clone cards for draw
        if (this.tag == "clone") //if this dbdisplay's card is a clone, then we draw it from the game deck and update the game deck's information
        {
            //
            displayList[0] = playerDeck.staticDeck[deckCount - 1]; //add last card of playerDeck's staticDeck to the top of the display list
            //
            deckCount -= 1;
            playerDeck.deckSize -= 1;
            cardBack = false;
            this.tag = "Untagged";
        }
    }

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
            return Color.yellow;
        }
    }








}
