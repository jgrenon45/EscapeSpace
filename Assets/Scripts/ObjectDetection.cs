using HoloLab.DNN.Classification;
using Lightbug.GrabIt;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    private bool isDetectingObject = false;
    private GrabbableObject objectToClassify;
    private List<string> objectSequence;

    [SerializeField] ObjectsRoom room;
    [SerializeField] OpenableObject objectToOpen;

    [SerializeField] Transform objectTargetPosition;
    [SerializeField] float moveSpeed = 2f;

    [SerializeField] RenderTexture renderTexture;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        room.onSequenceGenerated += OnSequenceGenerated;        
    }

    private void Update()
    {
        //Debug
        if (Input.GetKeyDown(KeyCode.R))
        {
            Tuple<ClassificationModel, List<string>> modelInfo = GameObject.Find("ObjectsRoom").GetComponent<ObjectsRoom>().GetModelInfo().ToTuple();
            string detectedObjectName = ClassifyObject(modelInfo.Item1, modelInfo.Item2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable") && !isDetectingObject)
        {
            objectToClassify = other.GetComponent<GrabbableObject>();
            isDetectingObject = true;

            // Ungrab the object and disable rigid body so it can move towards the target position
            Camera.main.GetComponent<GrabIt>().Ungrab();
            objectToClassify.GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(MoveObject(objectToClassify, objectTargetPosition.position));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(OnDetectionZoneExit(1f));
    }

    private IEnumerator MoveObject(GrabbableObject obj, Vector3 targetPos)
    {
        while (obj != null && Vector3.Distance(obj.transform.position, targetPos) > 0.01f)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, targetPos, moveSpeed * Time.deltaTime);

            //Do the same thing for rotation
            obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, Quaternion.Euler(obj._targetRotation), moveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
        OnMoveCompleted();
    }

    private IEnumerator OnDetectionZoneExit(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        isDetectingObject = false;
    }

    private void CheckObjectSequence()
    {
        //When object is in the center, run the detection process
        Tuple<ClassificationModel, List<string>> modelInfo = GameObject.Find("ObjectsRoom").GetComponent<ObjectsRoom>().GetModelInfo().ToTuple();
        string detectedObjectName = ClassifyObject(modelInfo.Item1, modelInfo.Item2);
        if (ObjectMatches(detectedObjectName, objectToClassify))
        {
            //Remove first object since it has corresponded to the detected object
            objectSequence.RemoveAt(0);

            //If there is still objects in the sequence
            if (objectSequence.Count > 0)
            {
                AudioManager.instance.soundsAudioSource.PlayOneShot(AudioManager.instance.digitSuccess);
            }
            else
            {
                AudioManager.instance.soundsAudioSource.PlayOneShot(AudioManager.instance.codeSuccess);
                objectToOpen.Open();
                gameObject.SetActive(false);
            }
            Destroy(GameObject.Find(objectToClassify.name));
            isDetectingObject = false;
        }
        else
        {
            AudioManager.instance.soundsAudioSource.PlayOneShot(AudioManager.instance.codeFail);
            Rigidbody rb = objectToClassify.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.linearVelocity = rb.transform.forward * 2;
        }
    }

    private void OnMoveCompleted()
    {
        StopAllCoroutines();
        CheckObjectSequence();
    }

    private void OnSequenceGenerated()
    {
        objectSequence = room.GetObjectSequence();
    }

    private string ClassifyObject(ClassificationModel model, List<string> labels)
    {
        if (model != null && labels != null)
        {
            // Get Texture from RenderTexture
            Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);

            // ReadPixels looks at the active RenderTexture.
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();
            if (tex != null)
            {
                // Crop Texture from Center
                var croped_texture = Crop.CenterCrop(tex);

                // Classify
                (var class_id, var score) = model.Classify(croped_texture);

                // Show Class on Unity Console
                Debug.Log($"{class_id} {labels[class_id]} ({score:F3})");

                // Convert to PNG and save
                byte[] bytes = croped_texture.EncodeToPNG();
                File.WriteAllBytes(Application.dataPath + "/Screenshot.png", bytes);

                // Destroy Texture
                Destroy(croped_texture);

                //Return the object's name
                return labels[class_id];
            }
        }
        return String.Empty;
    }

    private bool ObjectMatches(string detectedObjectName, GrabbableObject objectToClassify)
    {
        if (detectedObjectName.ToLower().Contains(objectSequence[0].ToLower()))
        {
            return true;
        }
        else
        {           
            return false;
        }
    }
}
