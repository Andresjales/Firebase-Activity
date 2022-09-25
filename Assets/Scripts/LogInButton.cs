using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class LogInButton : MonoBehaviour
{
    [SerializeField] InputField emailField;
    [SerializeField] InputField passwordField;

    private Button logInButton;

    void Awake()
    {
        logInButton = GetComponent<Button>();

        logInButton.onClick.AddListener(HandleLogIn);
    }

    private void HandleLogIn()
    {
        string email = emailField.text;
        string password = passwordField.text;

        StartCoroutine(LogInCoroutine(email, password));
    }

    IEnumerator LogInCoroutine(string email, string password)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        var logInTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => logInTask.IsCompleted);

        if (logInTask.Exception != null)
        {
            Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + logInTask.Exception);
        }
        else
        {
            FirebaseUser newUser = logInTask.Result;
            Debug.LogFormat("User loged in successfully: {0} ({1})", newUser.Email, newUser.UserId);
            SceneManager.LoadScene("Game");
        }
    }
}
