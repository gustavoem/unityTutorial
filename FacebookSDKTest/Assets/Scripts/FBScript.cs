using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Facebook.Unity;

public class FBScript : MonoBehaviour
{

    public GameObject DialogLoggedIn;
    public GameObject DialogLoggedOut;
    public GameObject WelcomeMessageText;
    public GameObject UserProfileImage;

    public Text ScoresText;

    // Happens before initialization
    void Awake ()
    {
        FacebookManager.Instance.InitFB ();
        SetLoginMenu (FacebookManager.Instance.IsLoggedIn);
    }

    // Make the login when the login button is pressed
    public void FBLogin ()
    {
        FacebookManager.Instance.Login ();

        if (FacebookManager.Instance.IsLogginIn)
            StartCoroutine ("WaitForLogin");
        else
            SetLoginMenu (FacebookManager.Instance.IsLoggedIn);
    }
    

    // Controls UI logged ind and logged out UI
    void SetLoginMenu (bool isLoggedIn)
    {
        RectTransform user_pic_rectt = UserProfileImage.GetComponent<RectTransform> ();
        float user_pic_height = user_pic_rectt.rect.height;
        float user_pic_width = user_pic_rectt.rect.width;
        Debug.Log ("Entered SetLoginMenu. IsLoggedIn: " + isLoggedIn);
        if (isLoggedIn)
        {
            Debug.Log ("It is actually logged in"); 
            if (FacebookManager.Instance.IsQueryingFB ())
            {
                StartCoroutine ("WaitForAPIData", "name");
            }
            else {
                Debug.Log ("Im about to change the welcome message");
                Text welcome_message = WelcomeMessageText.GetComponent<Text> ();
                welcome_message.text = "Welcome, " + FacebookManager.Instance.UserName + "!";
            }

            if (FacebookManager.Instance.IsQueryingFB ())
            {
                StartCoroutine ("WaitForAPIData", "picture");
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
    IEnumerator WaitForAPIData (string reason)
    {
        Debug.Log ("Waiting to fetch info...");
        while (FacebookManager.Instance.IsQueryingFB ())
        {
            yield return null;
        }

        Debug.Log (reason);

        if (reason == "name" ||
            reason == "picture")
            SetLoginMenu (FacebookManager.Instance.IsLoggedIn);

        if (reason == "score_get")
            FillScoresText ();
    }

    // Makes the menu wait the manager to finish login
    IEnumerator WaitForLogin ()
    {
        Debug.Log ("Waiting to login...");
        while (FacebookManager.Instance.IsLogginIn)
        {
            yield return null;
        }

        SetLoginMenu (FacebookManager.Instance.IsLoggedIn);
    }

    // Set user score
    public void SetScore ()
    {
    }


    // Get user friends scores
    public void QueryScores ()
    {
        FacebookManager.Instance.FetchScores ();

        if (FacebookManager.Instance.IsQueryingFB ())
            StartCoroutine ("WaitForAPIData", "score_get");
        else
            FillScoresText ();          
    }

    void FillScoresText ()
    {
        ScoresText.text = "";
        foreach (var user in FacebookManager.Instance.scores_list)
            ScoresText.text = user.Key + ": " + user.Value + "\n" + ScoresText.text;
    }
}
