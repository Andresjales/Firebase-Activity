using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;

public class SignUpButton : MonoBehaviour
{
    [SerializeField] InputField usernameField;
    [SerializeField] InputField emailField;
    [SerializeField] InputField passwordField;

    private Button signUpButton;
    DatabaseReference mDatabase;

    void Awake()
    {
        signUpButton = GetComponent<Button>();
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;

        signUpButton.onClick.AddListener(HandleSignUp);
    }

    private void HandleSignUp()
    {
        string username = usernameField.text;
        string email = emailField.text;
        string password = passwordField.text;

        if (!string.IsNullOrEmpty(username))
        {
            StartCoroutine(SignUpCoroutine(username, email, password));
        }
        else
        {
            Debug.LogError("Enter a valid username");

        }
    }

    IEnumerator SignUpCoroutine(string username, string email, string password)
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
            UserData data = new UserData();
            data.username = username;
            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.SetInt("HighScore", 0);
            string json = JsonUtility.ToJson(data);
            var usernameTask = mDatabase.Child("users").Child(newUser.UserId).SetRawJsonValueAsync(json);
            yield return new WaitUntil(() => usernameTask.IsCompleted);
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.Email, newUser.UserId);
            SceneManager.LoadScene("Game");
        }
    }
}
