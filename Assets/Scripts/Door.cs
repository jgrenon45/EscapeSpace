using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;

    [SerializeField]
    private Vector3 SlideDirection = Vector3.back;

    [SerializeField]
    private float SlideAmount = 1.9f;

    [SerializeField]
    private float Speed = 1f;

    private Vector3 startPosition;

    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        startPosition = transform.position;

    }

    public void Open()
    {
        if (!isOpen)
        {
            if(AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            AnimationCoroutine = StartCoroutine(DoSlidingOpen());
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            AnimationCoroutine = StartCoroutine(DoSlidingClose());
        }
    }

    private IEnumerator DoSlidingOpen()
    {
        Vector3 endPosition = startPosition + SlideDirection * SlideAmount;
        Vector3 startingPosition = transform.position;
        float t = 0;
        isOpen = true;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
            t += Time.deltaTime * Speed;
        }
        isOpen = true;
    }

    private IEnumerator DoSlidingClose()
    {
        Vector3 endPosition = startPosition;
        Vector3 startingPosition = transform.position;
        float t = 0;
        isOpen = false;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
            t += Time.deltaTime * Speed;
        }
        isOpen = true;
    }
}
