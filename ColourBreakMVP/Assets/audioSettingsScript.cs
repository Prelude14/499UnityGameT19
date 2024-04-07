using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audioSettingsScript : MonoBehaviour
{

    public Text myText;
    public Slider mySlider;

    // void Start()
    // {
    // }

    void Update() {
        myText.text = "Volume: " + (int) (mySlider.value*100);
   }
}
