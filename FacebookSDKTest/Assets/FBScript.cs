using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System.Collections.Generic;

public class FBScript : MonoBehaviour
{

    public GameObject DialogLoggedIn;
    public GameObject DialogLoggedOut;
    public GameObject WelcomeMessageText;
    public GameObject UserProfileImage;

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
        RectTransform user_pic_rectt = UserProfileImage.GetComponent<RectTransform> ();
        double user_pic_height = user_pic_rectt.rect.height;
        double user_pic_width = user_pic_rectt.rect.width;

        if (isLoggedIn)
        {
            FB.API ("/me?fields=first_name", HttpMethod.GET, DisplayUserName);
            FB.API ("/me/picture?type=square&height=" + user_pic_height + "&width=" + user_pic_width, HttpMethod.GET, DisplayUserPicture);
        }

        DialogLoggedOut.SetActive (!isLoggedIn);
        DialogLoggedIn.SetActive (isLoggedIn);
    }


    // Changes the welcome message to include user name
    void DisplayUserName (IResult result)
    {
        if (result.Error == null)
        {
            Text welcome_message = WelcomeMessageText.GetComponent<Text> ();
            welcome_message.text = "How are you, " + result.ResultDictionary ["first_name"] + "?";
        }
        else
        {
            Debug.Log ("Couldn't get user name");
            Debug.Log (result.Error);
        }
    }

    
    // Displays the user profile picture in a login
    void DisplayUserPicture (IGraphResult result)
    {
        if (result.Error == null)
        {
            Image profile_image = UserProfileImage.GetComponent<Image> ();
            RectTransform user_pic_rectt = UserProfileImage.GetComponent<RectTransform> ();
            float pic_height = user_pic_rectt.rect.height;
            float pic_width = user_pic_rectt.rect.width;

            profile_image.sprite = Sprite.Create (result.Texture, new Rect(0, 0, pic_height, pic_width), new Vector2 ());
        }
        else
        {
            Debug.Log ("Couldn't get user profile picture");
            Debug.Log (result.Error);
        }
    }
}
