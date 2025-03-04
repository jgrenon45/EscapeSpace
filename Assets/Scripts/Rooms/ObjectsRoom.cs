using System.Collections.Generic;
using Unity.Sentis;
using UnityEngine;
using UnityEngine.UI;
using HoloLab.DNN.Classification;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

public class ObjectsRoom : Room
{
    [SerializeField, Tooltip("Weights")] private ModelAsset weights = null;
    [SerializeField, Tooltip("Label List")] private TextAsset names = null;
    [SerializeField, Tooltip("Apply Softmax")] private bool apply_softmax = true;
    [SerializeField, Tooltip("Mean")] private Vector3 mean = new Vector3(0.485f, 0.456f, 0.406f);
    [SerializeField, Tooltip("Std")] private Vector3 std = new Vector3(0.229f, 0.224f, 0.225f);
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] RawImage testTexture;

    private ClassificationModel model;
    private List<string> labels;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create Classification Model
        model = new ClassificationModel(weights);
        if (apply_softmax) { model.ApplySoftmax(); }
        model.ApplyQuantize();
        model.SetInputMean(mean);
        model.SetInputStd(std);

        // Read Label List from Text Asset
        labels = new List<string>(Regex.Split(names.text, "\r\n|\r|\n"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            CheckObject();
        }
    }

    void CheckObject()
    {
        // Get Texture from Raw Image
        var input_texture = testTexture.texture as Texture2D;
        if (input_texture == null)
        {
            return;
        }

        // Crop Texture from Center
        var croped_texture = HoloLab.DNN.Classification.Crop.CenterCrop(input_texture);

        // Classify
        (var class_id, var score) = model.Classify(croped_texture);

        // Show Class on Unity Console
        Debug.Log($"{class_id} {labels[class_id]} ({score:F3})");

        // Destroy Texture
        Destroy(croped_texture);
    }

    private void OnDestroy()
    {
        model?.Dispose();
        model = null;
    }
}
