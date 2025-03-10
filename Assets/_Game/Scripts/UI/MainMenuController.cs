using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void Play()
    {
        LoadingScreen.Instance.StartLoading();
        
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        var gameplay = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        
        if (gameplay != null)
            gameplay.completed += operation =>
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
                LoadingScreen.Instance.Fade();
            };
    }

    public void Quit()
    {
        Application.Quit();
    }
}
