using UnityEngine;

public class CodeRoom : Room
{   
    protected int[] code;

    public bool isCodeSolved = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();    
        code = GenerateCode(3);
    }

    protected int[] GenerateCode(int codeLength)
    {
        if (GameManager.instance.debugModeOn)
        {
            code = GameManager.instance.defaultCode;
        }
        else
        {
            code = new int[codeLength];
            for (int i = 0; i < codeLength; i++)
            {
                code[i] = Random.Range(0, 10);
            }
        }
        return code;
    }

    public string GetCodeToString()
    {
        string codeString = "";
        for (int i = 0; i < code.Length; i++)
        {
            codeString += code[i].ToString();
        }
        return codeString;
    }

    public int[] GetCode() { return code; }

}
