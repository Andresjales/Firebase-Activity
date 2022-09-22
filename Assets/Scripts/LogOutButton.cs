using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogOutButton : MonoBehaviour
{
    private Button logOutButton;

    private void Start()
    {
        logOutButton = GetComponent<Button>();

        logOutButton.onClick.AddListener(LogOut);
    }

    private void LogOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene("MainMenu");
    }
}
