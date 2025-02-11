using Mono.Cecil.Cil;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private BoxCollider roomBounds;

    [SerializeField] protected Transform[] randomPositions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomBounds = GetComponent<BoxCollider>();
        
    }

}
