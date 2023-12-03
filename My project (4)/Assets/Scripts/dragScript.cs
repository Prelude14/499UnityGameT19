using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dragScript : MonoBehaviour
{
  public GameObject Canvas;
  public GameObject dropZone;
  public GameObject DropZone;
  private GameObject startParent;
  private Vector2 startPos;
  private bool isOverDropZone = false;
  private bool isDragging = false;
  private bool isDraggable = true;
  void Start()
  {
    Canvas = GameObject.Find("Canvas");
    DropZone = GameObject.Find("playPanel");
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    Debug.Log("Colliding");
    isOverDropZone = true;
    dropZone = collision.gameObject;

  }
  private void OnCollisionExit2D(Collision2D collision)
  {
    Debug.Log("Not colliding");
    isOverDropZone = false;
    dropZone = null;
  }
  public void StartDrag()
  {
    if (!isDraggable)
    {
      return;
    }
    startParent = transform.parent.gameObject; //find the parent of this transform and find that game object so it should be hand
    startPos = transform.position;
    isDragging = true;
  }
  public void EndDrag()
  {
    if (!isDraggable)
    {
      return;
    }
    isDragging = false;
    if (isOverDropZone)
    {
      transform.SetParent(dropZone.transform, false);
      isDraggable = false;
    }
    else
    {
      transform.position = startPos;
      transform.SetParent(startParent.transform, false);

    }
  }

  // Update is called once per frame
  void Update()
  {
    if (isDragging)
    {
      transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
      transform.SetParent(Canvas.transform, true);
    }
  }
}
