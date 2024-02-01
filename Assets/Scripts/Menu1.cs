using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu1 : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Begin!");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
        
}
