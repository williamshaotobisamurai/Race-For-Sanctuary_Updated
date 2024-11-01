using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointsManager : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> checkPointsList;

    public Checkpoint FindCheckpoint(SkullySnapshot data)
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

    public bool HasSavedCheckPoint(out SkullySnapshot snapshot)
    {
        if (FileManager.FileExistsInPersistentPath(GameConstants.SAVE_FILE_DATA))
        {
            string text = FileManager.LoadTextFromPersistentPath(GameConstants.SAVE_FILE_DATA);
            snapshot = JsonUtility.FromJson<SkullySnapshot>(text);

            return true;
        }
        else
        {
            snapshot = null;
            return false;
        }
    }

    private void CheckPoint_OnSkullyEnteredEvent(Checkpoint checkpoint, Skully skully)
    {
        Debug.Log("reach checkpoint " + checkpoint.name);

        SkullySnapshot snapshot = GameManager.Instance.GetCurrentData();
        snapshot.checkPointID = checkpoint.ID;

        Debug.Log(JsonUtility.ToJson(snapshot));

        FileManager.SaveTextToPersistentPath(GameConstants.SAVE_FILE_DATA, JsonUtility.ToJson(snapshot));
    }
}
