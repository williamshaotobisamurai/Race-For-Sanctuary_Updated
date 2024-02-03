using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;

    public float restartDelay = 1f;
    public GameObject completeLevelUI;

    [SerializeField] private Skully skully;
    public Skully Skully { get => skully; }

    [SerializeField] private CollectedCoinsManager collectedCoinsManager;
    [SerializeField] private EndTrigger endTrigger;

    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        collectedCoinsManager.Init();
        skully.OnSkullyDiedEvent += Skully_OnSkullyDiedEvent;
        endTrigger.OnSkullyEnterEvent += EndTrigger_OnSkullyEnterEvent;
    }

    private void OnDestroy()
    {
        skully.OnSkullyDiedEvent -= Skully_OnSkullyDiedEvent;
        endTrigger.OnSkullyEnterEvent -= EndTrigger_OnSkullyEnterEvent;
    }

    private void Skully_OnSkullyDiedEvent()
    {
        EndGame();
    }

    private void EndTrigger_OnSkullyEnterEvent()
    {
        CompleteLevel();
    }

    public void CompleteLevel()
    {
        Debug.Log("complete level");
        completeLevelUI.SetActive(true);
        collectedCoinsManager.AddLevelCoinsToTotal();
    }

    public void EndGame()
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
