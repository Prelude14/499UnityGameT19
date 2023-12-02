using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler //
{
  Transform parentReturn = null;

  public void OnBeginDrag(PointerEventData eventData)
  { //pointereventdata are cursor actions 
    //Ibegindraghandler requires this method
    parentReturn = this.transform.parent; //where to return to if bounce
    this.transform.SetParent(this.transform.parent.parent); //set current card to parent of location
    Debug.Log(parentReturn);
    // GetComponent<CanvasGroup>().blocksRaycasts = false; //stop raycast momentarily
  }
  public void OnDrag(PointerEventData eventData)
  { //pointereventdata are cursor actions 
    //Ibegindraghandler requires this method

    this.transform.position = eventData.position;

  }
  public void OnEndDrag(PointerEventData eventData)
  { //pointereventdata are cursor actions 
    //Ibegindraghandler requires this method
    this.transform.SetParent(parentReturn);
    // GetComponent<CanvasGroup>().blocksRaycasts = true;
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
