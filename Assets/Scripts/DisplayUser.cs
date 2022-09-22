using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using System;

public class DisplayUser : MonoBehaviour
{
    private Text userName;

    private void Start()
    {
        userName = GetComponent<Text>();

        HandleUser();
    }

    private void HandleUser()
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;

        if (currentUser == null)
        {
            userName.text = "Anonymous";
        }
        else
        {
            userName.text = currentUser.Email;
        }
    }
}
