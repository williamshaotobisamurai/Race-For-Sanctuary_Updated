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
        startSceneManager.InitUI();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        checkpointsManager.RefreshCheckpoints();

        GameManager.Instance.InitScene();
        collectedCoinsManager.Init(GameManager.Instance.Skully);

        if (checkpointsManager.HasSavedCheckPoint(out SkullySnapshot snapshot))
        {
            if (snapshot.levelIndex == SceneManager.GetActiveScene().buildIndex)
            {
                Checkpoint cp = checkpointsManager.FindCheckpoint(snapshot);
                GameManager.Instance.InitSkullyWithData(snapshot, cp);
            }
            else
            {
                Debug.LogError("level index doesn't match");
            }
        }
    }
}
