using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    [SerializeField] private CheckpointsManager checkpointsManager;
    public static CheckpointsManager CheckpointsManager { get => instance.checkpointsManager; }

    [SerializeField] private CollectedCoinsManager collectedCoinsManager;
    public static CollectedCoinsManager CollectedCoinsManager { get => instance.collectedCoinsManager; }

    [SerializeField] private StartSceneManager startSceneManager;

    private static MainGameManager instance;
    public static MainGameManager Instance
    {
        get => instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            checkpointsManager.InitSaveData();
        }
        else
        {
            Debug.LogError("duplicated objects");
            Destroy(instance.gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        GameSaveData data = checkpointsManager.LoadSavedData();
        startSceneManager.InitUI(data);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        checkpointsManager.RefreshCheckpoints();

        if (arg0.name.Contains("Excerpt"))
        {
            return;
        }

        LevelManager.Instance.InitLevel();
        collectedCoinsManager.Init(LevelManager.Instance.Skully);

        GameSaveData saveData = checkpointsManager.LoadSavedData();

        if (saveData.levelIndex == SceneManager.GetActiveScene().buildIndex)
        {
            Checkpoint cp = checkpointsManager.FindCheckpoint(saveData);
            LevelManager.Instance.InitSkullyWithData(saveData, cp);
        }
    }
}
