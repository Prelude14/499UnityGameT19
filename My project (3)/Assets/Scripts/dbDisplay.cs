using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; //important --> use this for db method;

public class dbDisplay : MonoBehaviour
{
    public List<Card1> displayList = new List<Card1>();
    public int displayId;
    public static bool attackDragging;
    public int colour; //for each card's colour
    public int id;
    public int hp;
    public int pow;
    public int cost;
    public static int staticCost;
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
    public bool attackBorder;
    public static bool staticAttackBorder;
    public GameObject Image;
    public GameObject playableBorder;
    public GameObject unplayableBorder;
    public static GameObject currentLoc;
    public static GameObject pz;

    // Start is called before the first frame update
    void Start()
    {
        deckCount = playerDeck.deckSize;
        displayList[0] = cardDatabase.neutralCardList[displayId];
        this.id = displayList[0].id;

        isSummoned = false;
        Enemy = GameObject.Find("enemyHealth");
        canAttack = false;
        targeting = false;
        targetingEnemy = false;
        if (cost <= 1)
        {
            canBeSummoned = true;
        }
        else
        {
            canBeSummoned = false;
        }
        staticAttackBorder = false;
        attackBorder = false;
        playableBorder.SetActive(false);
        unplayableBorder.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        staticAttackBorder = false;
        staticCost = cost;
        staticSummoned = isSummoned;
        Debug.Log(staticSummoned + " " + cardName);
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
        currentLoc = currentZone;
        pz = playZone;
        //summoning logic and cost logic

        Debug.Log(cardName + " Is summoned false");
        if (this.cost <= turnScript.currentMana && isSummoned == false)
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
            playableBorder.SetActive(false);
            unplayableBorder.SetActive(true);
        }

        if (canBeSummoned)
        {
            dragScript.isDraggable = true;
            Debug.Log(cardName + " is now " + currentlyDraggable);
            playableBorder.SetActive(true);
            unplayableBorder.SetActive(false);
        }


        GameObject startParent = transform.parent.gameObject;

        if (isSummoned == false && currentZone == playZone)
        {
            unplayableBorder.SetActive(false);
            playableBorder.SetActive(false);
            if (this.cost > turnScript.currentMana)
            {
                transform.SetParent(GameObject.Find("hand").transform, true);
                return;
            }
            isSummoned = true;
            turnScript.totalSummons++;
            Debug.Log(cardName + " Summoned sucess | Cost: " + this.cost + " | Current zone: " + currentZone + " | play zone: " + playZone + " | Is summoned? " + isSummoned);
            //disable script component when summoned
            GetComponent<dragScript>().enabled = false;
            turnScript.currentMana = turnScript.currentMana - this.cost;
            Debug.Log("Mana left: " + turnScript.currentMana);

        }

        currentlyDraggable = dragScript.isDraggable;

        //decide attackers
        if (turnScript.isMyTurn == true && isSummoned == true && hasAttacked == false && currentZone == playZone)
        {
            cantAttack = false;
            Debug.Log(cardName + " ready to attack");
            unplayableBorder.SetActive(false);
            playableBorder.SetActive(false);
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

        if (canAttack == true && currentLoc == playZone)
        {
            attackBorder = true;
            staticAttackBorder = attackBorder;
        }
        else
        {
            staticAttackBorder = false;
            attackBorder = false;
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
                    staticAttackBorder = false;
                    attackBorder = false;

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
        if (currentZone == playZone)
        {
            attackDragging = true;
        }
    }
    public void StopAttack()
    {
        staticTargeting = false;
        if (currentZone == playZone)
        {
            attackDragging = false;
        }

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
        Image.GetComponent<Image>().color = border; //then render the correct colour


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
