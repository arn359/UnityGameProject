using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class inventoryManager : MonoBehaviour
{
    public Image itempic;
    public TextMeshProUGUI describetext;

    public static bool inventoryOpen;
    public GameObject inventorymenu;

    private static inventoryManager _instance;
    public static inventoryManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<inventoryManager>(FindObjectsInactive.Include);
            return _instance;
        }
    }

    public itemslot[] slots;

    [HideInInspector] public Item inspectedItem;
    public Item selecteditem;
    public Image usepanelimage;

    void OnEnable()  { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

   void OnSceneLoaded(Scene s, LoadSceneMode m)
{
    
    if (inventorymenu == null)
    {
        GameObject menuObj = GameObject.Find("inventorymenu");
        if (menuObj != null) inventorymenu = menuObj;
    }

    
    if (inventorymenu != null)
    {
        slots = inventorymenu.GetComponentsInChildren<itemslot>(true);
    }

    
    Button invButton = GameObject.Find("InventoryButton")?.GetComponent<Button>(); 
    if (invButton != null)
    {
        invButton.onClick.RemoveAllListeners();
        invButton.onClick.AddListener(ToggleInventory);
    }
}

    void Awake()
    {
        
        if (_instance != null && _instance != this)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        if (inventorymenu != null) inventorymenu.SetActive(false);
        inventoryOpen = false;

        if (slots != null)
        {
            foreach (itemslot slot in slots)
            {
                if (slot == null) continue;
                slot.currentitem = null;
                if (slot.icon != null) slot.icon.enabled = false;
            }
        }
    }

    public void closeinventory()
    {
        if (inventorymenu != null) inventorymenu.SetActive(false);
        inventoryOpen = false;
    }

    public void AddItemintoinv(Item item)
    {
        Debug.Log($"AddItem {item.itemName} | slots={(slots == null ? -1 : slots.Length)} | icon={(item.icon == null ? "NULL" : item.icon.name)}");
        if (slots == null) return;
        foreach (itemslot slot in slots)
        {
            if (slot == null) continue;
            if (slot.currentitem == null)
            {
                slot.SetItem(item);
                Debug.Log("Put in slot: " + slot.name);
                return;
            }
        }
        Debug.LogWarning("No free slot!");
    }

    public void removeitemfromslot(Item item)
    {
        foreach (itemslot slot in slots)
        {
            if (slot == null) continue;
            if (slot.currentitem == item)
            {
                slot.currentitem = null;
                slot.icon.sprite = null;
                slot.icon.enabled = false;

                if (itempic != null) itempic.enabled = false;
                if (describetext != null) describetext.text = "";

                if (selecteditem == item) selecteditem = null;
                if (inspectedItem == item) inspectedItem = null;

                if (usepanelimage != null)
                {
                    usepanelimage.sprite = null;
                    usepanelimage.enabled = false;
                }
                return;   
            }
        }
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.X)) ToggleInventory();
}

public void ToggleInventory()
{
    Debug.Log(">>> [TRACE] ToggleInventory() called on inventoryManager.");
    if (inventorymenu == null) { Debug.Log("Menu not assigned"); return; }

    inventorymenu.SetActive(!inventorymenu.activeSelf);
    inventoryOpen = inventorymenu.activeSelf;

    CanvasGroup dialogueGroup = FindFirstObjectByType<Yarn.Unity.DialogueRunner>()?.GetComponentInParent<CanvasGroup>();
    if (dialogueGroup != null)
    {
        dialogueGroup.blocksRaycasts = !inventoryOpen;
        dialogueGroup.interactable = !inventoryOpen;
    }
}
}