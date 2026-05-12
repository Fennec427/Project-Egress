using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStuff : MonoBehaviour
{
    public void PlayMyGame() 
    {
        SceneManager.LoadSceneAsync("Menu_Test2");  
    }
    public void QuitMyGame()
    {
        Application.Quit();
    }
    public void BackMyGame() 
    {
        SceneManager.LoadSceneAsync("Menu_Test");  
    }
}
