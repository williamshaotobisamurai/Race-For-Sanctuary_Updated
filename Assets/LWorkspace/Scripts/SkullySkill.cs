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
    [SerializeField] private ParticleSystem shockParticle;

    private void Start()
    {
        coolDownIcon.DOFillAmount(1f, coolDown);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(skillKey))
        {
            if (IsReady())
            {
                Cast();
            }
        }
    }

    private bool IsReady()
    {
        return Time.time > lastCastTimeStamp + coolDown;
    }

    private void Cast()
    {
        Debug.Log("cast ");
        skillSphere.SetActive(true);
        skillSphere.transform.localPosition = Vector3.zero;
        lastCastTimeStamp = Time.time;
        shockParticle.Play();
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => coolDownIcon.fillAmount = 0f);
        seq.Append(skillSphere.transform.DOLocalMoveZ( 20f,1f));
        seq.AppendCallback(() =>
        {
            skillSphere.transform.localPosition = Vector3.zero;
            skillSphere.SetActive(false);
        });
        seq.Append(coolDownIcon.DOFillAmount(1f, coolDown));        
        seq.Play();
    }
}
