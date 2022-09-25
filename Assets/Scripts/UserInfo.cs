using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class UserInfo : MonoBehaviour
{
    [SerializeField] private Text username;
    [SerializeField] private Text userScore;

    private void Awake()
    {
        HandleUser();
    }

    private void HandleUser()
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;

        if (currentUser == null)
        {
            username.text = "Anonymous";
            userScore.text = "0";
        }
        else
        {
            GetUsername(currentUser.UserId);
            GetUserScore(currentUser.UserId);
        }
    }

    void GetUserScore(string userId)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users/" + userId + "/score").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("GetValueAsync encountered an error: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                userScore.text = "HighScore: " + snapshot.Value.ToString();

                PlayerPrefs.SetInt("HighScore", int.Parse(snapshot.Value.ToString()));
            }
        });
    }

    void GetUsername(string userId)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users/" + userId + "/username").GetValueAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsFaulted)
            {
                Debug.LogError("GetValueAsync encountered an error: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                username.text = "Username: " + snapshot.Value.ToString();

                PlayerPrefs.SetString("Username", snapshot.Value.ToString());
            }
        });
    }
}
