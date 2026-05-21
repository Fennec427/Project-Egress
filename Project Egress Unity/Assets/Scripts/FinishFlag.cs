using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections; //For IEnumerator to be able to be used.
public class FinishFlag : MonoBehaviour
{
    private Animator anim;   
    [SerializeField] private string nextSceneName; //A box in the inspector should allow a specific scene name to be typed. 

    [SerializeField] private float delayTime = 3.5f; //A time interval can be typed here or overidden through the inspector.

    //public GameObject
     
    void Start()
    {
        anim = GetComponent<Animator>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Player")) //Name of tag for the main character.
        {
            anim.SetTrigger("Finish"); //Name of the animation trigger for the finish flag.
        }
    } 

    public void LoadNextLevel() { //"Custom function" that acts as a way to allow the animation to trigger. Anything could be next to the parenthesis.   
        
        StartCoroutine(DelaySceneLoad()); //Tells the game to run the following function alongside the game loop. 

    }   

    private IEnumerator DelaySceneLoad() //A function that can pause its own execution using a timer!
    { 
        Time.timeScale = 0.5f; //Slow speed variable here!
 
        yield return new WaitForSecondsRealtime(delayTime); //The actual pause that calls to the variable "delayTime."

        Time.timeScale = 0;


    }

    public void NextLevel()
    {
        Time.timeScale = 1.0f; //Resets the speed back to normal!
        SceneManager.LoadScene(nextSceneName); //Uses the scne name from the inspector to actually teleport you. 
    }

    public void LevelSelect()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu_Test2");
    }

}
