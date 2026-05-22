using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject menuPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPanel.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f; // FREEZE GAME
    }

    public void ResumeGame()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f; // RESUME GAME
    }

    public void Logout()
    {
        Time.timeScale = 1f; // IMPORTANT: reset before leaving
        SceneManager.LoadScene("LoginScene");
    }
}