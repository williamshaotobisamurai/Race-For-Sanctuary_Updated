using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    
    public Rigidbody rb;
    // this defines the starting position of camera
    // relative to the player
    [SerializeField] Vector3 startingCameraOffset = new Vector3(0.0f,4.0f,4.0f);
   
    // This defines the starting angle of the camera   
    Vector3 startingAngle = new Vector3(45.0f,180.0f,0.0f);
    Vector3 endingAngle = new Vector3(0.0f,0.0f,0.0001f);
    bool ifRoutineEnded = false;
    // This defines how fast the camera interpolate from the 
    // starting position to the ending position as well as
    // from starting angle to the ending angle
    float transformRate = 0.05f;

    void Awake()
{
    // Initialize camera angle in Awake
    transform.eulerAngles = startingAngle;
}

    // Update is called once per frame
    void Start(){
       // initialize camera position
       transform.position = player.position + startingCameraOffset;
       //starting coroutine
       StartCoroutine(cameraInterpolate());

    }
    void Update()    
    {
        if(ifRoutineEnded){
            transform.position = player.position + offset;
        }
    }
    IEnumerator cameraInterpolate()
{
    Vector3 positionInterval = (offset-startingCameraOffset)/(10.0f/transformRate);
    Vector3 newOffset = startingCameraOffset;
    Vector3 angleInterval = (endingAngle -startingAngle)/(10.0f/transformRate);

    Debug.Log($"angle interval: {angleInterval}");
    for (float alpha = 10.0f; alpha >= 0; alpha -= transformRate)
    {
        //interpolating position
        //Debug.Log($"current angle: {transform.eulerAngles}");
        Debug.Log($"current offset: {newOffset}");
        newOffset += positionInterval;
        transform.position = player.position + newOffset;
        //interpolating angle
        transform.eulerAngles += angleInterval;

        yield return null;
    }
    ifRoutineEnded = true;
    transform.position = player.position + offset;
    transform.eulerAngles = endingAngle;
}
}
