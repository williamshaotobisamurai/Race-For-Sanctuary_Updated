using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SophiaComeEventManager : MonoBehaviour
{
    [SerializeField] private PoliceshipBoss policeshipBoss;
    [SerializeField] private GameObject sophia;
    [SerializeField] private Skully skully;
    [SerializeField] private TimerManager timerManager;
    [SerializeField] private Transform eventStartPos;

    private void Start()
    {
        policeshipBoss.OnKilledEvent += PoliceshipBoss_OnKilledEvent;
        Debug.LogError("stop timer");
        timerManager.StopAndHideTimer();
    }

    private void OnDestroy()
    {
        policeshipBoss.OnKilledEvent -= PoliceshipBoss_OnKilledEvent;
    }

    public void MoveSkullyToEventStartPos()
    {
        skully.DisableControl();
        StartCoroutine(MoveSkullyCotoutine(skully, eventStartPos,20));
    }

    public void EnableSkullyXYControl()
    {
        skully.SetMaxForwardSpeedFactor(0);
        skully.EnableControl();
        skully.EnableXYControl();
    }

    private void PoliceshipBoss_OnKilledEvent()
    {
        skully.DisableControl();
        sophia.gameObject.SetActive(true);
        sophia.gameObject.transform.position = policeshipBoss.transform.position;
        StartCoroutine(MoveSkullyCotoutine(skully, sophia.transform,20));
    }

    IEnumerator MoveSkullyCotoutine(Skully skully, Transform target,float speed)
    {
        float distance = Vector3.Distance(skully.transform.position, target.position);
        while (distance >= 3f)
        {
            skully.transform.position = Vector3.MoveTowards(skully.transform.position, target.position, Time.deltaTime * speed);
            skully.transform.LookAt(target.position);
            distance = Vector3.Distance(skully.transform.position, target.position);
            yield return new WaitForEndOfFrame();
        }
    }
}
