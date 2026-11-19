using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class NewMonoBehaviourScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                Debug.Log("[UI Click Hit] Your mouse clicked on: " + results[0].gameObject.name, results[0].gameObject);
            }
            else
            {
                Debug.Log("[UI Click Hit] Mouse clicked empty space. No UI element detected under cursor.");
            }
        }
    }
}