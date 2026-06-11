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
    public void SetMyGame()
    {
        Debug.Log(Time.timeScale);
    }
    public void BackMyGame() 
    {
        SceneManager.LoadSceneAsync("Menu_Test");  
    }
    public void Level1()
    {
        SceneManager.LoadSceneAsync("Level1");
    }
    public void Level2()
    {
        SceneManager.LoadSceneAsync("Level2");
    }
    public void Level3()
    {
        SceneManager.LoadSceneAsync("Level3");
    }
    public void Level4()
    {
        SceneManager.LoadSceneAsync("Level4");
    }
    public void CommunityLevel()
    {
        SceneManager.LoadSceneAsync("Community Level");
    }
}
