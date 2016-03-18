using UnityEngine;
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
        _instance = this;
        IsLoggedIn = true;
    }
}
