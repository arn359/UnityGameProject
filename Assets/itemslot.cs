using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class itemslot : MonoBehaviour
{
    public Image icon;
    public Item currentitem;

    public Image itempic;
    public TextMeshProUGUI describetext;

    public void SetItem(Item item)
    {
        currentitem = item;
        if (icon != null)
        {
        
        icon.sprite = item.icon;
        icon.enabled = true;
        Color c = icon.color;
        c.a = 1f;
        icon.color = c;
        Canvas.ForceUpdateCanvases();
        if (icon.gameObject.GetComponent<RectTransform>() != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(icon.gameObject.GetComponent<RectTransform>());
        }
       }
    }
        
       

    
    public void OnClick()
    {
        if (currentitem == null)
           return;

        
        inventoryManager.instance.inspectedItem = currentitem;

        
        itempic.sprite = currentitem.icon;
        itempic.enabled = true;
        describetext.text = currentitem.description;
    }

    
    public void useitem()
    {
        if (inventoryManager.instance.inspectedItem == null)
            return;

        
        Item item = inventoryManager.instance.inspectedItem;

        bool stillininventory = false;
        foreach (itemslot slot in inventoryManager.instance.slots)
        {
            if (slot != null && slot.currentitem == item)
            {
                stillininventory = true;
                break;
            }
        }
        if (!stillininventory)
        {
            inventoryManager.instance.usepanelimage.enabled = false;
            return;
        }
        inventoryManager.instance.selecteditem = item;

        
        inventoryManager.instance.usepanelimage.sprite = item.icon;
        inventoryManager.instance.usepanelimage.enabled = true;

        
inventoryManager.instance.ToggleInventory();  
    }
}