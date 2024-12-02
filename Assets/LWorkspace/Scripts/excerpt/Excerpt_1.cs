using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Excerpt_1 : MonoBehaviour
{
    [SerializeField] private TypeWritter typeWriiter;
    // Start is called before the first frame update
    private void Start()
    {
        typeWriiter.ShowText(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }
}
