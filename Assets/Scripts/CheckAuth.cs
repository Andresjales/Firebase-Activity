using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;
using UnityEngine.SceneManagement;

public class CheckAuth : MonoBehaviour
{
    void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleCheckAuth; 
    }

    private void HandleCheckAuth(object sender, EventArgs e)
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
