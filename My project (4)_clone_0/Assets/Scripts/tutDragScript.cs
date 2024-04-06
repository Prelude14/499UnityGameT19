using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutDragScript : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject dropZone;
    public GameObject DropZone;
    private GameObject startParent;
    private Vector2 startPos;
    private bool isOverDropZone = false;
    private bool isDragging = false;
    public static bool isDraggable = true;
    public GameObject placeholder = null;

    void Start()
    {
        Canvas = GameObject.Find("Canvas");
        DropZone = GameObject.Find("tutPlayPanel");
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
            Debug.Log("not draggable");
            return;
        }
        startParent = transform.parent.gameObject; //find the parent of this transform and find that game object so it should be hand
        startPos = transform.position;
        isDragging = true;
        Debug.Log("DRAGGING!");
    }
    public void EndDrag()
    {


        isDragging = false;
        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
            isDraggable = false;

        }
        else
        {
            if (startParent == null)
            {
                transform.position = startPos;
                startParent = transform.parent.gameObject;
                return;
            }
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
