using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameData;

public class StartMenu : MonoBehaviour
{

    public TMP_InputField nameInputField;  // The input field where the player types their name
    public Button startButton;        // Button to submit the name
    public Button clearHighScoreButton; // Button to clear the high score
    private HighScoreManager highScoreManager;
    public TMP_Text nameAndScoreText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         highScoreManager = HighScoreManager.Instance; // Get reference to HighScoreManager

       // Set up the button click listener
        startButton.onClick.AddListener(StartButtonClicked);

         clearHighScoreButton.onClick.AddListener(OnClearHighScoreButtonClicked); // Set up clear high score button listener

         // Load and display the player's name and high score
        LoadPlayerNameAndHighScore();

    }
    
 public void StartButtonClicked()
    {
        string name = nameInputField.text;
    PlayerData.playerName = name;  // Save to static class

    Debug.Log("Player's name is: " + name);
    SceneManager.LoadScene("main");
    }

    //clear score button
public void OnClearHighScoreButtonClicked()
{
    // Call the method to clear the high score
    highScoreManager.ClearHighScore();

// Load and display the player's name and high score
        LoadPlayerNameAndHighScore();
}

  // This method will load and display the player's name and the high score
     private void LoadPlayerNameAndHighScore()
    {
        // Get the player's name (will be saved when the player enters it)
        string playerName = PlayerData.playerName;

        // Load the high score data
        HighScoreData highScore = highScoreManager.LoadHighScore();
        
        // Combine player name and high score into one string and display it
        string displayText = $"High Score:\n {highScore.playerName} = {highScore.score}";

        if (nameAndScoreText != null)
        {
            nameAndScoreText.text = displayText;
        }
    }

}

namespace GameData
{
    public static class PlayerData
{
    public static string playerName;
}


}
