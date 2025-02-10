using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    [SerializeField]
    private Vector3 pressedOffset = new Vector3(0, 0, -0.1f); // How much the button moves
    [SerializeField]
    private float pressSpeed = 0.1f; // How fast it moves
    
    private Button button; //Button reference

    private Vector3 startPosition;

    private bool isPressed = false;

    void Start()
    {
        startPosition = transform.localPosition;
        button = GetComponent<Button>();
    }

    void OnMouseDown()
    {
        if (!isPressed)
        {
            StopAllCoroutines();
            StartCoroutine(MoveButton(startPosition + pressedOffset));
            isPressed = true;
            button.onClick.Invoke();
        }
    }

    void OnMouseUp()
    {
        if (isPressed)
        {
            StopAllCoroutines();
            StartCoroutine(MoveButton(startPosition));
            isPressed = false;
        }
    }

    IEnumerator MoveButton(Vector3 targetPos)
    {
        Vector3 startPos = transform.localPosition;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / pressSpeed;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        transform.localPosition = targetPos;
    }
}
