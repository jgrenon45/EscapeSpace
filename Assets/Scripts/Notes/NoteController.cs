using StarterAssets;
using System;
using TMPro;
using UnityEngine;

public class NoteController : MonoBehaviour, IInteractable
{
    [SerializeField] protected Room room;

    [Header("UI Text")]
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextAreaUI;
    [SerializeField] protected TMP_Text noteCodeAreaUI;
    [SerializeField] protected TMP_Text interactText;
    [SerializeField] protected string prompt;

    [Space(10)]
    [SerializeField][TextArea] protected string noteText;

    private bool isOpen = false;

    public string InteractionText { get => prompt; }

    public bool IsInteractable { get; set; }

    public void Interact(Interactor interactor)
    {
        if (IsInteractable)
        {
            if (!isOpen)
            {
                ShowNote();

            }
            else
            {
                HideNote();
            }
        }
    }

    public void InRange(bool inRange)
    {
        if (IsInteractable)
        {
            GetComponent<Renderer>().materials[0].SetFloat("_isEnabled", Convert.ToInt32(inRange));
            ToggleText(inRange, interactText);
        }
    }

    protected virtual void ShowNote()
    {
        noteTextAreaUI.text = noteText;
        noteCanvas.SetActive(true);
        isOpen = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().DisableInput();
        AudioManager.instance.soundsAudioSource.PlayOneShot(AudioManager.instance.noteOpen);
    }

    private void HideNote()
    {
        noteCanvas.SetActive(false);
        isOpen = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().EnableInput();
    }

    private void Start()
    {
        IsInteractable = true;
        noteCanvas.SetActive(false);
        interactText.text = InteractionText;
        ToggleText(false, interactText);
    }

    public void DisableInteraction()
    {
        IsInteractable = false;
    }

    private void ToggleText(bool isVisible, TMP_Text textToToggle)
    {
        textToToggle.enabled = isVisible;
    }
}
