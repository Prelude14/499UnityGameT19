using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardDatabase : MonoBehaviour
{
   public static List<Card1> cardList = new List<Card1>();

   void Awake(){
    //insert card using format: Card(int id, string cardName, int cost, int pow, int hp, string txt)
      populateList();
   }
   public void populateList(){
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
   }
}
