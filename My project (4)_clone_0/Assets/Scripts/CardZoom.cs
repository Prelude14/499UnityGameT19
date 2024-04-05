using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject ZoomCard;

    private GameObject zoomCard;
    private Sprite zoomSprite;

    public void Awake()
    {
        Canvas = GameObject.Find("Canvas");
        zoomSprite = gameObject.GetComponent<Image>().sprite;
    }

    public void OnHoverEnter()
    {
        // Add logic for zooming in on hover enter
        zoomCard = Instantiate(ZoomCard, new Vector2(Input.mousePosition.x, Input.mousePosition.y + 250), Quaternion.identity);
        zoomCard.transform.SetParent(Canvas.transform, true);
        RectTransform rect = zoomCard.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 300);
    }

    public void OnHoverExit()
    {
        // Add logic for zooming out on hover exit
        Destroy(zoomCard);
    }

}
