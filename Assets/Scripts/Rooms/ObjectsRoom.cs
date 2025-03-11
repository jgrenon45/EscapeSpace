using System.Collections.Generic;
using Unity.Sentis;
using UnityEngine;
using HoloLab.DNN.Classification;
using System.Text.RegularExpressions;
using System.Linq;

public class ObjectsRoom : Room
{
    [SerializeField, Tooltip("Weights")] private ModelAsset weights = null;
    [SerializeField, Tooltip("Label List")] private TextAsset names = null;
    [SerializeField, Tooltip("Apply Softmax")] private bool apply_softmax = true;
    [SerializeField, Tooltip("Mean")] private Vector3 mean = new Vector3(0.485f, 0.456f, 0.406f);
    [SerializeField, Tooltip("Std")] private Vector3 std = new Vector3(0.229f, 0.224f, 0.225f);

    [SerializeField] private GameObject[] possibleObjects;
    [SerializeField] private int sequenceLength;

    private ClassificationModel model;
    private List<string> labels;
    private List<string> objectSequence = new List<string>();

    public delegate void OnSequenceGenerated();
    public OnSequenceGenerated onSequenceGenerated;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        // Create Classification Model
        model = new ClassificationModel(weights);
        if (apply_softmax) { model.ApplySoftmax(); }
        model.ApplyQuantize();
        model.SetInputMean(mean);
        model.SetInputStd(std);

        // Read Label List from Text Asset
        labels = new List<string>(Regex.Split(names.text, "\r\n|\r|\n"));

        //Create Random object sequence to be the "code"
        List<GameObject> tempObjList = possibleObjects.ToList();
        for (int i = 0; i < sequenceLength; i++)
        {
            int randIndex = UnityEngine.Random.Range(0, tempObjList.Count);
            objectSequence.Add(tempObjList[randIndex].GetComponent<GrabbableObject>()._name);
            tempObjList.Remove(tempObjList[randIndex]);
        }
        onSequenceGenerated?.Invoke();
    }
 
    public (ClassificationModel, List<string>) GetModelInfo() { return (model, labels); }

    public List<string> GetObjectSequence() { return objectSequence; }

    private void OnDestroy()
    {
        model?.Dispose();
        model = null;
    }
}
