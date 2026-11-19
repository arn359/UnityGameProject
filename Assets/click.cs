using UnityEngine;
using Yarn.Unity;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class Vase : MonoBehaviour
{
    Vector3 originalScale;

    public AudioSource audioshow;
    public GameObject showobj;
    public AudioSource audioshow2;
    public AudioSource audioshow3;
    public DialogueRunner lineshow;
    
    public string[] nodes;
      

    public int clickCount = 0;      


    public string requireditem;
    public string rightnode;
    public string wrongnode;

    void Awake()
    {
     
        originalScale = transform.localScale;
        if (lineshow == null) return;

        lineshow.AddCommandHandler("ShowThing_" + gameObject.name, ShowThing);
        lineshow.AddCommandHandler("ShowAudio_"+ gameObject.name, ShowAudio);
        lineshow.AddCommandHandler("ShowAudio2_"+ gameObject.name, ShowAudio2);
        
        
    }

   
    void OnMouseDown()
    {

        Debug.Log($"{name} clicked | running={lineshow?.IsDialogueRunning} showing={playerMovement.isShowingobj} invOpen={inventoryManager.inventoryOpen}");
        
        if (lineshow != null && lineshow.IsDialogueRunning) return;
        if (playerMovement.isShowingobj) return; 
        if (inventoryManager.inventoryOpen) return;

        playerMovement player = FindFirstObjectByType<playerMovement>();
        if (player == null)
        {
            Debug.LogError("No player obj with script found");
            return;
        }

        player.MoveandInteract(transform.position, () =>
        {
            
            if (lineshow != null && lineshow.IsDialogueRunning) return;
            if (playerMovement.isShowingobj) return;

            inventoryManager inv = inventoryManager.instance;
            if (inv == null)
            {
                Debug.Log("Instance null, skipping item.");
            }

            if (inv != null && inv.selecteditem != null)
            {
                Item selected = inv.selecteditem;

                
                if (selected.gameObject != null)
                {
                    if (selected.itemName == requireditem)
                       lineshow.StartDialogue(rightnode);
                    else 
                       lineshow.StartDialogue(wrongnode);
                    
                    inv.selecteditem = null;
                    inv.usepanelimage.enabled = false;
                    return;
                }
            }

            if (audioshow != null) audioshow.Play();
            if (lineshow == null || nodes.Length == 0) return;

            
            string[] current;

            
            
            
        
                current = nodes;
        

            if (current == null || current.Length == 0)
            {
                Debug.LogWarning("No nodes configured for current state on: " + name);
                return;
            }

            
            if (clickCount >= current.Length)
                clickCount = current.Length - 1;

            lineshow.StartDialogue(current[clickCount]);
            clickCount++;

            
           
        });
    }
    
    public void ShowAudio()
    {
        if (audioshow2 != null)
        {
            audioshow2.enabled = true;
            if (!audioshow2.isPlaying) audioshow2.Play();
            
        }
        
    }

    public void ShowAudio2()
    {
        if (audioshow3 != null)
        {
            audioshow3.enabled = true;
            if (!audioshow3.isPlaying) audioshow3.Play();
            
        }
        
    }

   public void ShowThing()
{
    if (showobj != null) showobj.SetActive(true);
    playerMovement.isShowingobj = false;
}
   

    

    
}