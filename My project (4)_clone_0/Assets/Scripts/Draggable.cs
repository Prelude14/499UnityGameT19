// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
// public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

// {
//     public Transform parentToReturnTo = null;
//     public Transform placeholderParent = null;

//     GameObject placeholder = null;

//     public void OnBeginDrag(PointerEventData eventData)
//     {
//         Debug.Log("OnBeginDrag");

//         placeholder = new GameObject();
//         placeholder.transform.SetParent(this.transform.parent);
//         LayoutElement le = placeholder.AddComponent<LayoutElement>();
//         le.preferredWidth = this.getComponent<LayoutElement>().preferredWidth;
//         le.preferredHeight = this.getComponent<LayoutElement>().preferredHeight;

//         placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

//         parentToReturnTo = this.transform.parent;
//         placeholderParent = parentToReturnTo;

//         this.transform.SetParent(this.transform.parent.parent);

//         GetComponents<CanvasGroup>().blocksRaycasts = false;
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         this.transform.position = eventData.position;
//         if (placeholder.transform.parent != placeholderParent)
//         {
//             placeholder.transform.SetParent(placeholderParent);
//         }
//         int newSiblingIndex = placeholderParent.childCount;

//         for (int i = 0; i < placeholderParent.childCount; i++)
//         {
//             if (this.transform.position.x < placeholder.getChild(i).position.x)
//             {
//                 newSiblingIndex = i;

//                 if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
//                 {
//                     newSiblingIndex--;
//                 }
//                 break;
//             }

//         }
//         placeholder.transform.SetSiblingIndex(newSiblingIndex);
//     }

//     public void OnEndDrag(PointerEventData eventData)
//     {
//         Debug.Log("On End Drag");

//         this.transform.SetParent(parentToReturnTo);
//         this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
//         GetComponent<CanvasGroup>().blocksRaycasts = true;

//         Destroy(placeholder);
//     }

//     // Start is called before the first frame update
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }
// }
