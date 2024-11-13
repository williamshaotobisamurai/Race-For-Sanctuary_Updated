using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private SettingsPanel settingsPanel;
     

    public void InitUI(GameSaveData data)
    {
        if (data.levelIndex == 0)
        {
            continueButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(true);
        }
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnContinueButtonClicked()
    {
        GameSaveData saveData = MainGameManager.CheckpointsManager.LoadSavedData();
        SceneManager.LoadScene(saveData.levelIndex);
    }

    public void OnSettingsButtonClicked()
    {
        settingsPanel.gameObject.SetActive(true);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
