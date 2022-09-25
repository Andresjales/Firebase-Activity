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
        FirebaseDatabase.DefaultInstance.GetReference("users/" + userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("GetValueAsync encountered an error: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                var data = (Dictionary<string, object>)snapshot.Value;

                userScore.text = "HighScore: " + data["score"].ToString();

                PlayerPrefs.SetInt("HighScore", int.Parse(data["score"].ToString()));
            }
        });
    }

    void GetUsername(string userId)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users/" + userId).GetValueAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsFaulted)
            {
                Debug.LogError("GetValueAsync encountered an error: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                var data = (Dictionary<string, object>)snapshot.Value;

                username.text = "Username: " + data["username"].ToString();

                PlayerPrefs.SetString("Username", data["username"].ToString());
            }
        });
    }
}
