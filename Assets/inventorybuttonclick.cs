using UnityEngine;

public class inventorybuttonclick : MonoBehaviour
{
    
    public void Click()
    {
        Debug.Log("[inv] button clicked");

        if (inventoryManager.instance != null)
            inventoryManager.instance.ToggleInventory();
        else
            Debug.LogError("[inv] inventoryManager.instance is NULL");
    }
}