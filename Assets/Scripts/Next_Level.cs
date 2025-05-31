using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class Next_Level : MonoBehaviour
{
    public string[] levelNames; 
    public static List<string> playedLevels = new List<string>(); 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextUniqueLevel();
            Debug.Log(GameManager.levelCount);
        }
    }

    void LoadNextUniqueLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        GameManager.levelCount++;
        
        if (!playedLevels.Contains(currentScene))
        {
            playedLevels.Add(currentScene);
        }

        
        List<string> availableLevels = levelNames
            .Where(name => !playedLevels.Contains(name))
            .ToList();

        if (availableLevels.Count == 0)
        {
            Debug.Log("Đã hoàn thành tất cả level. Reset danh sách và chơi lại từ level khác.");

           
            playedLevels.Clear();
            playedLevels.Add(currentScene); 

            availableLevels = levelNames
                .Where(name => name != currentScene)
                .ToList();
        }

        if (availableLevels.Count == 0)
        {
            Debug.LogWarning("Không còn level nào khác để chuyển!");
            return;
        }

        string nextLevel = availableLevels[Random.Range(0, availableLevels.Count)];
        SceneManager.LoadScene(nextLevel);
    }
}
