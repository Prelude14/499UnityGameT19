using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
public class playerDeck : MonoBehaviour
{
    public List<Card1> container = new List<Card1>();
    public static int deckSize;
    public int handSize = 0;
    public List<Card1> deck = new List<Card1>();
    public static List<Card1> staticDeck = new List<Card1>();
    public int x;

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    
    public GameObject[] clones;
    public GameObject hand;
    public GameObject cardInHand;
    public Text deckCountText;

    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        deckSize = 20;
        //populate card list
        populateDeck();
        shuffle();
        StartCoroutine(StartGame());

    }

    // Update is called once per frame
    void Update()
    {
        //reduce number of "visible" cards on the -deck- stack
        staticDeck = deck;
        if(deckSize < 20){
            cardInDeck1.SetActive(false);
        }
        if(deckSize<10) {
            cardInDeck2.SetActive(false);
        }
        if(deckSize<1) {
            cardInDeck3.SetActive(false);
        }

        //display deck card count;
        deckCountText.text = "" + deckSize; 
        
    }
    IEnumerator StartGame() {
        //coroutine: way to count down
        
        for(int i = 0; i < 5; i++){ // number of starting hand
            yield return new  WaitForSeconds(1);
            //each second it draws a card
            //spawns new object using instantiate duplicating it as a clone
            Instantiate(cardInHand, transform.position, transform.rotation);
        }

    }
    public void shuffle(){
        for(int i =0; i < deckSize ; i++) {
            container[0] = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container[0];
        }
        // string result = "";
        // for (int i = 0; i < deck.Count; i++) {
        //     result = result + deck[i].print() + ",";
        // }
        // Debug.Log(result);
    }
     public void shuffle(int deckSize, List<Card1> container){
        for(int i = 0; i < deckSize ; i++) {
            container[0] = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container[0];
        }
        
    }
    public void populateDeck(){
        for (int i = 0; i < 20; i++){
            deck[i] = cardDatabase.cardList[i];
        }
    }
    public void populateDeck(List<Card1> listA){
        int listSize = listA.Count;
        for (int i = 0; i < listSize; i++){
            deck.Add(listA[i]);
        }
    }

}
