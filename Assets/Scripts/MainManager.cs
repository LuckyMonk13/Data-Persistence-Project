using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameData;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    public string playerName;

    private HighScoreManager highScoreManager;
    public Text newRecordText;

    
    // Start is called before the first frame update
    void Start()
    {
newRecordText.text = "";  // Clear the text

playerName = PlayerData.playerName;

ScoreText.text = $"Score: {playerName} = {m_Points}";

highScoreManager = HighScoreManager.Instance; 

HighScoreData highScore = highScoreManager.LoadHighScore();
BestScoreText.text = $"Best Score: {highScore.playerName} = {highScore.score}";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Start Menu");
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score: {playerName} = {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

          // Call TryUpdateHighScore when the game is over
    highScoreManager.TryUpdateHighScore(playerName, m_Points);

    // Check if there is a new high score when the main menu is loaded
        CheckForNewHighScore();

        HighScoreData highScore = highScoreManager.LoadHighScore();
BestScoreText.text = $"Best Score: {highScore.playerName} = {highScore.score}";
    }

     private void CheckForNewHighScore()
    {
        Debug.Log("check for new high score happened. High Score variable = " + PlayerPrefs.GetInt("NewHighScore", 0));
        HighScoreData highScoreData = HighScoreManager.Instance.LoadHighScore();

        // Check if the high score has changed (i.e., a new record has been set)
        if (PlayerPrefs.GetInt("NewHighScore", 0) == 1)  // 1 means new high score was set
        {
            // Show the "New Record!" message
            if (newRecordText != null)
            {
                newRecordText.text = "New Record!";
                StartCoroutine(HideNewRecordText());
            }

            // Reset the flag in PlayerPrefs
            PlayerPrefs.SetInt("NewHighScore", 0);
        }
    }

    private IEnumerator HideNewRecordText()
    {
        yield return new WaitForSeconds(4f);  // Display for 2 seconds
        if (newRecordText != null)
        {
            newRecordText.text = "";  // Clear the text after the delay
        }
    }

}
