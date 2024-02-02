using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    bool gameHasEnded = false;

    public float restartDelay = 1f;

    public GameObject completeLevelUI;


    [SerializeField] private Skully skully;
   
    [SerializeField] private CollectedCoinsManager collectedCoinsManager;

    private void Start()
    {
        collectedCoinsManager.Init();
        skully.OnSkullyDiedEvent += Skully_OnSkullyDiedEvent;
    }

    private void OnDestroy()
    {
        skully.OnSkullyDiedEvent -= Skully_OnSkullyDiedEvent;
    }

    private void Skully_OnSkullyDiedEvent()
    {
        EndGame();  
    }


    public void CompleteLevel () 
    {
        completeLevelUI.SetActive(true);
    }

    public void EndGame () 
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            Invoke("Restart", restartDelay);
        }
    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
