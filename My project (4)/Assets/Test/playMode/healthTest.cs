using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
public class healthTest : MonoBehaviour
{
    // Start is called before the first frame update public IEnumerator healthTest()

    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("SampleScene");
    }
    [UnityTest]
    public IEnumerator healthTestMain()
    {

        int testHp = 25;
        GameObject.Find("circleBar").GetComponent<playerHealth>().setHealth(testHp);
        float result = 25f / 30f;
        Debug.Log(result);
        yield return null;
        float y = GameObject.Find("circleBar").GetComponent<playerHealth>().getFillAmount();
        Debug.Log(y);
        Assert.AreEqual(result, y);
        yield return new WaitForSeconds(2);

    }
    [UnityTest]
    public IEnumerator healthTestUnder0()
    {
        int testHp2 = -50;
        GameObject.Find("circleBar").GetComponent<playerHealth>().setHealth(testHp2);
        float result2 = 0f / 30f;
        Debug.Log(result2);
        yield return null;
        float z = GameObject.Find("circleBar").GetComponent<playerHealth>().getFillAmount();
        Debug.Log(z);
        Assert.AreEqual(result2, z);
        yield return new WaitForSeconds(2);

    }
    [UnityTest]
    public IEnumerator healthTestOver30()
    {
        int testHp2 = 50;
        GameObject.Find("circleBar").GetComponent<playerHealth>().setHealth(testHp2);
        float result2 = 50f / 30f;
        Debug.Log(result2);
        yield return null;
        float z = GameObject.Find("circleBar").GetComponent<playerHealth>().getFillAmount();
        Debug.Log(z);
        Assert.AreEqual(result2, z);
        yield return new WaitForSeconds(2);

    }
}
