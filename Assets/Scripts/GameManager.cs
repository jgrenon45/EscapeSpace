using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Room[] rooms;

    public bool debugModeOn = false;    

    public int[] defaultCode = { 1, 1, 1 };

    private void Awake()
    {
        instance = this;
    }
}
