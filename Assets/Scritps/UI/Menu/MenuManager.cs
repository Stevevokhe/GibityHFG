using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private MenuSettingsUI settingsUI;

    private void Awake()
    {
        if(settingsUI == null)
        {
            throw new System.ArgumentNullException(nameof(settingsUI));
        }

        settingsUI.LoadAllSettings();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
