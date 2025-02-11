using Mono.Cecil.Cil;
using System.Collections.Generic;
using UnityEngine;

public class CodeRoom : Room
{
    private BoxCollider roomBounds;

    [SerializeField] private GameObject [] numbersPrefabs;

    public int[] code;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomBounds = GetComponent<BoxCollider>();
        code = GenerateRandomCode(3);
        SetRandomPositions();
    }

    private void SetRandomPositions()
    {
        Vector3 previousPos = Vector3.zero;
        for (int i = 0; i < code.Length; i++)
        {
            Vector3 newPos = randomPositions[Random.Range(0, randomPositions.Length)].position;
            while(newPos == previousPos)
            {
                newPos = randomPositions[Random.Range(0, randomPositions.Length)].position;
            }
            Instantiate(numbersPrefabs[code[i]], newPos, Quaternion.identity);
            previousPos = newPos;
        }
    }

    private int[] GenerateRandomCode(int codeLength)
    {
        code = new int[codeLength];
        for (int i = 0; i < codeLength; i++)
        {
            code[i] = Random.Range(0, 9);
        }
        return code;
    }

}
