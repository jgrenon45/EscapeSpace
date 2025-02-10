using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DrawPad : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string prompt;

    [SerializeField]
    private TMP_Text predictionText;

    [SerializeField]
    private float transitionSpeed = 5.0f;

    [SerializeField]
    private int codeLength = 3;

    [SerializeField]
    private AudioClip codeSuccess;

    [SerializeField]
    private AudioClip codeFailed;

    [SerializeField]
    private AudioClip digitSuccess;

    [SerializeField]
    private Door doorToOpen;

    [SerializeField]
    private CodeRoom room;

    public MNISTEngine mnist;

    private TMP_Text interactText;

    private bool isInteracting = false;

    private int numbersGuessed = 0;

    private Draw drawingCanvas;

    // digit recognition
    int predictedNumber;
    float probability;

    public string InteractionText { get => prompt; }

    public bool IsInteractable { get; private set; }

    public void Interact(Interactor interactor)
    {
        if (IsInteractable)
        {
            if (!isInteracting)
            {
                isInteracting = true;
                GetComponentInChildren<Draw>().canDraw = true;
                ToggleText(false, interactText);
                ToggleText(true, predictionText);

            }
            else
            {
                isInteracting = false;
                GetComponentInChildren<Draw>().canDraw = false;
                ToggleText(true, interactText);
                ToggleText(false, predictionText);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Toggle the interact text when the player is in range
        if (IsInteractable)
        {
            if (other.GetComponent<Interactor>() != null)
            {
                ToggleText(true, interactText);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Toggle the interact text when the player is out of range
        if (IsInteractable)
        {
            if (other.GetComponent<Interactor>() != null)
            {
                ToggleText(false, interactText);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsInteractable = true;
        drawingCanvas = GetComponentInChildren<Draw>();
        interactText = GetComponentInChildren<TMP_Text>();
        interactText.text = InteractionText;
        predictionText.text = "?";
        ToggleText(false, predictionText);
        ToggleText(false, interactText);
    }

    private void ToggleText(bool isVisible, TMP_Text textToToggle)
    {
        textToToggle.enabled = isVisible;
    }

    public void CheckDrawing()
    {
        // Get the player drawn texture
        Texture2D texture = drawingCanvas.drawingTexture;

        //Pass it into the neural network model (digit regognition model)
        var probabilityAndIndex = mnist.GetMostLikelyDigitProbability(texture);

        probability = probabilityAndIndex.Item1;
        predictedNumber = probabilityAndIndex.Item2;
        predictionText.text = predictedNumber.ToString();
        if(predictedNumber == room.code[numbersGuessed])
        {
            numbersGuessed++;
            GetComponent<AudioSource>().PlayOneShot(digitSuccess);
            if (numbersGuessed == codeLength)
            {
                GetComponent<AudioSource>().PlayOneShot(codeSuccess);
                GameObject.FindWithTag("Player").GetComponent<Interactor>().Interact();
                doorToOpen.Open();
                IsInteractable = false;
                ToggleText(false, interactText);
                ToggleText(false, predictionText);
            }

        }
        else
        {
            numbersGuessed = 0;
            GetComponent<AudioSource>().PlayOneShot(codeFailed);
        }
        drawingCanvas.ClearTexture();
    }
}
