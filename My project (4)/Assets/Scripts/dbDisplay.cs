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

    public GameObject Canvas;
    public GameObject ZoomCard;

    private GameObject zoomCard;
    private Sprite zoomSprite;

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
    public static int staticID;
    public static int staticCardColor;

    public static int staticPow;
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
     public void Awake()
    {
        Canvas = GameObject.Find("Canvas");
    }

    public void OnHoverEnter()
    {
        // Add logic for zooming in on hover enter
        Debug.Log("Zooming on: " + cardName);
        zoomCard = Instantiate(ZoomCard, new Vector2(Input.mousePosition.x, Input.mousePosition.y + 250), Quaternion.identity);
        zoomCard.transform.SetParent(Canvas.transform, true);
        RectTransform rect = zoomCard.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 300);
        zoomCard.GetComponent<contentZoom>().cardName = cardName;
        zoomCard.GetComponent<contentZoom>().txt = txt;
        zoomCard.GetComponent<contentZoom>().cost = cost;
        zoomCard.GetComponent<contentZoom>().pow = pow;
        zoomCard.GetComponent<contentZoom>().hp = hp;



    }

    public void OnHoverExit()
    {
        // Add logic for zooming out on hover exit
        Destroy(zoomCard);
    }

    // Update is called once per frame
    void Update()
    {
        staticPow = pow;
        staticCardColor = colour;
        staticID = id;
        staticAttackBorder = false;
        staticCost = cost;
        staticSummoned = isSummoned;
        // Debug.Log(staticSummoned + " " + cardName);
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

        // Debug.Log(cardName + " Is summoned false");
        if (this.cost <= turnScript.currentMana && isSummoned == false && turnScript.actionPoints >= 1)
        {
            canBeSummoned = true;
            // Debug.Log(cardName + " Is now playable");
            if (currentZone == hand)
            {
                // Debug.Log(cardName + " Is now playable");
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
            // Debug.Log(cardName + " is now " + currentlyDraggable);
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
            // Debug.Log(cardName + " Summoned sucess | Cost: " + this.cost + " | Current zone: " + currentZone + " | play zone: " + playZone + " | Is summoned? " + isSummoned);
            //disable script component when summoned
            GetComponent<dragScript>().enabled = false;
            turnScript.currentMana = turnScript.currentMana - this.cost;
            // Debug.Log("Mana left: " + turnScript.currentMana);
            turnScript.actionPoints--;

        }

        currentlyDraggable = dragScript.isDraggable;

        //decide attackers
        if (turnScript.isMyTurn == true && isSummoned == true && hasAttacked == false && currentZone == playZone && turnScript.actionPoints >= 1)
        {
            cantAttack = false;
            // Debug.Log(cardName + " ready to attack");
            unplayableBorder.SetActive(false);
            playableBorder.SetActive(false);
        }

        if (turnScript.isMyTurn == true && cantAttack == false && turnScript.actionPoints >= 1)
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
        if (canAttack == true && isSummoned && turnScript.actionPoints >= 1)
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
                    turnScript.actionPoints--;
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
