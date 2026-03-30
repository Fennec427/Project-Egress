using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public BoxCollider2D trigger;
    public Animator animator; 

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager gm = Object.FindFirstObjectByType<GameManager>();
            gm.UpdateRespawnPoint(transform.position);
            GetComponent<Collider2D>().enabled = false; //disable flag retrigger
            if (animator != null)
            {
                animator.SetTrigger("Enabled"); 
            }
        }
    }
}