using Mono.Cecil.Cil;
using System.Collections.Generic;
using UnityEngine;

public class CodeRoom : Room
{   
    protected int[] code;

    public delegate void OnCodeGenerated();
    public OnCodeGenerated onCodeGenerated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        code = GenerateRandomCode(3);
    }

    //private void SetRandomPositions()
    //{
    //    Vector3 previousPos = Vector3.zero;
    //    for (int i = 0; i < code.Length; i++)
    //    {
    //        Vector3 newPos = randomPositions[Random.Range(0, randomPositions.Length)].position;
    //        while(newPos == previousPos)
    //        {
    //            newPos = randomPositions[Random.Range(0, randomPositions.Length)].position;
    //        }
    //        Instantiate(numbersPrefabs[code[i]], newPos, Quaternion.identity);
    //        previousPos = newPos;
    //    }
    //}

    protected int[] GenerateRandomCode(int codeLength)
    {
        code = new int[codeLength];
        for (int i = 0; i < codeLength; i++)
        {
            code[i] = Random.Range(0, 10);
        }
        onCodeGenerated?.Invoke();
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
