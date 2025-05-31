using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionPanel != null && optionPanel.activeSelf)
            {
                optionPanel.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                Setting();
            }
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Setting()
    {
        Time.timeScale = 0f;
        optionPanel.SetActive(true);
    }

    public void Exit()
    {
        Time.timeScale = 0f;
        optionPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
