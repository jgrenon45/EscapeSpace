using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Interactor : MonoBehaviour
{

    [SerializeField] private LayerMask interactionLayer;

    [SerializeField] private CinemachineVirtualCamera playerCamera;

    [Header("Raycast Info")]
    [SerializeField] private float rayLength = 5f;

    private GameObject interactableActor;

    private bool isInteracting;

    private void Update()
    {
        if (Physics.Raycast(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)), transform.forward, out RaycastHit hit, rayLength, interactionLayer))
        {
            Debug.Log(hit.collider.name);
            var interactableObject = hit.collider.gameObject;
            if (interactableObject.GetComponent<IInteractable>() != null)
            {
                interactableActor = interactableObject;
                interactableActor.GetComponent<IInteractable>().InRange(true);
                //Highlight object
            }
            else
            {
                interactableActor.GetComponent<IInteractable>().InRange(false);
                interactableActor = null;
            }
        }
        else
        {
            interactableActor = null;
        }
        if (interactableActor != null)
        {
            //noteController.Interact(GameObject.FindGameObjectWithTag("Player").GetComponent<Interactor>());
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
