using Mono.Cecil.Cil;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    protected BoxCollider roomBounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        roomBounds = GetComponent<BoxCollider>();        
    }

}
