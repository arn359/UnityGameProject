using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class NewMonoBehaviourScript1 : MonoBehaviour
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
                Debug.Log($"--- UI Click Stack Layer Count: {results.Count} ---");
                for (int i = 0; i < results.Count; i++)
                {
                    Debug.Log($"Layer [{i}]: {results[i].gameObject.name} (Component Type: {results[i].gameObject.transform.parent?.name ?? "No Parent"})");
                }
            }
        }
    }
}