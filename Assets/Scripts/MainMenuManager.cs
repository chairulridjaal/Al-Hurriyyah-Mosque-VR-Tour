using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartFreeMode()
    {
        PlayerPrefs.SetString("Mode", "Free");
        SceneManager.LoadScene("FreeExploration");
    }

    public void StartNarratedMode()
    {
        PlayerPrefs.SetString("Mode", "Narrated");
        SceneManager.LoadScene("NarratedMode");
    }

    public void StartMainMenu()
    {
        PlayerPrefs.SetString("Mode", "VR");
        SceneManager.LoadScene("BasicScene");
    }
}
