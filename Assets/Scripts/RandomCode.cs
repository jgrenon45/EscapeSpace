using UnityEngine;

public class RandomCode : MonoBehaviour
{
    [SerializeField]
    private int codeLength;

    public int[] code;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        code = new int[codeLength];
        for (int i = 0; i < codeLength; i++)
        {
            code[i] = Random.Range(0, 9);
        }
    }

    
}
