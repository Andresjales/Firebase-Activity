using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class SignUpButton : MonoBehaviour
{
    [SerializeField] InputField emailField;
    [SerializeField] InputField passwordField;

    private Button signUpButton;

    void Start()
    {
        signUpButton = GetComponent<Button>();

        signUpButton.onClick.AddListener(HandleSignUp);
    }

    private void HandleSignUp()
    {
        string email = emailField.text;
        string password = passwordField.text;

        StartCoroutine(SignUpCoroutine(email, password));
    }

    IEnumerator SignUpCoroutine(string email, string password)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        var signUpTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => signUpTask.IsCompleted);

        if (signUpTask.Exception != null)
        {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + signUpTask.Exception);
        }
        else
        {
            FirebaseUser newUser = signUpTask.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.Email, newUser.UserId);
            SceneManager.LoadScene("Game");
        }
    }
}
