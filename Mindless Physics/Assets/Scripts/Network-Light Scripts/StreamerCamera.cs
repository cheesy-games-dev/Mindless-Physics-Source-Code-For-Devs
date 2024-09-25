using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamerCamera : MonoBehaviour
{
    public float newFOV = 90f;
    public float lerpSpeed = 25f;
    public Camera myCamera;
    public Camera referenceCamera;

    void Awake()
    {
        myCamera = GetComponent<Camera>();
        if(Application.isMobilePlatform){
            Destroy(this.gameObject);
        }
        transform.SetParent(null, false);
        transform.parent = null;
        transform.position = referenceCamera.transform.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(myCamera.transform.position, referenceCamera.transform.position, lerpSpeed * Time.deltaTime);
        transform.eulerAngles = referenceCamera.transform.eulerAngles;
        myCamera.fieldOfView = newFOV;
        myCamera.farClipPlane = referenceCamera.farClipPlane - referenceCamera.farClipPlane / 10;
        myCamera.nearClipPlane = referenceCamera.nearClipPlane + 0.1f;
        myCamera.depth = referenceCamera.depth + 1f;
    }
}
