using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    // this defines the starting position of camera
    // relative to the player
    [SerializeField] Vector3 startingCameraOffset = new Vector3(0.0f, 4.0f, 4.0f);

    // This defines the starting angle of the camera   
    Vector3 startingAngle = new Vector3(45.0f, 180.0f, 0.0f);
    Vector3 endingAngle = new Vector3(0.0f, 0.0f, 0.0001f);

    [SerializeField] private bool ifRoutineEnded = false;
    // This defines how fast the camera interpolate from the 
    // starting position to the ending position as well as
    // from starting angle to the ending angle
    float transformRate = 5f;

    void Awake()
    {
        // Initialize camera angle in Awake
     //   transform.eulerAngles = startingAngle;
    }

    // Update is called once per frame
    void Start()
    {
        // initialize camera position
      //  transform.position = player.position + startingCameraOffset;
        //starting coroutine
        //    StartCoroutine(CameraInterpolate());
    }

    public void UpdateCamera()
    {
        if (ifRoutineEnded)
        {
            transform.position = player.position + offset;
        }
    }

    public IEnumerator CameraInterpolate(Action OnComplete)
    {
        Vector3 positionInterval = (offset - startingCameraOffset) / (10.0f / transformRate);
        Vector3 newOffset = startingCameraOffset;
        Vector3 angleInterval = (endingAngle - startingAngle) / (10.0f / transformRate);

        Debug.Log($"angle interval: {angleInterval}");
        //   for (float alpha = 10.0f; alpha >= 0; alpha -= transformRate)
        float transitionTime = 2f;
        float duration = 0f;
        while (duration < transitionTime)
        {
            duration += Time.deltaTime;
            //interpolating position
            //Debug.Log($"current angle: {transform.eulerAngles}");
            newOffset += positionInterval * Time.deltaTime;
            transform.position = player.position + newOffset;
            //interpolating angle
            transform.eulerAngles += angleInterval * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        ifRoutineEnded = true;
        transform.position = player.position + offset;
        transform.eulerAngles = endingAngle;
        OnComplete?.Invoke();
    }

    //public IEnumerator Shake(float duration, float magnitude)
    //{
    //    Vector3 originalPos = transform.position;

    //    float elapsed = 0.0f;
    //    while (elapsed < duration)
    //    {
    //        float x = UnityEngine.Random.Range(-2f, 2f) * magnitude;
    //        float y = UnityEngine.Random.Range(-2f, 2f) * magnitude;

    //        transform.localPosition = 
    //    }
    //}
}
