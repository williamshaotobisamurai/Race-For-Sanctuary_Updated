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
    [SerializeField] private TimerManager timerManager;
    [SerializeField] private EndTrigger endTrigger;
    [SerializeField] private PoliceShip policeShip;

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
        timerManager.Init();
        skully.OnSkullyDiedEvent += Skully_OnSkullyDiedEvent;
        skully.OnCollectItemEvent += Skully_OnCollectItemEvent;
        endTrigger.OnSkullyEnterEvent += EndTrigger_OnSkullyEnterEvent;
        policeShip.OnCaughtSkullyEvent += PoliceShip_OnCaughtSkullyEvent;
        timerManager.OnOutOfTimeEvent += TimerManager_OnOutOfTimeEvent;
    }

    private void OnDestroy()
    {
        skully.OnSkullyDiedEvent -= Skully_OnSkullyDiedEvent;
        skully.OnCollectItemEvent -= Skully_OnCollectItemEvent;
        endTrigger.OnSkullyEnterEvent -= EndTrigger_OnSkullyEnterEvent;
        policeShip.OnCaughtSkullyEvent -= EndTrigger_OnSkullyEnterEvent;
        timerManager.OnOutOfTimeEvent -= TimerManager_OnOutOfTimeEvent;
    }

    private void Skully_OnCollectItemEvent(ItemBase item)
    {
      
        switch (item.ItemType)
        {
            case ItemBase.EItemType.JETPACK_FUEL:
                JetpackFuelItem jetpack = item as JetpackFuelItem;
                timerManager.IncreaseTime(jetpack.IncreaseAmount);
                break;
      
            default:
                break;
        }
    }

    private void TimerManager_OnOutOfTimeEvent()
    {
        EndGame();
    }

    private void PoliceShip_OnCaughtSkullyEvent()
    {
        EndGame();
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
