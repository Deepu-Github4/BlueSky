using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string gameSceneName = "Game";

    public void PlayButton()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ExitButton()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
