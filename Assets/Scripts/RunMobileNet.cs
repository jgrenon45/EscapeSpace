using System.Collections.Generic;
using Unity.Sentis;
using UnityEngine;
using System.IO;
/*
 *  MovileNetV2 Inference Script
 *  ============================
 *  
 *  Place this script on the Main Camera
 *  
 *  Drag an image to the inputImage field
 *  
 *  When run the prediction of what the image is will output to the console window.
 *  You can modify the script to make it do something more interesting.
 * 
 */


public class RunMobileNet : MonoBehaviour
{
    //draw the sentis file here:
    public ModelAsset modelAsset;

    const string modelName = "mobilenet_v2.sentis";

    //The image to classify here:
    public Texture2D inputImage;

    //Link class_desc.txt here:
    public TextAsset labelsAsset;

    //All images are resized to these values to go into the model
    const int imageHeight = 224;
    const int imageWidth = 224;

    const BackendType backend = BackendType.GPUCompute;

    // input tensor
    Tensor<float> inputTensor = null;

    private Worker engine;

    // This small model works just as fast on the CPU as well as the GPU:
    static BackendType backendType = BackendType.GPUCompute;

    private string[] labels;

    //Used to normalise the input RGB values
    Tensor<float> mulRGB = new Tensor<float>(new TensorShape(1, 3, 1, 1), new float[] { 1 / 0.229f, 1 / 0.224f, 1 / 0.225f });
    Tensor<float> shiftRGB = new Tensor<float>(new TensorShape(1, 3, 1, 1), new float[] { 0.485f, 0.456f, 0.406f });

    void Start()
    {
        //Parse neural net labels
        labels = labelsAsset.text.Split('\n');

        //Load model from file or asset
        //var model = ModelLoader.Load(Path.Join(Application.streamingAssetsPath, modelName));
        var model = ModelLoader.Load(modelAsset);

        inputTensor = new Tensor<float>(new TensorShape(1, 3, imageWidth, imageWidth));

        // Create a new computation graph
        var graph = new FunctionalGraph();

        // Define input node (shape should match your model's expected input)
        var input = graph.AddInput(DataType.Float, model.inputs[0].shape);

        // Normalize RGB values
        var normalized = NormaliseRGB(graph, input);

        // Forward pass through the model
        var output = Functional.Forward(model, normalized)[0];

        // Process output: Get max probability and class ID
        var probabilities = Functional.Softmax(output);
        var indexOfMaxProba = Functional.ArgMax(probabilities, -1, false);

        // Compile the graph with outputs
        var model2 = graph.Compile(probabilities, indexOfMaxProba);

        //Setup the engine to run the model
        engine = new Worker(model2, backendType);

        ExecuteML(inputImage);
    }

    public void ExecuteML(Texture2D texture)
    {
        // Convert the texture into a tensor, it has width=W, height=W, and channels=1:
        TextureConverter.ToTensor(texture, inputTensor, new TextureTransform());

        //Execute neural net
        engine.Schedule(inputTensor);

        //Read output tensor
        var probability = engine.PeekOutput("output_0") as Tensor<float>;
        var item = engine.PeekOutput("output_1") as Tensor<int>;

        // Readback tensors from GPU to CPU
        Tensor<int> itemCPU = item.ReadbackAndClone();
        Tensor<float> probabilityCPU = probability.ReadbackAndClone();

        // Now access values safely
        var ID = itemCPU[0];
        var accuracy = probabilityCPU[0];

        //The result is output to the console window
        int percent = Mathf.FloorToInt(accuracy * 100f + 0.5f);
        Debug.Log($"Prediction: {labels[ID]} {percent}﹪");

        //Clean memory
        Resources.UnloadUnusedAssets();
    }

    //This scales and shifts the RGB values for input into the model
    FunctionalTensor NormaliseRGB(FunctionalGraph graph, FunctionalTensor image)
    {
        return (image - Functional.Constant(shiftRGB)) * Functional.Constant(mulRGB);
    }
    
    private void OnDestroy()
    {
        mulRGB?.Dispose();
        shiftRGB?.Dispose();
        engine?.Dispose();      
    }
}
