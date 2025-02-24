using System;
using TMPro;
using UnityEngine;

public class KeyPad : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;

    [SerializeField] private OpenableObject objectToOpen;

    [SerializeField] private TMP_Text interactText;

    public string InteractionText { get => prompt; }

    public bool IsInteractable { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsInteractable = true;
        interactText.text = InteractionText;
        interactText.enabled = false;
    }

    public void InRange(bool inRange)
    {
        if (IsInteractable)
        {
            GetComponent<Renderer>().materials[0].SetFloat("_isEnabled", Convert.ToInt32(inRange));
            interactText.enabled = inRange;
            if (!inRange)
            {
                interactText.text = InteractionText;
            }
        }
    }

    public void Interact(Interactor interactor)
    {
        if (IsInteractable)
        {
            if (interactor.hasKeyCard)
            {
                objectToOpen.Open();
                interactor.hasKeyCard = false;
                DisableInteraction();
                AudioManager.instance.soundsAudioSource.PlayOneShot(AudioManager.instance.codeSuccess);
            }
            else
            {
                AudioManager.instance.soundsAudioSource.PlayOneShot(AudioManager.instance.codeFail);
                interactText.text = "Nécessite une clé pour ouvrir";
            }
        }
    }

    public void DisableInteraction()
    {
        interactText.enabled = false;
        IsInteractable = false;
        GetComponent<Renderer>().materials[0].SetFloat("_isEnabled", Convert.ToInt32(false));
    }

    
}
