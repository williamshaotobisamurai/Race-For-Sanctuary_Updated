using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkullyMissile : MonoBehaviour
{
    [SerializeField] private KeyCode skillKey;
    [SerializeField] private float coolDown = 3f;

    private float lastCastTimeStamp;



    [SerializeField] private GameObject skillSphere;
 

    [SerializeField] private float aimingDuration = 3f;
    [SerializeField] private State state;
    [SerializeField] private SIRS SIRS;
    [SerializeField] private Missile missilePrefab;

    [SerializeField] private GameObject aimmingObject;

  

    private enum State
    {
        READY,
        AIMMING,
        COOLDOWN,
    }

    private void Start()
    {
        state = State.COOLDOWN;
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

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.SphereCast(ray, 20f, out hit, 500,SkullyWeaponManager.Instance.TargetsLayer))
                    {
                        Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.cyan, 5f);

                        Debug.Log(hit.transform.name);
                        Debug.Log("hit");
                        if (hit.collider.tag.Equals(GameConstants.OBSTACLE) || hit.collider.tag.Equals(GameConstants.ENEMY_HITPOINT))
                        {
                            Debug.DrawRay(ray.origin, hit.collider.transform.position - ray.origin, Color.magenta, 5f);
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
