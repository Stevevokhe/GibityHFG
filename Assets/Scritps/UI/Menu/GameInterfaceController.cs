using System;
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
    private GameObject startPanel;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject losePanel;
    [SerializeField]
    private PrisonPanel prisonPanel;
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

        if (startPanel == null)
            throw new System.Exception($"{name}: {nameof(startPanel)} can't be null");

        if (winPanel == null)
            throw new System.Exception($"{name}: {nameof(winPanel)} can't be null");

        if (losePanel == null)
            throw new System.Exception($"{name}: {nameof(losePanel)} can't be null");

        if (gameController == null)
            throw new System.Exception($"{name}: {nameof(gameController)} can't be null");

        pausePanel.SetActive(false);
        gameController.PlayerCaught += StartPrison;
        gameController.LostGame += LostGame;
        gameController.WonGame += WonGame;
    }

    private void Start()
    {
        Time.timeScale = 0.0f;
        startPanel.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1.0f;
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
        prisonPanel.Hide();
    }

    public void StartPrison(object sender, int lostYears)
    {
        Time.timeScale = 0.0f;
        prisonPanel.Show(lostYears);
    }

    private void WonGame(object sender, EventArgs e)
    {
        Time.timeScale = 0.0f;
        winPanel.SetActive(true);
    }

    private void LostGame(object sender, EventArgs e)
    {
        Time.timeScale = 0.0f;
        losePanel.SetActive(true);
    }
}
