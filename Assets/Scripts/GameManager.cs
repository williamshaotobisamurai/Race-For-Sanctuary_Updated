using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    protected bool gameHasEnded = false;

    public float restartDelay = 1f;
    public GameObject completeLevelUI;

    [SerializeField] protected Transform startTrans;

    [SerializeField] protected CameraFollowPlayer cameraFollowPlayer;

    [SerializeField] protected Skully skully;
    public Skully Skully { get => skully; }

    [SerializeField] private TimerManager timerManager;
    public TimerManager TimerManager { get => timerManager; }

    [SerializeField] protected EndTrigger endTrigger;
    [SerializeField] protected PoliceShip policeShip;
    public PoliceShip PoliceShip { get => policeShip; }

    [SerializeField] private List<MissileSoldier> missileSoldiers;
    [SerializeField] private List<EnemyMissile> enemyMissileList;

    [SerializeField] private MissileWarningUI missileWarningUI;

    [SerializeField] private SpaceStationTrigger spaceStationEntry;
    [SerializeField] private SpaceStationTrigger spaceStationExit;
    [SerializeField] private OverheatingStartSign overheatingStartSign;

    [SerializeField] private GameObject staticBarrier;
    [SerializeField] private Light overheatingLight;

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
            Debug.LogError("duplicated level manager");
            Destroy(gameObject);
        }
    }

    public virtual void InitScene()
    {
        missileSoldiers = new List<MissileSoldier>(FindObjectsOfType<MissileSoldier>());
        missileSoldiers.ForEach(soldier => soldier.OnShootEvent += Soldier_OnShootEvent);

        skully.OnSkullyDiedEvent += Skully_OnSkullyDiedEvent;
        skully.OnCollectItemEvent += Skully_OnCollectItemEvent;
        endTrigger.OnSkullyEnterEvent += EndTrigger_OnSkullyEnterEvent;
        PoliceShip.OnCaughtSkullyEvent += PoliceShip_OnCaughtSkullyEvent;
        TimerManager.OnOutOfTimeEvent += TimerManager_OnOutOfTimeEvent;

        if (spaceStationEntry != null)
        {
            spaceStationEntry.OnSkullyEnterEvent += SpaceStationEntry_OnSkullyEnterEvent;
            spaceStationExit.OnSkullyEnterEvent += SpaceStationExit_OnSkullyEnterEvent;
        }

        if (overheatingStartSign != null)
        {
            overheatingStartSign.OnSkullyEnterEvent += OnPlayerEnterOverheatingTrigger;
        }

        skully.DisableControl();

        cameraFollowPlayer.StartCoroutine(cameraFollowPlayer.CameraInterpolate(() =>
        {
            skully.EnableControl();
            TimerManager.Init();
        }));
    }

    public void InitSkullyWithData(SkullySnapshot snapshot, Checkpoint cp)
    {
        CollectedCoinsManager.CoinsInAllLevels = snapshot.totalCoinsCollected;
        CollectedCoinsManager.CollectedCoinsInCurrentLevel = snapshot.collectedCoinsInCurrentLevel;

        skully.HealthAmount = snapshot.health;
        skully.WeaponManager.SetupWeapon((WeaponItem.EWeaponType)snapshot.weaponType);

        if (cp == null)
        {
            skully.transform.position = startTrans.position;
        }
        else
        {
            skully.transform.position = cp.RespawnTrans.position;
        }
    }


    private void SpaceStationExit_OnSkullyEnterEvent(SpaceStationTrigger spaceStationTrigger)
    {
        policeShip.gameObject.SetActive(true);
        staticBarrier.SetActive(true);
    }

    private void SpaceStationEntry_OnSkullyEnterEvent(SpaceStationTrigger spaceStationTrigger)
    {
        policeShip.gameObject.SetActive(false);
        staticBarrier.SetActive(false);
        skully.StopOverheating();
        overheatingLight.DOIntensity(0, 0f);
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


    public void OnPlayerEnterOverheatingTrigger()
    {
        skully.StartOverheating();
        overheatingLight.DOIntensity(1.2f, 0.5f);
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
    }

    public void EndGame()
    {
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            //    Invoke("Restart", restartDelay);
            DOVirtual.DelayedCall(restartDelay, Restart);
        }
    }

    void Restart()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public SkullySnapshot GetCurrentData()
    {
        return new SkullySnapshot()
        {
            levelIndex = SceneManager.GetActiveScene().buildIndex,
            health = skully.HealthAmount,
            weaponType = ((int)skully.WeaponManager.CurrentWeapon),
            collectedCoinsInCurrentLevel = CollectedCoinsManager.CollectedCoinsInCurrentLevel
        };
    }
}
