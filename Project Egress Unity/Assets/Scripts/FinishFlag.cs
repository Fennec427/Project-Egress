using UnityEngine;
using UnityEngine.SceneManagement;
public class FinishFlag : MonoBehaviour
{
    private Animator anim;   
    [SerializeField] private string nextSceneName; //A box in the inspector should allow a specific scene name to be typed.
     
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
        SceneManager.LoadScene(nextSceneName); //Uses the scne name from the inspector to actually teleport you.
    } 
}
