using System;
using UnityEngine;

public class KeyCard : MonoBehaviour, IInteractable
{
    public string InteractionText { get; }

    public bool IsInteractable { get; set; }

    public void InRange(bool inRange)
    {
        GetComponent<Renderer>().materials[0].SetFloat("_isEnabled", Convert.ToInt32(inRange));
        IsInteractable = inRange;
    }

    public void Interact(Interactor interactor)
    {
        if (IsInteractable)
        {
            if (GameObject.Find("BottleRoom").GetComponent<BottleRoom>().isCodeSolved)
            {
                interactor.hasKeyCard = true;
                Destroy(gameObject);
            }
        }
    }
}

    