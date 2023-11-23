using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public static int maxHp;
    public static int HPStatic;
    public int hp;
    public Image health;
    public Text hpText;
    void Start()
    {
        maxHp = 30;
        HPStatic = 30;
    }

    // Update is called once per frame
    void Update()
    {
        //Fill health bar to this much
        fillHealth();
    }
    public int getHealth()
    {
        //return current hp
        return hp;
    }
    public void fillHealth()
    {
        hp = HPStatic;
        health.fillAmount = hp / maxHp; //percentage of fill amount
        if (hp >= maxHp)
        {
            hp = maxHp;
        }
        hpText.text = hp.ToString();
    }
}
