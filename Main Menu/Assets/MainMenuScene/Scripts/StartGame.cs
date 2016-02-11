using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGame : MonoBehaviour {

    public int sceneToStart = 1; // Defines the scene that is going to follow the menu scene. You can change this in the build setup

	// Use this for initialization
	void Start () {
	
	}


    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneToStart);
    }


	// Update is called once per frame
	void Update () {
	
	}
}
