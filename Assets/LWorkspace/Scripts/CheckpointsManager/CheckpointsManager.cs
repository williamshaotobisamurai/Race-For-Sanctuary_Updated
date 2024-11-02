using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointsManager : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> checkPointsList;

    public void InitSaveData()
    {
        if (!FileManager.FileExistsInPersistentPath(GameConstants.SAVE_FILE_DATA))
        {
            GameSaveData snapshot = new GameSaveData();
            FileManager.SaveTextToPersistentPath(GameConstants.SAVE_FILE_DATA, JsonUtility.ToJson(snapshot));
        }
    }

    public Checkpoint FindCheckpoint(GameSaveData data)
    {
        return checkPointsList.Find(t => t.ID == data.checkPointID);
    }

    public void RefreshCheckpoints()
    {
        Debug.Log("init checkpoints");
        checkPointsList.ForEach(c => c.OnSkullyEnteredEvent -= CheckPoint_OnSkullyEnteredEvent);
        checkPointsList.Clear();
        checkPointsList = FindObjectsOfType<Checkpoint>().ToList();
        checkPointsList.ForEach(c => c.OnSkullyEnteredEvent += CheckPoint_OnSkullyEnteredEvent);
    }

    public GameSaveData LoadSavedData()
    {
        string text = FileManager.LoadTextFromPersistentPath(GameConstants.SAVE_FILE_DATA);
        return JsonUtility.FromJson<GameSaveData>(text);
    }

    private void CheckPoint_OnSkullyEnteredEvent(Checkpoint checkpoint, Skully skully)
    {
        Debug.Log("reach checkpoint " + checkpoint.name);

        GameSaveData data = LevelManager.Instance.GetCurrentData();
        data.checkPointID = checkpoint.ID;

        Debug.Log(JsonUtility.ToJson(data));

        SaveCurrentGameData(data);
    }

    private void SaveCurrentGameData(GameSaveData data)
    {

        FileManager.SaveTextToPersistentPath(GameConstants.SAVE_FILE_DATA, JsonUtility.ToJson(data));
    }
}
