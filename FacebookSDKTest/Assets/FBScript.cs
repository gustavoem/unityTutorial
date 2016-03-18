using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System.Collections.Generic;

public class FBScript : MonoBehaviour
{

    public GameObject DialogLoggedIn;
    public GameObject DialogLoggedOut;
    public GameObject WelcomeMessageText;

    // Happens before initialization
    void Awake ()
    {
        FB.Init (SetInit, OnHideUnity);
    }


    // Init of FB
    private void SetInit ()
    {
        Debug.Log ("Started FB init");
        if (FB.IsLoggedIn)
            Debug.Log ("They already logged in");
        else
            Debug.Log ("They haven't logged in");

        SetLoginMenu (FB.IsLoggedIn);
    }


    // When FB goes over your app (game is not showing)
    private void OnHideUnity (bool isGameShown)
    {
        if (!isGameShown)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void FBLogin ()
    {
        List<string> permissions = new List<string> ();
        permissions.Add ("public_profile");
        FB.LogInWithReadPermissions (permissions, AuthCallBack);
    }


    // This is run after logged in
    void AuthCallBack (IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log ("Ops, we've got an error at the loggin.");
            Debug.Log (result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
                Debug.Log ("Successful login");
            else
                Debug.Log ("Failded login");

            SetLoginMenu (FB.IsLoggedIn);
        }
    }

    // Controls UI logged ind and logged out UI
    void SetLoginMenu (bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            FB.API ("/me?fields=first_name", HttpMethod.GET, DisplayUserName);
        }

        DialogLoggedOut.SetActive (!isLoggedIn);
        DialogLoggedIn.SetActive (isLoggedIn);
    }


    // Callback to set username when there's a login
    void DisplayUserName (IResult result)
    {
        Text welcome_message = WelcomeMessageText.GetComponent<Text> ();

        if (result.Error == null)
        {
            welcome_message.text = "How are you, " + result.ResultDictionary ["first_name"] + "?";
        }
        else
        {
            Debug.Log ("Couldn't get user name");
            Debug.Log (result.Error);
        }
    }
}
