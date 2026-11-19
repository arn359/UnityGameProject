using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Yarn.Unity;

public class Item : MonoBehaviour
{
    public string itemName;
    public string describe;
    public Sprite icon;
    public string description;

    public DialogueRunner lineshow;
    public string dianode;

    static HashSet<string> taken = new HashSet<string>();   
    bool persistent;

    void OnEnable()  { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        if (!persistent) return;
        lineshow = FindFirstObjectByType<DialogueRunner>();
        Register();
    }

    void Start()
    {
        
        if (!persistent && taken.Contains(itemName))
        {
            Destroy(gameObject);
            return;
        }
        Register();
    }

    void Register()
    {
        if (lineshow == null) return;
        lineshow.AddCommandHandler("AddItem_" + itemName, AddItemintoinv);
        lineshow.AddCommandHandler("remove" + itemName, removeitem);
    }

    void OnMouseDown()
    {
        if (inventoryManager.inventoryOpen) return;
        if (lineshow != null && lineshow.IsDialogueRunning) return;
        if (lineshow != null) lineshow.StartDialogue(dianode);
    }

    public void AddItemintoinv()
    {
        if (inventoryManager.instance == null)
        {
            Debug.LogError("Cannot pick up item because inventoryManager.instance is missing!");
            return;
        }

        inventoryManager.instance.AddItemintoinv(this);

        if (GetComponent<SpriteRenderer>() != null) GetComponent<SpriteRenderer>().enabled = false;
        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false;

        taken.Add(itemName);
        persistent = true;
        transform.SetParent(null);          
        DontDestroyOnLoad(gameObject);
    }

    public void removeitem()
    {
        if (inventoryManager.instance != null)
            inventoryManager.instance.removeitemfromslot(this);

        if (persistent) Destroy(gameObject);   
    }
}