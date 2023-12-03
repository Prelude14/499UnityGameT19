using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
public class numberTest
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("SampleScene");
    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator DeckSize()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
        int x = GameObject.Find("playerDeck").GetComponent<playerDeck>().getDeckSize();
        string y = GameObject.Find("playerDeck").GetComponent<playerDeck>().getDeckTop();
        Debug.Log(x);
        Debug.Log(y);
        bool success = false;
        if (x == 20 && y == "Card1")
        {
            success = true;
        }
        Assert.AreEqual(true, success);
    }
    [UnityTest]
    public IEnumerator healthTest()
    {
        int testHp = 25;
        GameObject.Find("circleBar").GetComponent<playerHealth>().setHealth(testHp);
        float result = 25f / 30f;
        Debug.Log(result);
        yield return null;
        float y = GameObject.Find("circleBar").GetComponent<playerHealth>().getFillAmount();
        Debug.Log(y);
        Assert.AreEqual(result, y);

    }
    // [UnityTest]
    // public IEnumerator turn_test()
    // {

    //     yield return new WaitForSeconds(7);
    //     GameObject gameObject = new GameObject();
    //     turnScript turnScriptComponent = gameObject.AddComponent<turnScript>();

    //     turnScriptComponent.Start();

    //     // Ensure initial values are set correctly
    //     Debug.Log("Initial Values:");
    //     Debug.Log("isMyTurn: " + turnScriptComponent.isMyTurn);
    //     Debug.Log("myTurn: " + turnScriptComponent.myTurn);
    //     Debug.Log("isTheirTurn: " + turnScriptComponent.isTheirTurn);
    //     Debug.Log("maxMana: " + turnScriptComponent.maxMana);
    //     Debug.Log("currentMana: " + turnScriptComponent.currentMana);
    //     Debug.Log("turnStart: " + turnScript.turnStart);

    //     Assert.IsTrue(turnScriptComponent.isMyTurn);
    //     Assert.AreEqual(1, turnScriptComponent.myTurn);
    //     Assert.AreEqual(0, turnScriptComponent.isTheirTurn);
    //     Assert.AreEqual(1, turnScriptComponent.maxMana);
    //     Assert.AreEqual(1, turnScriptComponent.currentMana);
    //     Assert.IsFalse(turnScript.turnStart);

    //     // Simulate ending player's turn
    //     turnScriptComponent.endTurn();
    //     Debug.Log("After ending player's turn:");
    //     Debug.Log("isMyTurn: " + turnScriptComponent.isMyTurn);
    //     Debug.Log("isTheirTurn: " + turnScriptComponent.isTheirTurn);

    //     // Ensure it's now opponent's turn
    //     Assert.IsFalse(turnScriptComponent.isMyTurn);
    //     Assert.AreEqual(1, turnScriptComponent.isTheirTurn);

    //     // Simulate ending opponent's turn
    //     turnScriptComponent.endOpponentTurn();
    //     yield return null; // Wait for the end of frame update

    //     Debug.Log("After ending opponent's turn:");
    //     Debug.Log("isMyTurn: " + turnScriptComponent.isMyTurn);
    //     Debug.Log("myTurn: " + turnScriptComponent.myTurn);
    //     Debug.Log("maxMana: " + turnScriptComponent.maxMana);
    //     Debug.Log("currentMana: " + turnScriptComponent.currentMana);
    //     Debug.Log("turnStart: " + turnScript.turnStart);

    //     // Ensure it's back to player's turn, and mana is updated
    //     Assert.IsTrue(turnScriptComponent.isMyTurn);
    //     Assert.AreEqual(1, turnScriptComponent.myTurn);
    //     Assert.AreEqual(2, turnScriptComponent.maxMana);
    //     Assert.AreEqual(2, turnScriptComponent.currentMana);
    //     Assert.IsTrue(turnScript.turnStart);
    // }
}