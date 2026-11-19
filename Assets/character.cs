using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using System.Collections.Generic;

public class character : MonoBehaviour
{
    public Image portraitImage;
    public List<CharacterPortrait> characters;

    [System.Serializable]
    public class CharacterPortrait
    {
        public string characterName;
        public Sprite portrait;
    }

    public void ShowPortrait(string characterName)
    {
        Debug.Log("ShowPortrait called with: " + characterName);
        foreach (var c in characters)
        {
            if (c.characterName == characterName)
            {
                portraitImage.gameObject.SetActive(true);
                portraitImage.sprite = c.portrait;
                portraitImage.enabled = true;
                return;
            }
        }
        Debug.LogWarning("No portrait found for: " + characterName);
        portraitImage.gameObject.SetActive(false); // hide if no match
    }

    public void HidePortrait()
    {
        portraitImage.gameObject.SetActive(false);
    }
}