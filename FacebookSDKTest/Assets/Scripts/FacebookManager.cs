using UnityEngine;
using Facebook.Unity;
using System.Collections;

// This code is inspired in the following tutorial:
// http://greyzoned.com/tutorials/facebook-sdk-7-4-0-in-unity-5-3-tutorial-singletons-inviting-sharing/


public class FacebookManager : MonoBehaviour {

    // An instance of the class
    private static FacebookManager _instance;

    
    // Instance getter and setter
    public static FacebookManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject fbm = new GameObject ("FBManger");
                fbm.AddComponent<FacebookManager> ();
            
            }

            return _instance;
        }
    }


    // Stores if FB is logged in or not. Should be the same as FB.IsLoggedIn
    public bool IsLoggedIn {
        get;
        set;
    }


    // Code run when starting the script component
    void Awake ()
    {
        // Not very sure of how this works...
        Debug.Log ("FBManager Awake is being called");
        DontDestroyOnLoad (this.gameObject);
        _instance = this;
    }


    // Initiates Facebook API
    public void InitFB ()
    {
        if (!FB.IsInitialized)
            FB.Init (SetInit, OnHideUnity);
        else
            IsLoggedIn = FB.IsLoggedIn;
    }


    // Init of FB
    private void SetInit ()
    {
        Debug.Log ("Started FB init");
        if (FB.IsLoggedIn)
            Debug.Log ("They already logged in");
        else
            Debug.Log ("They haven't logged in");

        IsLoggedIn = FB.IsLoggedIn;
    }


    // When FB goes over your app (game is not showing)
    private void OnHideUnity (bool isGameShown)
    {
        if (!isGameShown)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }


    //private void SetUserInfo (Result)
}
