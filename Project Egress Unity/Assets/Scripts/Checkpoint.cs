using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public BoxCollider2D trigger;

    private void OTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager gm = Object.FindFirstObjectByType<GameManager>();
            gm.UpdateRespawnPoint(transform.position);
            GetComponent<Collider2D>().enabled = false;
        }

    }
}
