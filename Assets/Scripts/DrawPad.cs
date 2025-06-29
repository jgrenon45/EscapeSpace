using TMPro;
using UnityEngine;

public class DrawPad : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;

    [SerializeField] private TMP_Text predictionText;

    [SerializeField] private TMP_Text interactText;

    [SerializeField] private float transitionSpeed = 5.0f;

    [SerializeField] private int codeLength = 3;

    [SerializeField] private OpenableObject objectToOpen;

    [SerializeField] private CodeRoom room;

    [SerializeField] private MeshRenderer[] lights;

    public MNISTEngine mnist;

    private bool isInteracting = false;

    private int numbersGuessed = 0;

    private Draw drawingCanvas;

    // digit recognition
    int predictedNumber;
    float probability;

    public string InteractionText { get => prompt; }

    public bool IsInteractable { get; set; }


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

    public void InRange(bool inRange)
    {
        if (IsInteractable)
        {          
            if (inRange && !isInteracting)
            {
                ToggleText(true, interactText);
            }
            else
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
        if (isInteracting)
        {
            // Get the player drawn texture
            Texture2D texture = drawingCanvas.drawingTexture;

            //Pass it into the neural network model (digit regognition model)
            var probabilityAndIndex = mnist.GetMostLikelyDigitProbability(texture);

            probability = probabilityAndIndex.Item1;
            predictedNumber = probabilityAndIndex.Item2;
            predictionText.text = predictedNumber.ToString();
            if (predictedNumber == room.GetCode()[numbersGuessed])
            {
                // Light up the corresponding light
                lights[numbersGuessed].material.SetColor("_EmissionColor", Color.green);

                numbersGuessed++;

                AudioManager.instance.soundsAudioSource.PlayOneShot(AudioManager.instance.digitSuccess);
                if (numbersGuessed == codeLength)
                {
                    AudioManager.instance.soundsAudioSource.PlayOneShot(AudioManager.instance.codeSuccess);
                    GameObject.FindWithTag("Player").GetComponent<Interactor>().Interact();
                    objectToOpen.Open();
                    room.isCodeSolved = true;
                    DisableInteraction();
                }
            }
            else
            {
                AudioManager.instance.soundsAudioSource.PlayOneShot(AudioManager.instance.codeFail);
            }
            drawingCanvas.ClearTexture();
        }
    }

    public void DisableInteraction()
    {
        ToggleText(false, interactText);
        ToggleText(false, predictionText);
        IsInteractable = false;
        drawingCanvas.ClearTexture();
    }
}
