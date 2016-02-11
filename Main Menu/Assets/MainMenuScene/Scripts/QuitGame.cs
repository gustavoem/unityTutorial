using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {
    
    public void Quit()
    {
        Debug.Log("Im at least being called");
        // If we are running in a standalone build of the game
#if UNITY_STANDALONE
        // Quit the application
        Application.Quit();
#endif

        // If we are running in the editor
#if UNITY_EDITOR
        // Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
