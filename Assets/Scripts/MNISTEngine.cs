using Unity.Sentis;
using UnityEngine;

public class MNISTEngine : MonoBehaviour
{
    public ModelAsset mnistONNX;
    public TextAsset labelsAsset;
    public Texture2D testImage;

    // engine type
    Worker engine;

    // This small model works just as fast on the CPU as well as the GPU:
    static BackendType backendType = BackendType.GPUCompute;

    // width and height of the image:
    [SerializeField] private int imageWidth = 28;

    // input tensor
    Tensor<float> inputTensor = null;

    Camera lookCamera;

    private string[] labels;

    void Start()
    {
        // load the neural network model from the asset:
        Model model = ModelLoader.Load(mnistONNX);

        if (labelsAsset)
        {
            //Parse neural net labels
            labels = labelsAsset.text.Split('\n');
        }

        var graph = new FunctionalGraph();
        inputTensor = new Tensor<float>(new TensorShape(1, 1, imageWidth, imageWidth));
        var input = graph.AddInput(DataType.Float, new TensorShape(1, 1, imageWidth, imageWidth));
        var outputs = Functional.Forward(model, input);
        var result = outputs[0];

        // Convert the result to probabilities between 0..1 using the softmax function
        var probabilities = Functional.Softmax(result);
        var indexOfMaxProba = Functional.ArgMax(probabilities, -1, false);
        model = graph.Compile(probabilities, indexOfMaxProba);

        // create the neural network engine:
        engine = new Worker(model, backendType);

        //The camera which we'll be using to calculate the rays on the image:
        lookCamera = Camera.main;
    }

    // Sends the image to the neural network model and returns the probability that the image is each particular digit.
    public (float, int) GetMostLikelyDigitProbability(Texture2D drawableTexture)
    {
        // Convert the texture into a tensor, it has width=W, height=W, and channels=1:
        TextureConverter.ToTensor(drawableTexture, inputTensor, new TextureTransform());

        // run the neural network:
        engine.Schedule(inputTensor);

        // We get a reference to the outputs of the neural network. Make the result from the GPU readable on the CPU
        using var probabilities = (engine.PeekOutput(0) as Tensor<float>).ReadbackAndClone();
        using var indexOfMaxProba = (engine.PeekOutput(1) as Tensor<int>).ReadbackAndClone();

        var predictedNumber = indexOfMaxProba[0];
        var probability = probabilities[predictedNumber];

        return (probability, predictedNumber);
    }

    public void GetMostLikelyObject(Texture2D texture)
    {
        GameObject.Find("RenderTex").GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", testImage);
        // Convert the texture into a tensor, it has width=W, height=W, and channels=1:
        TextureConverter.ToTensor(testImage, inputTensor, new TextureTransform());

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

    // Clean up all our resources at the end of the session so we don't leave anything on the GPU or in memory:
    private void OnDestroy()
    {
        inputTensor?.Dispose();
        engine?.Dispose();
    }
}
