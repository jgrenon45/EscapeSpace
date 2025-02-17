using StarterAssets;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NoteController : MonoBehaviour, IInteractable
{
    [SerializeField] protected CodeRoom room;

    [Header("UI Text")]
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextAreaUI;
    [SerializeField] protected TMP_Text noteCodeAreaUI;

    [Space(10)]
    [SerializeField][TextArea] protected string noteText;

    private bool isOpen = false;

    public string InteractionText => throw new System.NotImplementedException();

    public bool IsInteractable { get; set; }

    public void Interact(Interactor interactor)
    {
        if (IsInteractable)
        {
            if(!isOpen)
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
        GetComponent<Renderer>().materials[0].SetFloat("_isEnabled", Convert.ToInt32(inRange));
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
    }

    protected virtual void OnCodeGenerated()
    {
        if (noteCodeAreaUI)
        {
            noteCodeAreaUI.text = room.GetCodeToString();
        }
    }

    private void OnEnable()
    {
        room.onCodeGenerated += OnCodeGenerated;
    }
}
