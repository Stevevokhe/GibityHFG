using System;
using Unity.VisualScripting;
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
    [SerializeField]
    private DoorTeleportFadeAnimationEventController doorTeleportFadeAnimationController;
    [SerializeField]
    private TeleportTextMeshProUGUI teleportText;
    [SerializeField]
    private GameObject teleportImage;

    private Animator doorTeleportAnimator;

    public static GameInterfaceController Instance;
    public static bool IsTeleporting;

    private void Awake()
    {
        if (pauseButton == null)
            throw new Exception($"{name}: {nameof(pauseButton)} can't be null");

        if (pausePanel == null)
            throw new Exception($"{name}: {nameof(pausePanel)} can't be null");

        if (prisonPanel == null)
            throw new Exception($"{name}: {nameof(prisonPanel)} can't be null");

        if (startPanel == null)
            throw new Exception($"{name}: {nameof(startPanel)} can't be null");

        if (winPanel == null)
            throw new Exception($"{name}: {nameof(winPanel)} can't be null");

        if (losePanel == null)
            throw new Exception($"{name}: {nameof(losePanel)} can't be null");

        if (gameController == null)
            throw new Exception($"{name}: {nameof(gameController)} can't be null");

        if (doorTeleportFadeAnimationController == null)
            throw new Exception($"{name}: {nameof(doorTeleportFadeAnimationController)} can't be null");

        if (teleportText == null)
            throw new Exception($"{name}: {nameof(teleportText)} can't be null");

        if (teleportImage == null)
            throw new Exception($"{name}: {nameof(teleportImage)} can't be null");

        doorTeleportAnimator = doorTeleportFadeAnimationController.GetComponent<Animator>();
        doorTeleportFadeAnimationController.FadeInEnded += FadeInEnded;
        doorTeleportFadeAnimationController.FadeOutEnded += FadeOutEnded;
        pausePanel.SetActive(false);
        gameController.PlayerCaught += StartPrison;
        gameController.LostGame += LostGame;
        gameController.WonGame += WonGame;

        Instance = this;
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
        if (Input.GetKeyDown(KeyCode.T) && teleportImage.activeSelf)
        {
            ShowDoorFade();
        }
    }

    public void StartGame()
    {        
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

    public void ShowDoorFade()
    {
        doorTeleportFadeAnimationController.gameObject.SetActive(true);
    }

    private void FadeInEnded(object sender, EventArgs e)
    {
        IsTeleporting = true;
        var target = teleportText.GetCurrentTarget();
        if (target != null)
        {
            gameController.TeleportPlayer(target);
        }
        IsTeleporting = false;
        doorTeleportAnimator.SetTrigger("End");
    }

    private void FadeOutEnded(object sender, EventArgs e)
    {
        doorTeleportFadeAnimationController.gameObject.SetActive(false);
    }

    public void ShowTeleportButton(Transform transform)
    {
        if (teleportImage == null)
        {
            return;
        }
        teleportImage.SetActive(true);
        teleportText.SetCurrentTarget(transform);
    }

    public void HideTeleportButton()
    {
        if(teleportImage == null)
        {
            return;
        }
        teleportImage.SetActive(false);
    }
}
