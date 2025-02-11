using UnityEngine;
using System.Collections;

public class Raycast : MonoBehaviour
{
    [Header("Raycast Info")]
    [SerializeField] private float rayLength = 5f;

    private NoteController noteController;

    private Camera _camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)), transform.forward, out RaycastHit hit, rayLength))
        {
            var readableItem = hit.collider.GetComponent<NoteController>();
            Debug.Log(hit.collider.name);
            if (readableItem != null)
            {
                noteController = readableItem;
                HighlightCrosshair(true);
            }
            else
            {
                ClearNote();
            }
        }
        else
        {
            ClearNote();
        }
        if(noteController != null)
        {            
            //noteController.Interact(GameObject.FindGameObjectWithTag("Player").GetComponent<Interactor>());
        }
    }

    void ClearNote()
    {
        if (noteController != null)
        {
            HighlightCrosshair(false);
            noteController = null;
        }
    }

    void HighlightCrosshair(bool isOn)
    {
        if (isOn)
        {
            //Enable crosshair
        }
        else
        {
            //Disable crosshair
        }
    }
}
