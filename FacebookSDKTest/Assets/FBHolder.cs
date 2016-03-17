using UnityEngine;
using Facebook.Unity;

public class FBHolder : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Happens before initialization
    void Awake ()
    {
        FB.Init (SetInit, OnHideUnity);
    }

    private void SetInit ()
    {
        Debug.Log("FB loggin worked");

        if (FB.IsLoggedIn)
            Debug.Log("Logged in");
        else
            Debug.Log("Didint log in");
    }


    private void OnHideUnity (bool isGameShown)
    {
        // This is when fb pops in front of the game
        if (!isGameShown)
            Time.timeScale = 0;
    }
}
