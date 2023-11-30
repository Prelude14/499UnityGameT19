using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler //
{
    public void OnBeginDrag(PointerEventData eventData)
    { //pointereventdata are cursor actions 
        //Ibegindraghandler requires this method
        Debug.Log("Begin Drag");
    }
    public void OnDrag(PointerEventData eventData)
    { //pointereventdata are cursor actions 
      //Ibegindraghandler requires this method
        Debug.Log("dragging");
        this.transform.position = eventData.position;

    }
    public void OnEndDrag(PointerEventData eventData)
    { //pointereventdata are cursor actions 
      //Ibegindraghandler requires this method

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
