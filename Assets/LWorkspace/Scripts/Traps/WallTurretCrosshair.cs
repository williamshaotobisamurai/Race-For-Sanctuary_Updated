using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallTurretCrosshair : MonoBehaviour
{
    [SerializeField] private Image crosshairImg;

    [SerializeField] private Color firingColor;
    [SerializeField] private Color aimmingcolor;

    public void SetFiringColor()
    {
        crosshairImg.color = firingColor;
    }

    public void SetAimmingColor()
    {
        crosshairImg.color = aimmingcolor; 
    }
}
