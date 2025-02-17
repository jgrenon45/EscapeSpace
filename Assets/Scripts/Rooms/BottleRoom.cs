using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BottleRoom : CodeRoom
{
    [SerializeField] private GameObject blueBottle;
    [SerializeField] private GameObject yellowBottle;
    [SerializeField] private GameObject redBottle;
    [SerializeField] private GameObject randomPositions;

    private List<string> bottleOrderString = new List<string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        SetBottlePuzzle();
    }

    private void SetBottlePuzzle()
    {
        // Set the bottles in a random order. Order can be:
        // blue, yellow, red
        // blue, red, yellow
        // yellow, blue, red
        // yellow, red, blue
        // red, blue, yellow
        // red, yellow, blue
        int[] bottleOrder = new int[3];
        for (int i = 0; i < bottleOrder.Length; i++)
        {
            bottleOrder[i] = i;
        }

        ShuffleArray(bottleOrder);

        List<Transform> bottleTransforms = randomPositions.GetComponentsInChildren<Transform>().ToList();
        bottleTransforms.RemoveAt(0); //Remove first item since we don't want the parent's transform

        for (int i = 0; i < bottleOrder.Length; i++)
        {
            SetBottleOrderString(bottleOrder[i]);

            for (int j = 0; j < code[i]; j++)
            {
                // Ensure there are available positions left
                if (bottleTransforms.Count == 0)
                {
                    Debug.LogWarning("No available positions left for bottle placement.");
                    break; // Stop placing more bottles if there are no free positions
                }

                //Get random position
                int randIndex = Random.Range(0, bottleTransforms.Count);

                // Determine which bottle to spawn
                GameObject bottlePrefab = null;
                switch (bottleOrder[i])
                {
                    case 0:
                        bottlePrefab = blueBottle;
                        break;
                    case 1:
                        bottlePrefab = yellowBottle;
                        break;
                    case 2:
                        bottlePrefab = redBottle;
                        break;
                }

                // Spawn the bottle
                if (bottlePrefab != null)
                {
                    Instantiate(bottlePrefab, bottleTransforms[randIndex].transform.position, Quaternion.identity);
                }

                // Since the position is now occupied, remove it from the list
                bottleTransforms.RemoveAt(randIndex);
            }            
            
        }

    }

    private void ShuffleArray<T>(T[] array)
    {       
        int n = array.Length;

        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // Get a random index
            (array[i], array[j]) = (array[j], array[i]); // Swap elements
        }
    } 
    
    /// <summary>
    /// Set a string list in order of the bottles colors for the code
    /// </summary>
    /// <param name="bottleCode"></param>
    private void SetBottleOrderString(int bottleCode)
    {
        switch (bottleCode)
        {
            case 0:
                bottleOrderString.Add("Bleu");
                break;
            case 1:
                bottleOrderString.Add("Jaune");
                break;
            case 2:
                bottleOrderString.Add("Rouge");
                break;
        }
    }

    public List<string> GetBottleOrderString()
    {
        return bottleOrderString;
    }
}
