using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Quit : MonoBehaviour
{
   void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
    }
    private void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
