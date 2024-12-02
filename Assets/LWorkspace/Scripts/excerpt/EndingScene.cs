using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour
{
    [SerializeField] private TypeWritter typeWriiter;
    // Start is called before the first frame update
    [SerializeField] private GameObject credits;
    private void Start()
    {
        typeWriiter.ShowText(() =>
        {
            credits.gameObject.SetActive(true);
        });
    }
}
