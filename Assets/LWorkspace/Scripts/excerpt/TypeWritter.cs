using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TypeWritter : MonoBehaviour
{
    [SerializeField] private Text text;

    [SerializeField] private TextAsset textAsset;
    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private float duration = 30f;

    void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(text.DOText(textAsset.text, duration, false));
        seq.AppendCallback(() => audioSrc.Stop());
        seq.AppendInterval(3f);
        seq.OnComplete(()=>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }
}
 