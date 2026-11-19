using UnityEngine;
using TMPro;
using Yarn.Unity;

public class dateinput : MonoBehaviour
{
    public DialogueRunner runner;
    public TMP_InputField dateField;
    public TMP_InputField nameField; 
    public string nextNode;

    void OnEnable()  { inventoryManager.inventoryOpen = true; }
    void OnDisable() { inventoryManager.inventoryOpen = false; }

    public void Submit()
    {
        Debug.Log("[input] Submit clicked");
        string typedDate = dateField != null ? dateField.text : "";
        string typedName = nameField != null ? nameField.text : "";

        if (runner == null) { Debug.LogError("[input] Runner slot is empty"); return; }

        var vs = runner.VariableStorage as InMemoryVariableStorage;
        if (vs != null) 
        {
            vs.SetValue("$player_date", typedDate);
            vs.SetValue("$name", typedName); 
            Debug.Log($"[input] Saved date: {typedDate}, name: {typedName}");
        }

        if (!string.IsNullOrEmpty(nextNode)) 
        {
            runner.StartDialogue(nextNode);
        }

        gameObject.SetActive(false);   
    }
}