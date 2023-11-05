using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class testScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void populateDeck()
    {
        
        GameObject gameObject = new GameObject("playerDeck");
        playerDeck pd = gameObject.AddComponent<playerDeck>();
        // Use the Assert class to test conditions
        List<Card1> cardList = new List<Card1>();
        cardList.Add(new Card1(0, "None", 5, 1, 5, "None"));
        cardList.Add(new Card1(1, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(2, "Kobold charger", 5, 1, 5, "Haste"));
        cardList.Add(new Card1(3, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(4, "Warrior", 4, 4, 2, "haste"));
        cardList.Add(new Card1(5, "Rat", 1, 1, 1, ""));
        cardList.Add(new Card1(6, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(7, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(8, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(9, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(10, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(11, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(12, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(13, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(14, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(16, "Goblin", 2, 1, 1, ""));
        cardList.Add(new Card1(17, "Goblin", 2, 1, 1, ""));
        cardList.Add(new Card1(18, "Goblin", 2, 1, 1, ""));
        cardList.Add(new Card1(19, "Goblin", 2, 1, 1, ""));
        cardList.Add(new Card1(20, "Goblin Lord", 2, 1, 1, "Other goblins have +1/+1"));
        pd.populateDeck(cardList);
        var deckStart = pd.deck;
        Assert.AreEqual(deckStart, cardList);
        string result = "";
        for (int i = 0; i < deckStart.Count; i++) {
            result = result + deckStart[i].print() + ",";
        }
        Debug.Log(result);
    }

    [Test]
    public void shuffleDeck()
    {
        GameObject gameObject = new GameObject("playerDeck");
        playerDeck pd = gameObject.AddComponent<playerDeck>();
        // Use the Assert class to test conditions
        List<Card1> cardList = new List<Card1>();
        cardList.Add(new Card1(0, "None", 5, 1, 5, "None"));
        cardList.Add(new Card1(1, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(2, "Kobold charger", 5, 1, 5, "Haste"));
        cardList.Add(new Card1(3, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(4, "Warrior", 4, 4, 2, "haste"));
        cardList.Add(new Card1(5, "Rat", 1, 1, 1, ""));
        cardList.Add(new Card1(6, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(7, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(8, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(9, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(10, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(11, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(12, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(13, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(14, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(16, "Goblin", 2, 1, 1, ""));
        cardList.Add(new Card1(17, "Goblin", 2, 1, 1, ""));
        cardList.Add(new Card1(18, "Goblin", 2, 1, 1, ""));
        cardList.Add(new Card1(19, "Goblin", 2, 1, 1, ""));
        cardList.Add(new Card1(20, "Goblin Lord", 2, 1, 1, "Other goblins have +1/+1"));
        pd.populateDeck(cardList);
         int count = pd.deck.Count;
        List<Card1> container = new List<Card1>(new Card1[count]);
        var deckStart = pd.deck;
        string result = "";
        Debug.Log(deckStart.Count);
        for (int i = 0; i < deckStart.Count; i++) {
            result = result + deckStart[i].print() + ",";
        }
        Debug.Log("Preshuffle: "+result);

       
        pd.shuffle(count, container);
        var newDeck = pd.deck;
        
        string result2 = "";
        for (int i = 0; i < newDeck.Count; i++) {
            result2 = result2 + newDeck[i].print() + ",";
        }

        Debug.Log("Deck after shuffle: "+result2);
        bool check = false;

        if(result != result2) {
            check = true;
        }

        Assert.AreEqual(true, check);
    }
    [Test]
    public void draw(){

    }
    [Test]
    public void sizeCount(){
        
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator testScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
