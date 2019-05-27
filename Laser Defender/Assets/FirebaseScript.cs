using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class FirebaseScript : MonoBehaviour {

    [SerializeField] InputField Email;
    [SerializeField] InputField Password;

    public void Login()
    {
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(Email.text, Password.text).
            ContinueWith((obj) =>           //Lambda expression.
            {
                SceneManager.LoadSceneAsync("Start Menu Scene");
            });
    }
    
    public void SignUp()
    {
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(Email.text, Password.text).
            ContinueWith((obj) =>
            {
                Debug.Log("New user with email ID: " + Email.text);
                Debug.Log("Login to play game!");
            });
    }
}
