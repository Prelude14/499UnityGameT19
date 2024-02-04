using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardDatabase : MonoBehaviour
{
   public static List<Card1> cardList = new List<Card1>();

   void Awake()
   {
      //insert card using format: Card(int id, string cardName, int cost, int pow, int hp, string txt)
      //populateList(); //old list
      populateNeutralList();
      populateBlackList();
      populateRedList();
      populateWhiteList();
      populateBlueList();
   }
   public void populateList()
   { //colour 0 is the first int (1-4 are the actual colours)
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

   //we've got 8 NEUTRAL cards that will be mixed in to every other deck
   public static List<Card1> neutralCardList = new List<Card1>();

   public void populateNeutralList()
   { //colour 5 is for neutral since 0 is the old deck and 1-4 are the colours

      //colour,  id,  name,  cost, pow,  hp,  description
      neutralCardList.Add(new Card1(5, 0, "Stick Man", 1, 2, 3, "No abilities. Just a Dude."));
      neutralCardList.Add(new Card1(5, 1, "Stick Man", 1, 2, 3, "No abilities. Just a Dude."));
      neutralCardList.Add(new Card1(5, 2, "PickPocket", 2, 1, 3, "Selectively pick a card that matches your chosen deck colour from the player deck."));
      neutralCardList.Add(new Card1(5, 3, "PickPocket", 2, 1, 3, "Selectively pick a card that matches your chosen deck colour from the player deck."));
      neutralCardList.Add(new Card1(5, 4, "ToyPistol", 5, 3, 4, "Deal 3 damage to target."));
      neutralCardList.Add(new Card1(5, 5, "ToyPistol", 5, 3, 4, "Deal 3 damage to target."));
      neutralCardList.Add(new Card1(5, 6, "Toy Bomb", 7, 3, 3, "Destroy all other creatures currently in play."));
      neutralCardList.Add(new Card1(5, 7, "Toy Bomb", 7, 3, 3, "Destroy all other creatures currently in play."));

   } // end NEUTRAL deck ============================================================

   //this will be the BLACK deck of cards. Its made up of 12 cards that specialize in high risk high reward play styles. 
   public static List<Card1> blackCardList = new List<Card1>();

   public void populateBlackList()
   { //colour 1 is for black cards

      //colour,  id,  name,  cost, pow,  hp,  description
      blackCardList.Add(new Card1(1, 0, "Curse Gamble", 1, 3, 3, "Deal 2 damage to yourself."));
      blackCardList.Add(new Card1(1, 1, "Curse Gamble", 1, 3, 3, "Deal 2 damage to yourself."));

      blackCardList.Add(new Card1(1, 2, "Eye for an Eye", 3, 1, 2, "Discard a random card from your hand, but destroy a target creature as a trade."));
      blackCardList.Add(new Card1(1, 3, "Eye for an Eye", 3, 1, 2, "Discard a random card from your hand, but destroy a target creature as a trade."));

      blackCardList.Add(new Card1(1, 4, "Necromancer's Summon", 7, 5, 5, "If this card gets discarded, it gets summoned to the play area instead."));
      blackCardList.Add(new Card1(1, 5, "Necromancer's Summon", 7, 5, 5, "If this card gets discarded, it gets summoned to the play area instead."));

      blackCardList.Add(new Card1(1, 6, "Russian Roulette", 6, 3, 4, "Deal 1 damage for every card you've discarded so far to a single random enemy."));
      blackCardList.Add(new Card1(1, 7, "Russian Roulette", 6, 3, 4, "Deal 1 damage for every card you've discarded so far to a single random enemy."));

      blackCardList.Add(new Card1(1, 8, "Sacrificial Lamb", 3, 1, 1, "Discard a random card, and then draw a new card for each card you have discarded so far."));
      blackCardList.Add(new Card1(1, 9, "Sacrificial Lamb", 3, 1, 1, "Discard a random card, and then draw a new card for each card you have discarded so far."));

      blackCardList.Add(new Card1(1, 10, "Vengeful/Spiteful Spirits", 4, 3, 2, "Discard a random card, and then deal 3 damage to all enemy creatures. If you have discarded 3 or more cards so far, then it deals 6 damage to all enemy creatures instead."));
      blackCardList.Add(new Card1(1, 11, "Vengeful/Spiteful Spirits", 4, 3, 2, "Discard a random card, and then deal 3 damage to all enemy creatures. If you have discarded 3 or more cards so far, then it deals 6 damage to all enemy creatures instead."));

   } //end BLACK deck  ============================================================


   //this will be the RED deck of cards. Its made up of 12 cards that specialize in aggressive but low cost play styles. 
   public static List<Card1> redCardList = new List<Card1>();

   public void populateRedList()
   { //colour 2 is for red cards

      //colour,  id,  name,  cost, pow,  hp,  description
      redCardList.Add(new Card1(2, 0, "Toxic Blade", 1, 2, 1, "If this creature attacks an enemy, deal another 2 damage."));
      redCardList.Add(new Card1(2, 1, "Toxic Blade", 1, 2, 1, "If this creature attacks an enemy, deal another 2 damage."));

      redCardList.Add(new Card1(2, 2, "Beserker", 2, 1, 1, "Gives all your creatures played a +1/+0 stat buff."));
      redCardList.Add(new Card1(2, 3, "Beserker", 2, 1, 1, "Gives all your creatures played a +1/+0 stat buff."));

      redCardList.Add(new Card1(2, 4, "Summoner", 4, 2, 3, "Draw a new card for each creature you currently control."));
      redCardList.Add(new Card1(2, 5, "Summoner", 4, 2, 3, "Draw a new card for each creature you currently control."));

      redCardList.Add(new Card1(2, 6, "Dogpiler", 5, 2, 4, "Deal 2 damage for every creature you have attacked with so far, then destroy all other creatures."));
      redCardList.Add(new Card1(2, 7, "Dogpiler", 5, 2, 4, "Deal 2 damage for every creature you have attacked with so far, then destroy all other creatures."));

      redCardList.Add(new Card1(2, 8, "Revenge Seeker", 3, 2, 2, "Gives all your creatures played a +2/+0 stat buff."));
      redCardList.Add(new Card1(2, 9, "Revenge Seeker", 3, 2, 2, "Gives all your creatures played a +2/+0 stat buff."));

      redCardList.Add(new Card1(2, 10, "Suicide Pact", 8, 4, 4, "Destroy all of your other creatures in play, and for each creature you sacrifice, deal 2 damage to a random enemy target."));
      redCardList.Add(new Card1(2, 11, "Suicide Pact", 8, 4, 4, "Destroy all of your other creatures in play, and for each creature you sacrifice, deal 2 damage to a random enemy target."));

   } //end RED deck  =============================================================


   //this will be the WHITE deck of cards. Its made up of 12 cards that specialize in lifegain and more slow sustainability play styles. 
   public static List<Card1> whiteCardList = new List<Card1>();

   public void populateWhiteList()
   { //colour 3 is for white cards

      //colour,  id,  name,  cost, pow,  hp,  description
      whiteCardList.Add(new Card1(3, 0, "Healer", 1, 1, 1, "Heal yourself for 2 damage. Maybe also able to heal other creatures."));
      whiteCardList.Add(new Card1(3, 1, "Healer", 1, 1, 1, "Heal yourself for 2 damage. Maybe also able to heal other creatures."));

      whiteCardList.Add(new Card1(3, 2, "Spiteful Healer", 4, 2, 2, "Heal yourself for 2 damage, and deal 2 damage to the enemy as well."));
      whiteCardList.Add(new Card1(3, 3, "Spiteful Healer", 4, 2, 2, "Heal yourself for 2 damage, and deal 2 damage to the enemy as well."));

      whiteCardList.Add(new Card1(3, 4, "Lucky Healer", 5, 5, 5, "Draw a new card, and heal yourself an amount of damage equal to the cost of the card that was drawn."));
      whiteCardList.Add(new Card1(3, 5, "Lucky Healer", 5, 5, 5, "Draw a new card, and heal yourself an amount of damage equal to the cost of the card that was drawn."));

      whiteCardList.Add(new Card1(3, 6, "Field Medic", 3, 2, 4, "Heal a target creature for 3 damage."));
      whiteCardList.Add(new Card1(3, 7, "Field Medic", 3, 2, 4, "Heal a target creature for 3 damage."));

      whiteCardList.Add(new Card1(3, 8, "Copycat", 4, 3, 2, "Summon a copy of this creature if you have healed 5 damage so far."));
      whiteCardList.Add(new Card1(3, 9, "Copycat", 4, 3, 2, "Summon a copy of this creature if you have healed 5 damage so far."));

      whiteCardList.Add(new Card1(3, 10, "Karma Heal", 6, 5, 5, "Deal an amount of damage equal to the amount you have healed this game to the enemy."));
      whiteCardList.Add(new Card1(3, 11, "Karma Heal", 6, 5, 5, "Deal an amount of damage equal to the amount you have healed this game to the enemy."));

   } //end WHITE deck  =============================================================


   //this will be the BLUE deck of cards. Its made up of 12 cards that specialize in chaos and more disruptive play syles. Draw tons of cards
   public static List<Card1> blueCardList = new List<Card1>();

   public void populateBlueList()
   { //colour 4 is for blue cards

      //colour,  id,  name,  cost, pow,  hp,  description
      blueCardList.Add(new Card1(4, 0, "Damage Dealer", 2, 1, 1, "Deal damage equal to the amount of cards you've drawn this turn to one enemy minion."));
      blueCardList.Add(new Card1(4, 1, "Damage Dealer", 2, 1, 1, "Deal damage equal to the amount of cards you've drawn this turn to one enemy minion."));

      blueCardList.Add(new Card1(4, 2, "Minion Hunter", 4, 2, 2, "Destroy one enemy minion."));
      blueCardList.Add(new Card1(4, 3, "Minion Hunter", 4, 2, 2, "Destroy one enemy minion."));

      blueCardList.Add(new Card1(4, 4, "Firesale Shuffle", 2, 1, 2, "Draw a new card, and reduce its cost by 1."));
      blueCardList.Add(new Card1(4, 5, "Firesale Shuffle", 2, 1, 2, "Draw a new card, and reduce its cost by 1."));

      blueCardList.Add(new Card1(4, 6, "Balance the Scales", 5, 3, 5, "Draw new cards until you have the same amount of cards in your hand as your opponent."));
      blueCardList.Add(new Card1(4, 7, "Balance the Scales", 5, 3, 5, "Draw new cards until you have the same amount of cards in your hand as your opponent."));

      blueCardList.Add(new Card1(4, 8, "Clean Slate", 4, 3, 2, "You and your opponent both discard your entire hands. You then draw the same amount that you discarded plus 1 extra card."));
      blueCardList.Add(new Card1(4, 9, "Clean Slate", 4, 3, 2, "You and your opponent both discard your entire hands. You then draw the same amount that you discarded plus 1 extra card."));

      blueCardList.Add(new Card1(4, 10, "The House Always Wins", 6, 2, 2, "Deal 2 damage to a random enemy for every card you have drawn this game."));
      blueCardList.Add(new Card1(4, 11, "The House Always Wins", 6, 2, 2, "Deal 2 damage to a random enemy for every card you have drawn this game."));

   } //end BLUE deck  =============================================================

}

