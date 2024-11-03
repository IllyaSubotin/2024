using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;   

    void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });

        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
           
        });

    }

}
