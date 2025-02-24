using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Interactor : MonoBehaviour
{
    public bool hasKeyCard = false;

    [SerializeField] private LayerMask interactionLayer;

    [SerializeField] private CinemachineVirtualCamera playerCamera;

    [Header("Raycast Info")]
    [SerializeField] private float rayLength = 5f;

    private GameObject interactableActor;

    private bool isInteracting;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red); // Debug visualization

        if (Physics.Raycast(ray, out RaycastHit hit, rayLength, interactionLayer))
        {
            var interactableObject = hit.collider.gameObject;
            if (interactableObject.GetComponent<IInteractable>() != null)
            {
                interactableActor = interactableObject;
                interactableActor.GetComponent<IInteractable>().InRange(true);
            }
            else
            {
                interactableActor.GetComponent<IInteractable>().InRange(false);
                interactableActor = null;
            }
        }
        else
        {
            if (interactableActor != null)
            {
                interactableActor.GetComponent<IInteractable>().InRange(false);
                interactableActor = null;
            }
        }       
    }

    public void Interact()
    {
        if(interactableActor)
        {
            //If actor is interactable, interact with it
            if (interactableActor.GetComponent<IInteractable>().IsInteractable)
            {
                CinemachineVirtualCamera targetCamera = interactableActor.GetComponentInChildren<CinemachineVirtualCamera>();
                //Change Camera target and disable player input only if the interactable object has a camera
                if (targetCamera)
                {
                    FirstPersonController fpc = GetComponentInParent<FirstPersonController>();
                    if (!isInteracting)
                    {
                        isInteracting = true;
                        fpc.DisableInput();

                        //enable Interact Camera mode
                        targetCamera.enabled = true;
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                    }
                    else
                    {
                        isInteracting = false;
                        fpc.EnableInput();

                        //disable Interact Camera mode
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
