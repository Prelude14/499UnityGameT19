using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class tutPlayerDeck : MonoBehaviour
{
    public List<Card1> container = new List<Card1>();
    public int deckSize;
    public int handSize = 0;
    public List<Card1> tutDeck = new List<Card1>();
    public List<Card1> staticDeck = new List<Card1>();
    public int x;
    public static int staticAmount;
    public static bool drawStatic = false;
    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public int burnDamage = 1;
    public GameObject cardInDeck4;
    public GameObject cardInDeck5;
    public GameObject cardInDeck6;

    public GameObject[] clones;
    public GameObject hand;
    public GameObject tutCardInHand;
    public Text deckCountText;

    public static int playerColour;

    // Start is called before the first frame update
    void Start()
    {

        x = 0;
        deckSize = 8;

        //populate card list depending on decks selected. Done inside the function
        populateTutDeck();

        shuffle();
        StartCoroutine(StartGame());

    }
    // Update is called once per frame
    void Update()
    {
        //reduce number of "visible" cards on the -deck- stack
        if (drawStatic == true)
        {
            Debug.Log("DRAW TRIGGER: " + staticAmount);
            StartCoroutine(Draw(staticAmount));
            drawStatic = false;
        }
        staticDeck = tutDeck;

        changeSize();

        //display deck card count;
        deckCountText.text = "" + deckSize;
        if (tutTurnScript.turnStart == true)
        {
            StartCoroutine(Draw(1));
            tutTurnScript.turnStart = false;
        }
    }
    IEnumerator StartGame()
    {
        //coroutine: way to count down
        //initial draw
        for (int i = 0; i < 4; i++)
        { // number of starting hand
            yield return new WaitForSeconds(.15f);
            //each second it draws a card
            //spawns new object using instantiate duplicating it as a clone of cardInHand
            GameObject card = Instantiate(tutCardInHand, new Vector2(0, 0), Quaternion.identity);

            //renderCardColour(card);
        }
        Debug.Log(tutTurnScript.currentMana);

    }

    public void changeSize()
    {
        if (deckSize < 20)
        {
            cardInDeck6.SetActive(false);
        }
        if (deckSize < 15)
        {
            cardInDeck5.SetActive(false);
        }
        if (deckSize < 10)
        {
            cardInDeck4.SetActive(false);
        }
        if (deckSize < 5)
        {
            cardInDeck3.SetActive(false);
        }
        if (deckSize < 3)
        {
            cardInDeck2.SetActive(false);
        }
        if (deckSize < 1)
        {
            cardInDeck1.SetActive(false);
        }
    }
    public void shuffle()
    {
        for (int i = 0; i < deckSize; i++)
        {
            container.Add(tutDeck[i]); //always store current combinedDeck value in first slot of container's list
            int randomIndex = Random.Range(i, deckSize);
            tutDeck[i] = tutDeck[randomIndex];
            tutDeck[randomIndex] = container[0];
            container.RemoveAt(0); //delete container's first value after use, so next card is shuffled properly
        }
        //container should show 
        // string result = "";
        // for (int i = 0; i < deck.Count; i++) {
        //     result = result + deck[i].print() + ",";
        // }
        // Debug.Log(result);
    }

    IEnumerator Draw(int drawSize)
    {
        if (deckSize > 0) //check if there's other cards. do not draw if no more cards do smth else;
        {
            //draw cards repeat until drawSize
            for (int x = 0; x < drawSize; x++)
            {
                //slow down code so we don't draw too fast
                yield return new WaitForSeconds(.15F);
                //Instantiate(cardInHand, transform.position, transform.rotation);
                GameObject card = Instantiate(tutCardInHand, new Vector2(0, 0), Quaternion.identity);

                //renderCardColour(card);
            }
        }
        else
        {
            //lose game
            //TODO: Add a lose health

            tutPlayerHealth.HPStatic -= burnDamage;
            burnDamage++;
        }
        drawStatic = false;
    }

    //add 20 cards from the cardDatabase using a neutral deck plus one colour (which gets selected by player)
    public void populateTutDeck()
    {
        populateTutorialList(); //generate list of playing cards cutom for tutorial
        //every deck gets the same 8 neutral cards added to it first
        for (int i = 0; i < 8; i++)
        {
            tutDeck.Add(tutorialCardList[i]);
        } //all 8 cards from tutorial deck to player deck

       

    }

    //we've got 8 NEUTRAL cards that will be mixed in to every other deck
    public static List<Card1> tutorialCardList = new List<Card1>();

    public void populateTutorialList()
    { //colour 5 is for neutral since 0 is the old deck and 1-4 are the colours

        //colour,  id,  name,  cost, pow,  hp,  description
        tutorialCardList.Add(new Card1(5, 0, "Stick Man", 1, 2, 3, "Just a dude. No abilities.")); //low cost neutral
        tutorialCardList.Add(new Card1(5, 1, "Stick Man", 1, 2, 3, "Just a dude. No abilities.")); //low cost neutral
        tutorialCardList.Add(new Card1(5, 2, "PickPocket", 2, 1, 3, "Draw a card")); //low cost neutral
        tutorialCardList.Add(new Card1(1, 3, "Eye for an Eye", 3, 1, 2, "Destroy a random minion, deal 5 damage to yourself")); //interesting black card
        tutorialCardList.Add(new Card1(1, 4, "Curse Gamble", 1, 3, 3, "Deal 2 damage to yourself")); //cheap black card
        tutorialCardList.Add(new Card1(2, 5, "Toxic Blade", 1, 2, 1, "If this creature attacks an enemy, deal another 2 damage")); //interesting red card
        tutorialCardList.Add(new Card1(3, 6, "Healer", 1, 1, 1, "Heal 2 damage")); //cheap white
        tutorialCardList.Add(new Card1(4, 7, "Damage Dealer", 2, 1, 1, "Draw 1")); //cheapest blue

    } // end NEUTRAL deck ============================================================


    public int getDeckSize()
    {
        return deckSize;
    }
    public string getDeckTop()
    {
        return tutDeck[0].ToString();
    }

}
