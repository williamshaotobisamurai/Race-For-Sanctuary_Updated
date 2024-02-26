using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUIManager : MonoBehaviour
{
    [SerializeField] private Skully skully;

    [SerializeField] private RectTransform speedIconRotationRoot;

    [SerializeField] private List<Image> arrows;

    private void Update()
    {
        Vector2 skullyVelocityOnPlane = skully.GetCurrentVelocity();        
        float angle = Vector2.SignedAngle(Vector2.up, skullyVelocityOnPlane);

        speedIconRotationRoot.localEulerAngles = new Vector3(0, 0, angle);

        arrows.ForEach(t => t.gameObject.SetActive(false));
        float speed = skullyVelocityOnPlane.magnitude;
        switch (speed)
        {
            case 0:
                break;

            case float s when (s > 0 && s < 50):
                arrows[0].gameObject.SetActive(true);
                break;

            case float s when (s >= 50 && s < 100):
                arrows[0].gameObject.SetActive(true);
                arrows[1].gameObject.SetActive(true);

                break;

            case float s when (s >= 100):
                arrows[0].gameObject.SetActive(true);
                arrows[1].gameObject.SetActive(true);
                arrows[2].gameObject.SetActive(true);
                break;
        }
    }
}
