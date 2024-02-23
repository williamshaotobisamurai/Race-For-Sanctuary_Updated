using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HangingAlien : EnemyBase
{
    [SerializeField] private Animator animator;
    [SerializeField] private float flySpeed = 10f;
    [SerializeField] protected float rotateRate = 20f;

    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private float jumpAttackDistance = 5f;

    [SerializeField] private int jumpAttackDamage = 50;
    public int JumpAttackDamage { get => jumpAttackDamage; set => jumpAttackDamage = value; }

    [SerializeField] private float jumpAttckBiteDuration = 3f;
    public float JumpAttckBiteDuration { get => jumpAttckBiteDuration; set => jumpAttckBiteDuration = value; }

    // Start is called before the first frame update

    protected override void LookAtSkully()
    {
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Skully skully = other.GetComponent<Skully>();
        if (skully != null)
        {
            detectTrigger.enabled = false;

            JumpAttack(skully);
     //       StartCoroutine(FlyTowardsSkullyCoroutine(skully));
        }
    }


    private IEnumerator FlyTowardsSkullyCoroutine(Skully skully)
    {
        animator.Play("Fly");

        while (true)
        {
            transform.Translate(Vector3.forward * flySpeed * Time.deltaTime, Space.Self);

            Quaternion prevRotation = transform.rotation;

            transform.LookAt(GameManager.Instance.Skully.transform);

            Quaternion desiredRotation = transform.rotation;

            transform.rotation = Quaternion.Lerp(prevRotation, desiredRotation, Time.deltaTime * rotateRate);
            float distance = Vector3.Distance(skully.transform.position, transform.position);
            Debug.Log("remaining  distance " + distance);

            Debug.Log("flying ");

            if (distance <= jumpAttackDistance)
            {
                JumpAttack(skully);
                yield break;
            }

            if (transform.position.z < skully.transform.position.z - 5f)
            {
                Destroy(gameObject);
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void JumpAttack(Skully skully)
    {
        Vector3 leavingStartPosition = Vector3.zero;
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            skully.AttackByAlienJumpAttack(this);
            transform.LookAt(skully.transform);
        });
        seq.Append(transform.DOMove(skully.transform.position, 0.2f));
        seq.AppendCallback(() => { animator.Play("FlyBiteAttack"); });
        seq.AppendInterval(JumpAttckBiteDuration + 0.2f);
        seq.AppendCallback(() =>
        {
            animator.Play("Fly");
            Debug.Log("finish biting " + Time.time);
            leavingStartPosition = transform.position;
        });
        seq.Append(transform.DOMove(leavingStartPosition - transform.forward * 2 + transform.up * 2, 3f));
        seq.OnComplete(() =>
        {
            Debug.Log("leave and destroy " + Time.time);
            Destroy(gameObject);
        });
        seq.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(0);
        }
    }
}


