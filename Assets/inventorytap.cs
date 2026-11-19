using UnityEngine;

public class inventorytap : MonoBehaviour
{
    void OnMouseDown()
    {
        if (inventoryManager.instance != null)
            inventoryManager.instance.ToggleInventory();
    }
}