using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject firstSelectedButton;
    [SerializeField] private string nextSceneName;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject retryButton;

    public bool IsPaused => menuPanel.activeSelf;

    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        Pause();
    }

    public void WinGame()
    {
        Time.timeScale = 0f;
        winPanel.SetActive(true);
    }

    public void LoseGame()
    {
        Time.timeScale = 0f;
        losePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(retryButton);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        menuPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void TogglePause()
    {
        if (IsPaused)
        {
            Resume();
        }
        else 
        {
            Pause();
        }
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}