using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectDeck : MonoBehaviour
{

    public TMPro.TextMeshProUGUI text1;
    public TMPro.TextMeshProUGUI text2;

    // Start is called before the first frame update
    void Start()
    {
        changeToBlack();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeToWhite()
    {
        text1.color = Color.white;
        text2.color = Color.white;
    }

    public void changeToBlack()
    {
        text1.color = Color.black;
        text2.color = Color.black;
    }
}
