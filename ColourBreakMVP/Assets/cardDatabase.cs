using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardDatabase : MonoBehaviour
{
    //public static List<Card1> cardList = new List<Card1>(); //this is the old placeholder dummy deck of cards

    void Awake(){
    //insert card using format: Card(int id, string cardName, int cost, int pow, int hp, string txt)
        //populateList(); //old list
        populateNeutralList();
        populateBlackList();
        populateRedList();
        populateWhiteList();
        populateBlueList();
    }
   /*
   public void populateList()
    { //colour 0 is the first int (1-4 are the actual colours)    //this is the old placeholder dummy deck of cards
        cardList.Add(new Card1(0, 0, "None", 5, 1, 5, "None"));
        cardList.Add(new Card1(0, 1, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(0, 2, "Kobold charger", 5, 1, 5, "Haste"));
        cardList.Add(new Card1(0, 3, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(0, 4, "Warrior", 4, 4, 2, "haste"));
        cardList.Add(new Card1(0, 5, "Rat", 1, 1, 1, ""));
        cardList.Add(new Card1(0, 6, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(0, 7, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(0, 8, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(0, 9, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(0, 10, "Training dummy", 1, 1, 1, "Taunt"));
        cardList.Add(new Card1(0, 11, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(0, 12, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(0, 13, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(0, 14, "Bat", 1, 1, 1, "Flying"));
        cardList.Add(new Card1(0, 16, "Goblin", 2, 1, 1, ""));
        cardList.Add(new Card1(0, 17, "Goblin", 2, 1, 1, ""));
        cardList.Add(new Card1(0, 18, "Goblin", 2, 1, 1, ""));
        cardList.Add(new Card1(0, 19, "Goblin", 2, 1, 1, ""));
        cardList.Add(new Card1(0, 20, "Goblin Lord", 2, 1, 1, "Other goblins have +1/+1"));
   }//old deck  ============================================================
   */


   //we've got 8 NEUTRAL cards that will be mixed in to every other deck
   public static List<Card1> neutralCardList = new List<Card1>();

   public void populateNeutralList(){ //colour 5 is for neutral since 0 is the old deck and 1-4 are the colours
        
                             //colour,  id,  name,  cost, pow,  hp,  description
        neutralCardList.Add(new Card1(5, 0, "Stick Man", 1, 2, 3, " "));
        neutralCardList.Add(new Card1(5, 1, "Stick Man", 1, 2, 3, " "));
        neutralCardList.Add(new Card1(5, 2, "PickPocket", 2, 1, 3, "Draw a card"));
        neutralCardList.Add(new Card1(5, 3, "PickPocket", 2, 1, 3, "Draw a card"));
        neutralCardList.Add(new Card1(5, 4, "Toy Pistol", 5, 3, 4, "Deal 3 damage to the enemy"));
        neutralCardList.Add(new Card1(5, 5, "Toy Pistol", 5, 3, 4, "Deal 3 damage to the enemy"));
        neutralCardList.Add(new Card1(5, 6, "Toy Bomb", 7, 3, 3, "Destroy all creatures currently in play, including this one!"));
        neutralCardList.Add(new Card1(5, 7, "Toy Bomb", 7, 3, 3, "Destroy all creatures currently in play, including this one!"));

   } // end NEUTRAL deck ============================================================

   //this will be the BLACK deck of cards. Its made up of 12 cards that specialize in high risk high reward play styles. 
   public static List<Card1> blackCardList = new List<Card1>();

   public void populateBlackList(){ //colour 1 is for black cards

                                  //colour,  id,  name,  cost, pow,  hp,  description
        blackCardList.Add(new Card1(1, 0, "Curse Gamble", 1, 3, 3, "Deal 2 damage to yourself"));
        blackCardList.Add(new Card1(1, 1, "Curse Gamble", 1, 3, 3, "Deal 2 damage to yourself"));

        blackCardList.Add(new Card1(1, 2, "Eye for an Eye", 3, 1, 2, "Destroy a random minion, deal 5 damage to yourself"));
        blackCardList.Add(new Card1(1, 3, "Eye for an Eye", 3, 1, 2, "Destroy a random minion, deal 5 damage to yourself"));

        blackCardList.Add(new Card1(1, 4, "Necromancer's Summon", 5, 5, 5, "Deal 3 to self and 3 damage to the enemy"));
        blackCardList.Add(new Card1(1, 5, "Necromancer's Summon", 5, 5, 5, "Deal 3 to self and 3 damage to the enemy"));

        blackCardList.Add(new Card1(1, 6, "Russian Roulette", 10, 3, 4, "For each life point under 30, deal that much damage to the enemy"));
        blackCardList.Add(new Card1(1, 7, "Russian Roulette", 10, 3, 4, "For each life point under 30, deal that much damage to the enemy"));

        blackCardList.Add(new Card1(1, 8, "Sacrificial Lamb", 6, 1, 1, "Heal half the damage you've lost throughout the game"));
        blackCardList.Add(new Card1(1, 9, "Sacrificial Lamb", 6, 1, 1, "Heal half the damage you've lost throughout the game"));

        blackCardList.Add(new Card1(1, 10, "Vengeful/Spiteful Spirits", 5, 3, 2, "Discard a random card, and then deal 3 damage to all enemy creatures. If you have discarded 3 or more cards so far, then it deals 6 damage to all enemy creatures instead."));
        blackCardList.Add(new Card1(1, 11, "Vengeful/Spiteful Spirits", 5, 3, 2, "Discard a random card, and then deal 3 damage to all enemy creatures. If you have discarded 3 or more cards so far, then it deals 6 damage to all enemy creatures instead."));

   } //end BLACK deck  ============================================================


   //this will be the RED deck of cards. Its made up of 12 cards that specialize in aggressive but low cost play styles. 
   public static List<Card1> redCardList = new List<Card1>();

   public void populateRedList(){ //colour 2 is for red cards

                                 //colour,  id,  name,  cost, pow,  hp,  description
        redCardList.Add(new Card1(2, 0, "Toxic Blade", 1, 2, 1, "If this creature attacks an enemy, deal another 2 damage"));
        redCardList.Add(new Card1(2, 1, "Toxic Blade", 1, 2, 1, "If this creature attacks an enemy, deal another 2 damage"));

        redCardList.Add(new Card1(2, 2, "Beserker", 3, 5, 1, " "));
        redCardList.Add(new Card1(2, 3, "Beserker", 3, 5, 1, " "));

        redCardList.Add(new Card1(2, 4, "Summoner", 4, 2, 3, "Draw a new card for each creature you currently control"));
        redCardList.Add(new Card1(2, 5, "Summoner", 4, 2, 3, "Draw a new card for each creature you currently control"));

        redCardList.Add(new Card1(2, 6, "Dogpiler", 5, 2, 4, "Destroy a random enemy minion. Repeat this effect for each creature you control"));
        redCardList.Add(new Card1(2, 7, "Dogpiler", 5, 2, 4, "Destroy a random enemy minion. Repeat this effect for each creature you control"));

        redCardList.Add(new Card1(2, 8, "Cannoneer", 3, 2, 2, "Deal 4 damage to the enemy"));
        redCardList.Add(new Card1(2, 9, "Cannoneer", 3, 2, 2, "Deal 4 damage to the enemy"));

        redCardList.Add(new Card1(2, 10, "Soldier's Pact", 8, 4, 4, "Deal 2 damage to the enemy for each creature you control"));
        redCardList.Add(new Card1(2, 11, "Soldier's Pact", 8, 4, 4, "Deal 2 damage to the enemy for each creature you control"));

   } //end RED deck  =============================================================


   //this will be the WHITE deck of cards. Its made up of 12 cards that specialize in lifegain and more slow sustainability play styles. 
   public static List<Card1> whiteCardList = new List<Card1>();

   public void populateWhiteList(){ //colour 3 is for white cards

                                //colour,  id,  name,  cost, pow,  hp,  description
        whiteCardList.Add(new Card1(3, 0, "Healer", 1, 1, 1, "Heal 2 damage"));
        whiteCardList.Add(new Card1(3, 1, "Healer", 1, 1, 1, "Heal 2 damage"));

        whiteCardList.Add(new Card1(3, 2, "Spiteful Healer", 4, 2, 2, "Heal 2 damage, deal 2 damage to the enemy"));
        whiteCardList.Add(new Card1(3, 3, "Spiteful Healer", 4, 2, 2, "Heal 2 damage, deal 2 damage to the enemy"));

        whiteCardList.Add(new Card1(3, 4, "Lucky Healer", 5, 5, 5, "Draw a card and heal 5 damage"));
        whiteCardList.Add(new Card1(3, 5, "Lucky Healer", 5, 5, 5, "Draw a card and heal 5 damage"));

        whiteCardList.Add(new Card1(3, 6, "Field Medic", 3, 2, 4, "Heal 3 damage, destroy a random enemy minion"));
        whiteCardList.Add(new Card1(3, 7, "Field Medic", 3, 2, 4, "Heal 3 damage, destroy a random enemy minion"));

        whiteCardList.Add(new Card1(3, 8, "Surprise Gift", 8, 3, 2, "Heal back to full health"));
        whiteCardList.Add(new Card1(3, 9, "Surprise Gift", 8, 3, 2, "Heal back to full health"));

        whiteCardList.Add(new Card1(3, 10, "Karma Heal", 10, 5, 5, "Deal an amount of damage equal to the amount you have healed this game to the enemy"));
        whiteCardList.Add(new Card1(3, 11, "Karma Heal", 10, 5, 5, "Deal an amount of damage equal to the amount you have healed this game to the enemy"));
    
   } //end WHITE deck  =============================================================


   //this will be the BLUE deck of cards. Its made up of 12 cards that specialize in chaos and more disruptive play syles. Draw tons of cards
   public static List<Card1> blueCardList = new List<Card1>();

   public void populateBlueList(){ //colour 4 is for blue cards

                               //colour,  id,  name,  cost, pow,  hp,  description
        blueCardList.Add(new Card1(4, 0, "Damage Dealer", 2, 1, 1, "Draw 1"));
        blueCardList.Add(new Card1(4, 1, "Damage Dealer", 2, 1, 1, "Draw 1"));

        blueCardList.Add(new Card1(4, 2, "Minion Hunter", 4, 2, 2, "Destroy a random enemy minion and draw 1"));
        blueCardList.Add(new Card1(4, 3, "Minion Hunter", 4, 2, 2, "Destroy a random enemy minion and draw 1"));

        blueCardList.Add(new Card1(4, 4, "Firesale Shuffle", 3, 1, 2, "Draw 2 and deal 1"));
        blueCardList.Add(new Card1(4, 5, "Firesale Shuffle", 3, 1, 2, "Draw 2 and deal 1"));

        blueCardList.Add(new Card1(4, 6, "Balance the Scales", 5, 3, 5, "Draw cards until you have the same amount of cards in your hand as your opponent"));
        blueCardList.Add(new Card1(4, 7, "Balance the Scales", 5, 3, 5, "Draw cards until you have the same amount of cards in your hand as your opponent"));

        blueCardList.Add(new Card1(4, 8, "Clean Slate", 4, 3, 2, "Discard your hand, draw that many cards plus 1"));
        blueCardList.Add(new Card1(4, 9, "Clean Slate", 4, 3, 2, "Discard your hand, draw that many cards plus 1"));

        blueCardList.Add(new Card1(4, 10, "The House Always Wins", 6, 2, 2, "Deal 2 damage to a random enemy for every card you have drawn this game"));
        blueCardList.Add(new Card1(4, 11, "The House Always Wins", 6, 2, 2, "Deal 2 damage to a random enemy for every card you have drawn this game"));
    
   } //end BLUE deck  =============================================================

}

