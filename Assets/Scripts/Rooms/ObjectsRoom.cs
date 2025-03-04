using System.Collections.Generic;
using Unity.Sentis;
using UnityEngine;
using HoloLab.DNN.Classification;
using System.Text.RegularExpressions;
using System.IO;

public class ObjectsRoom : Room
{
    [SerializeField, Tooltip("Weights")] private ModelAsset weights = null;
    [SerializeField, Tooltip("Label List")] private TextAsset names = null;
    [SerializeField, Tooltip("Apply Softmax")] private bool apply_softmax = true;
    [SerializeField, Tooltip("Mean")] private Vector3 mean = new Vector3(0.485f, 0.456f, 0.406f);
    [SerializeField, Tooltip("Std")] private Vector3 std = new Vector3(0.229f, 0.224f, 0.225f);
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] Texture2D testTexture;

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
        // Get Texture from RenderTexture
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);

        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        if (tex == null)
        {
            return;
        }

        // Crop Texture from Center
        var croped_texture = HoloLab.DNN.Classification.Crop.CenterCrop(tex);

        /********TEST********/
        // Create a RenderTexture with the same size as the Texture2D
        RenderTexture test = new RenderTexture(croped_texture.width, croped_texture.height, 0);
        test.Create();

        // Copy the Texture2D into the RenderTexture
        Graphics.Blit(croped_texture, test);

        RenderTexture.active = renderTexture;
        /********************/

        // Classify
        (var class_id, var score) = model.Classify(croped_texture);

        // Show Class on Unity Console
        Debug.Log($"{class_id} {labels[class_id]} ({score:F3})");

        // Convert to PNG and save
        byte[] bytes = croped_texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Screenshot.png", bytes);

        // Destroy Texture
        Destroy(croped_texture);
    }

    private void OnDestroy()
    {
        model?.Dispose();
        model = null;
    }
}
