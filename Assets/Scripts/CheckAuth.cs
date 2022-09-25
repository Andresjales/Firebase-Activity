using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class CheckAuth : MonoBehaviour
{
    private void Start()
    {
        HandleCheckAuth();
    }

    private void HandleCheckAuth()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
