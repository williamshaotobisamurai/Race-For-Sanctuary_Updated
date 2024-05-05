using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    protected bool gameHasEnded = false;

    public float restartDelay = 1f;
    public GameObject completeLevelUI;

    [SerializeField] protected Skully skully;
    public Skully Skully { get => skully; }

    [SerializeField] protected CollectedCoinsManager collectedCoinsManager;
    [SerializeField] private TimerManager timerManager;
    public TimerManager TimerManager { get => timerManager;  }

    [SerializeField] protected EndTrigger endTrigger;
    [SerializeField] protected PoliceShip policeShip;
    public PoliceShip PoliceShip { get => policeShip; }

    [SerializeField] private List<MissileSoldier> missileSoldiers;
    [SerializeField] private List<EnemyMissile> enemyMissileList;

    [SerializeField] private MissileWarningUI missileWarningUI;

    [SerializeField] private SpaceStationTrigger spaceStationEntry;
    [SerializeField] private SpaceStationTrigger spaceStationExit;


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

    protected virtual void Start()
    {
        missileSoldiers = new List<MissileSoldier>(FindObjectsOfType<MissileSoldier>());
        missileSoldiers.ForEach(soldier => soldier.OnShootEvent += Soldier_OnShootEvent);

        collectedCoinsManager.Init();
        TimerManager.Init();
        skully.OnSkullyDiedEvent += Skully_OnSkullyDiedEvent;
        skully.OnCollectItemEvent += Skully_OnCollectItemEvent;
        endTrigger.OnSkullyEnterEvent += EndTrigger_OnSkullyEnterEvent;
        PoliceShip.OnCaughtSkullyEvent += PoliceShip_OnCaughtSkullyEvent;
        TimerManager.OnOutOfTimeEvent += TimerManager_OnOutOfTimeEvent;

        spaceStationEntry.OnSkullyEnterEvent += SpaceStationEntry_OnSkullyEnterEvent;
        spaceStationExit.OnSkullyEnterEvent += SpaceStationExit_OnSkullyEnterEvent;
    }

    private void SpaceStationExit_OnSkullyEnterEvent(SpaceStationTrigger spaceStationTrigger)
    {
        policeShip.gameObject.SetActive(true);
    }

    private void SpaceStationEntry_OnSkullyEnterEvent(SpaceStationTrigger spaceStationTrigger)
    {
        policeShip.gameObject.SetActive(false);
    }

    private void Soldier_OnShootEvent(EnemyMissile enemyMissile)
    {
        enemyMissileList.Add(enemyMissile);
    }

    private void OnDestroy()
    {
        skully.OnSkullyDiedEvent -= Skully_OnSkullyDiedEvent;
        skully.OnCollectItemEvent -= Skully_OnCollectItemEvent;
        endTrigger.OnSkullyEnterEvent -= EndTrigger_OnSkullyEnterEvent;
        PoliceShip.OnCaughtSkullyEvent -= EndTrigger_OnSkullyEnterEvent;
        TimerManager.OnOutOfTimeEvent -= TimerManager_OnOutOfTimeEvent;
        missileSoldiers.ForEach(soldier => soldier.OnShootEvent -= Soldier_OnShootEvent);
    }

    protected virtual void Update()
    {
        CheckIncomingMissile();
    }

    private void CheckIncomingMissile()
    {
        bool showWarning = false;
        enemyMissileList.ForEach(missile =>
        {
            if (missile != null)
            {
                missileWarningUI.Show();
                showWarning = true;
            }
        });

        if (!showWarning)
        {
            missileWarningUI.Hide();
        }
    }

    protected virtual void Skully_OnCollectItemEvent(ItemBase item)
    {
        switch (item.ItemType)
        {
            case ItemBase.EItemType.JETPACK_FUEL:
                JetpackFuelItem jetpack = item as JetpackFuelItem;
                TimerManager.IncreaseTime(jetpack.IncreaseAmount);
                break;

            default:
                break;
        }
    }

    protected virtual void TimerManager_OnOutOfTimeEvent()
    {
        EndGame();
    }

    protected virtual void PoliceShip_OnCaughtSkullyEvent()
    {
        EndGame();
    }

    protected virtual void Skully_OnSkullyDiedEvent()
    {
        EndGame();
    }

    protected virtual void EndTrigger_OnSkullyEnterEvent()
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
        if (!gameHasEnded)
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
