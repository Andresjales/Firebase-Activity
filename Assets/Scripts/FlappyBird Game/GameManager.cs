using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text[] players;

    public GameObject gameOverCanvas;

    DatabaseReference mDatabase;
    string userId;

    void Awake()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        Time.timeScale = 1;
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);

        SetNewScore();

        Time.timeScale = 0;
    }
    public void Replay()
    {
        SceneManager.LoadScene("Game");
    }

    public void GetLeaderboard()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("score").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("GetValueAsync encountered an error: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int i = 0;
                foreach (DataSnapshot userDoc in snapshot.Children.Reverse())
                {
                    players[i].text = (i + 1) + ". " + userDoc.Child("username").Value.ToString() + " = " + userDoc.Child("score").Value.ToString();
                    i++;
                }
            }
        });
    }

    private void SetNewScore()
    {
        int actualScore = Score.score;

        if (actualScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", actualScore);

            UserData data = new UserData();
            data.username = PlayerPrefs.GetString("Username");
            data.score = actualScore;

            string json = JsonUtility.ToJson(data);
            mDatabase.Child("users").Child(userId).SetRawJsonValueAsync(json);
        }
    }
}

[System.Serializable]
public class UserData
{
    public string username;
    public int score;
}
