using UnityEngine;
using System.IO;
using UnityEngine.UI;
using JetBrains.Annotations;

[System.Serializable]
public class HighScoreData
{
    public string playerName;
    public int score;
}
public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;
    private string filePath;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
            filePath = Path.Combine(Application.persistentDataPath, "highscore.json");
        }
        else
        {
            Destroy(gameObject); // Ensure only one exists
        }
    }

    public HighScoreData LoadHighScore()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<HighScoreData>(json);
        }

        return new HighScoreData { playerName = "No One", score = 0 };
    }

    public void SaveHighScore(string playerName, int score)
    {
        HighScoreData data = new HighScoreData { playerName = playerName, score = score };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public void TryUpdateHighScore(string playerName, int newScore)
    {
        HighScoreData current = LoadHighScore();

        if (newScore > current.score)
        {
            Debug.Log($"🎉 New High Score by {playerName}: {newScore}");
            SaveHighScore(playerName, newScore);

            // Set the new high score flag in PlayerPrefs
        PlayerPrefs.SetInt("NewHighScore", 1);  // Flag indicating a new high score
        }
        else 
        {
            PlayerPrefs.SetInt("NewHighScore", 0);
        }
        
    }

//clear score button
public void ClearHighScore()
{
    // Check if the high score file exists
    if (File.Exists(filePath))
    {
        File.Delete(filePath);  // Delete the high score file
    }

    // Optionally, you can save a default high score after clearing.
    SaveHighScore("No One", 0);  // Reset the high score to default values
}

}
