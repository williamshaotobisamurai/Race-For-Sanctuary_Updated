using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkullySkill : MonoBehaviour
{
    [SerializeField] private KeyCode skillKey;
    [SerializeField] private float coolDown = 3f;

    private float lastCastTimeStamp;

    [SerializeField] private Image coolDownIcon;

    [SerializeField] private GameObject skillSphere;
    [SerializeField] private Crosshair crosshair;

    [SerializeField] private float aimingDuration = 3f;
    [SerializeField] private State state;
    [SerializeField] private SIRS SIRS;
    [SerializeField] private Missile missilePrefab;

    [SerializeField] private GameObject aimmingObject;

    [SerializeField] private LayerMask obstacleLayer;

    private enum State
    {
        READY,
        AIMMING,
        COOLDOWN,
    }

    private void Start()
    {
        state = State.COOLDOWN;
        coolDownIcon.DOFillAmount(1f, coolDown);
    }

    private Missile missileInstance;

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.READY:

                if (Input.GetKeyDown(skillKey))
                {
                    state = State.AIMMING;
                    crosshair.Show();

                    if (missileInstance != null)
                    {
                        Destroy(missileInstance);
                    }

                    missileInstance = Instantiate(missilePrefab);
                    missileInstance.AttachTo(SIRS.MissileAttachedTrans);
                }
                break;

            case State.AIMMING:

                if (Input.GetMouseButtonDown(0))
                {
                    lastCastTimeStamp = Time.time;
                    state = State.COOLDOWN;
                    Vector3 mousePos = Input.mousePosition;
                    coolDownIcon.fillAmount = 0f;
                    coolDownIcon.DOFillAmount(1f, coolDown);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.SphereCast(ray, 20f, out hit, 500, obstacleLayer))
                    {
                        Debug.Log(hit.transform.name);
                        Debug.Log("hit");
                        if (hit.collider.tag.Equals(GameConstants.OBSTACLE))
                        {
                            aimmingObject.transform.position = hit.collider.transform.position;
                        }
                    }
                    else
                    {
                        mousePos += Camera.main.transform.forward * 30f; // Make sure to add some "depth" to the screen point 
                        Vector3 aim = Camera.main.ScreenToWorldPoint(mousePos);
                        aimmingObject.transform.position = aim;
                    }

                    missileInstance.transform.LookAt(aimmingObject.transform);
                    missileInstance.Launch();
                    missileInstance = null;
                    crosshair.Hide();
                }
                break;

            case State.COOLDOWN:

                if (IsReady())
                {
                    state = State.READY;
                }
                break;

            default:
                break;
        }
    }


    private bool IsReady()
    {
        return Time.time > lastCastTimeStamp + coolDown;
    }
}
