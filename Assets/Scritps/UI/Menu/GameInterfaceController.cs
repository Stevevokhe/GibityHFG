using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameInterfaceController : MonoBehaviour
{
    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject prisonPanel;
    [SerializeField]
    private GameController gameController;

    private void Awake()
    {
        if (pauseButton == null)
            throw new System.Exception($"{name}: {nameof(pauseButton)} can't be null");

        if (pausePanel == null)
            throw new System.Exception($"{name}: {nameof(pausePanel)} can't be null");

        if (prisonPanel == null)
            throw new System.Exception($"{name}: {nameof(prisonPanel)} can't be null");

        if(gameController == null)
            throw new System.Exception($"{name}: {nameof(gameController)} can't be null");

        pausePanel.SetActive(false);
        gameController.PlayerCaught += (s, e) => StartPrison();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        pauseButton.interactable = false;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        pauseButton.interactable = true;
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }

    public void EndPrison()
    {
        Time.timeScale = 1.0f;
        prisonPanel.SetActive(false);
    }

    public void StartPrison()
    {
        Time.timeScale = 0.0f;
        prisonPanel.SetActive(true);
    }
}
