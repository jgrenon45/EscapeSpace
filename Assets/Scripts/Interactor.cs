using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Interactor : MonoBehaviour
{

    [SerializeField]
    private LayerMask interactionLayer;

    [SerializeField]
    private SphereCollider interactionCollider;

    [SerializeField]
    private CinemachineVirtualCamera playerCamera;

    private GameObject interactableActor;

    private bool isInteracting;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IInteractable>() != null)
        {
            interactableActor = other.gameObject;
            //Highlight object
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactableActor = null;
    }

    public void Interact()
    {
        if(interactableActor)
        {
            //If actor is interactable, interact with it
            if (interactableActor.GetComponent<IInteractable>().IsInteractable)
            {
                CinemachineVirtualCamera targetCamera = interactableActor.GetComponentInChildren<CinemachineVirtualCamera>();
                if (!isInteracting)
                {
                    isInteracting = true;
                    GetComponentInParent<FirstPersonController>().DisableInput();
                    if (targetCamera)
                    {
                        //enable Interact Camera mode
                        targetCamera.enabled = true;
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                    }
                }
                else
                {
                    //disable Interact Camera mode
                    isInteracting = false;
                    GetComponentInParent<FirstPersonController>().EnableInput();
                    if (targetCamera)
                    {
                        targetCamera.enabled = false;
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                }
                interactableActor.GetComponent<IInteractable>().Interact(this);
            }
        }
    }
}
