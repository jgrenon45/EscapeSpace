using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NoteController : MonoBehaviour, IInteractable
{
    [Header("UI Text")]
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextAreaUI;

    [Space(10)]
    [SerializeField][TextArea] private string noteText;

    [Space(10)]
    [SerializeField] private UnityEvent openEvent;

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
        
    }

    private void ShowNote()
    {
        noteTextAreaUI.text = noteText;
        noteCanvas.SetActive(true);
        openEvent.Invoke();
        isOpen = true;
    }

    private void HideNote()
    {
        noteCanvas.SetActive(false);
        isOpen = false;
    }

    private void Start()
    {
        IsInteractable = true;
        noteCanvas.SetActive(false);
    }
}
