using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;

    public void InitUI()
    {
        if (MainGameManager.CheckpointsManager.HasSavedCheckPoint(out SkullySnapshot snapshot))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnContinueButtonClicked()
    {
        if (MainGameManager.CheckpointsManager.HasSavedCheckPoint(out SkullySnapshot snapshot))
        {
            SceneManager.LoadScene(snapshot.levelIndex);
        }
    }

    public void OnSettingsButtonClicked()
    { 
        
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
