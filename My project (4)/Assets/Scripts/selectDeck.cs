using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectDeck : MonoBehaviour
{

public static List<Card1> displayList = new List<Card1>();
public static int deckSize;

public void redDeckSelected()
    {
        displayList = cardDatabase.redCardList;
        deckSize = cardDatabase.redCardList.Count;

    }

public void blueDeckSelected()
    {
        displayList = cardDatabase.blueCardList;
        deckSize = cardDatabase.redCardList.Count;
    }

public void blackDeckSelected()
    {
        displayList = cardDatabase.blackCardList;
        deckSize = cardDatabase.redCardList.Count;
    }

public void whiteDeckSelected()
    {
        displayList = cardDatabase.whiteCardList;
        deckSize = cardDatabase.redCardList.Count;
    }
}

