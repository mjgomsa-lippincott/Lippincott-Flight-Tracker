using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ToggleScenes()
    {
        if (SceneManager.GetActiveScene().name == "GlobeScene")
        {
            SceneManager.LoadScene("PlaneScene");
        }
        else if (SceneManager.GetActiveScene().name == "PlaneScene")
        {
            SceneManager.LoadScene("GlobeScene");
        }
    }
}