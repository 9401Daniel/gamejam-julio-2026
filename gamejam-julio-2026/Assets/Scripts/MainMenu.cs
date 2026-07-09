using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Cambio de escena de menu a play
    public void Play()
    {
        SceneManager.LoadScene("Play");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }
}
