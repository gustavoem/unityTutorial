using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System.Collections.Generic;
using System.Collections;

public class FBScript : MonoBehaviour
{

    public GameObject DialogLoggedIn;
    public GameObject DialogLoggedOut;
    public GameObject WelcomeMessageText;
    public GameObject UserProfileImage;

    // Happens before initialization
    void Awake ()
    {
        FacebookManager.Instance.InitFB ();
        SetLoginMenu (FacebookManager.Instance.IsLoggedIn);
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
            {
                Debug.Log ("Successful login");
                FacebookManager.Instance.FetchUserProfile ();
            }
            else
                Debug.Log ("Failded login");

            // not cool... this whole login should be inside the manager
            FacebookManager.Instance.IsLoggedIn = FB.IsLoggedIn;

            SetLoginMenu (FB.IsLoggedIn);
        }
    }

    // Controls UI logged ind and logged out UI
    void SetLoginMenu (bool isLoggedIn)
    {
        RectTransform user_pic_rectt = UserProfileImage.GetComponent<RectTransform> ();
        //double user_pic_height = user_pic_rectt.rect.height;
        //double user_pic_width = user_pic_rectt.rect.width;
        Debug.Log ("Entered SetLoginMenu. IsLoggedIn: " + isLoggedIn);
        if (isLoggedIn)
        {
            Debug.Log ("It is actually logged in"); 
            if (FacebookManager.Instance.UserName == null)
            {
                StartCoroutine ("WaitForProfileName");
            }
            else {
                Debug.Log ("Im about to change the welcome message");
                Text welcome_message = WelcomeMessageText.GetComponent<Text> ();
                welcome_message.text = "Welcome, " + FacebookManager.Instance.UserName + "!";
            }

            if (FacebookManager.Instance.UserPicture == null)
            {
                StartCoroutine ("WaitForProfilePicture");
            }
            else {
                Image user_pic = UserProfileImage.GetComponent<Image> ();
                user_pic.sprite = FacebookManager.Instance.UserPicture;
            }
        }

        DialogLoggedOut.SetActive (!isLoggedIn);
        DialogLoggedIn.SetActive (isLoggedIn);
    }


    // Makes the menu wait to the profile name to be fetched before trying to display it
    IEnumerator WaitForProfileName ()
    {
        Debug.Log ("Waiting to fetch info...");
        while (FacebookManager.Instance.UserName == null)
        {
            yield return null;
        }

        SetLoginMenu (FacebookManager.Instance.IsLoggedIn);
    }

    // Makes the menu wait to the profile picture to be fetched before trying to display it
    IEnumerator WaitForProfilePicture ()
    {
        Debug.Log ("Waiting to fetch info...");
        while (FacebookManager.Instance.UserPicture == null)
        {
            yield return null;
        }

        SetLoginMenu (FacebookManager.Instance.IsLoggedIn);
    }
}
