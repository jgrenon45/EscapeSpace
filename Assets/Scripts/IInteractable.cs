using UnityEngine;

public interface IInteractable 
{
    public string InteractionText { get; }

    public bool IsInteractable { get; set; }

    public void Interact(Interactor interactor);

    /// <summary>
    /// Called when the interactor raycast is in range of the interactable object
    /// </summary>
    /// <param name="inRange">If the object is in range or not</param>
    public void InRange(bool inRange);

    public void DisableInteraction();
}
