using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class videoSettingsScript : MonoBehaviour
{

    public Text myText;
    public Slider mySlider;
    private SpriteRenderer[] spriteRenderers;
    public float brightnessValue;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderers = FindObjectsOfType<SpriteRenderer>();
        brightnessValue = mySlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        myText.text = "Bightness: " + (int) (mySlider.value*100);
        brightnessValue = mySlider.value;
        // foreach(SpriteRenderer spriteRenderer in spriteRenderers){
        //     spriteRenderer.color = new Color(brightnessValue,brightnessValue,brightnessValue, spriteRenderer.color.a);
        // }
    }
}
