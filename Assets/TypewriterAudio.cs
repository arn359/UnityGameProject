using UnityEngine;
using TMPro;

public class TypewriterAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip typingSound;
    [Range(0f, 1f)] [SerializeField] private float volume = 0.5f;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI textComponent;

    private AudioSource audioSource;
    private int lastVisibleCharacterCount = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (textComponent == null)
        {
            textComponent = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        if (textComponent == null || audioSource == null || typingSound == null) return;

        
        if (string.IsNullOrEmpty(textComponent.text))
        {
            audioSource.Stop();
            lastVisibleCharacterCount = 0;
            return;
        }

        int currentVisibleCharacters = textComponent.maxVisibleCharacters;

        
        if (currentVisibleCharacters >= textComponent.text.Length)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            lastVisibleCharacterCount = currentVisibleCharacters;
            return;
        }

        
        if (currentVisibleCharacters > lastVisibleCharacterCount)
        {
            if (currentVisibleCharacters - 1 < textComponent.text.Length)
            {
                char lastTypedChar = textComponent.text[currentVisibleCharacters - 1];
                
                if (lastTypedChar != ' ')
                {
                    PlayTypewriterSound();
                }
            }
        }

        lastVisibleCharacterCount = currentVisibleCharacters;
    }

    private void PlayTypewriterSound()
    {
        audioSource.pitch = 1.0f; 
        audioSource.PlayOneShot(typingSound, volume);
    }
}