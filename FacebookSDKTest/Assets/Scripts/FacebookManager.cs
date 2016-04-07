using UnityEngine;
using Facebook.Unity;
using System.Collections.Generic;
using Facebook.MiniJSON;

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
    public bool IsLoggedIn { get; set; }

    // It's true if we are in the middle of a authentication
    public bool IsLogginIn = false;

    // Counts the number of queries currently being fetched
    public int CurrentQueries = 0;

    // Stores the user name
    public string UserName { get; set; }

    // Stores the user profile picture
    public Sprite UserPicture { get; set; }

    // Stores the list of scores
    public Dictionary<string, double> scores_list { get; set; }


    // Code run when starting the script component
    void Awake ()
    {
        // Not very sure of how this works...
        Debug.Log ("FBManager Awake is being called");
        DontDestroyOnLoad (this.gameObject);
        _instance = this;
    }


    // Returns true if there's data being fetched
    public bool IsQueryingFB ()
    {
        return CurrentQueries > 0;
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
        {
            Debug.Log ("They already logged in");
            FetchUserProfile ();
        }
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


    public void FetchUserProfile ()
    {
        CurrentQueries++;
        FB.API ("/me?fields=first_name", HttpMethod.GET, FetchUserName);
        //FB.API ("/me/picture?type=square&height=" + user_pic_height + "&width=" + user_pic_width, HttpMethod.GET, DisplayUserPicture);
        CurrentQueries++;
        FB.API ("/me/picture?type=square&height=" + "200" + "&width=" + "200", HttpMethod.GET, FetchUserPicture);
    }

    // Changes the welcome message to include user name
    void FetchUserName (IResult result)
    {
        if (result.Error == null)
        {
            UserName = result.ResultDictionary["first_name"].ToString ();
            Debug.Log ("Fetched user name: " + UserName);
        }
        else
        {
            Debug.Log ("Couldn't get user name");
            Debug.Log (result.Error);
        }
        CurrentQueries--;
    }


    // Displays the user profile picture in a login
    void FetchUserPicture (IGraphResult result)
    {
        if (result.Error == null)
        {
            //Image profile_image = UserProfileImage.GetComponent<Image> ();
            //RectTransform user_pic_rectt = UserProfileImage.GetComponent<RectTransform> ();
            //float pic_height = user_pic_rectt.rect.height;
            //float pic_width = user_pic_rectt.rect.width;

            //UserPicture = Sprite.Create (result.Texture, new Rect (0, 0, pic_height, pic_width), new Vector2 ());
            UserPicture = Sprite.Create (result.Texture, new Rect (0, 0, 200, 200), new Vector2 ());
        }
        else
        {
            Debug.Log ("Couldn't get user profile picture");
            Debug.Log (result.Error);
        }
        CurrentQueries--;
    }


    public void Login ()
    {
        IsLogginIn = true;

        List<string> permissions = new List<string> ();
        permissions.Add ("public_profile");
        permissions.Add ("user_games_activity");
        permissions.Add ("user_friends");
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
                FetchUserProfile ();
            }
            else
                Debug.Log ("Failded login");

            IsLoggedIn = FB.IsLoggedIn;
        }
        IsLogginIn = false;
    }

    // Get scores in a string
    public void FetchScores ()
    {
        CurrentQueries++;
        FB.API ("/app/scores", HttpMethod.GET, ScoresCallback);
    }

    private void ScoresCallback (IResult result)
    {
        scores_list = new Dictionary<string, double> ();

        Dictionary<string, object> result_dict = Json.Deserialize (result.RawResult) as Dictionary<string, object>;
        List<object> data = result_dict["data"] as List<object>;
        object[] array_data = data.ToArray ();
        foreach (var item in array_data)
        {
            Dictionary<string, object> user_dictionary = item as Dictionary<string, object>;
            string name = ((Dictionary<string, object>) user_dictionary["user"])["name"].ToString ();
            double score = double.Parse (user_dictionary["score"].ToString ());
            scores_list.Add (name, score);
        }
        CurrentQueries--;
    }
}
