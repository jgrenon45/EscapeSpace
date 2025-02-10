using UnityEngine;

public interface IInteractable 
{
    public string InteractionText { get; }

    public bool IsInteractable { get; }

    public void Interact(Interactor interactor);
}
