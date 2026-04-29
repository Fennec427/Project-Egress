using UnityEngine;

public class Teleporter : MonoBehaviour 
{
    [Header("Settings")] 
    public Teleporter destination; 
    public float teleportDelay = 1.0f; //Seconds between each teleport
    [Range(0.1f, 1.0f)] 
    public float topDetectionThreshold = 0.5f; //Top collision for telporting
    private static float lastTeleportTime = 0f; // Static so it can apply to all teleporters 
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && Time.time > lastTeleportTime + teleportDelay)
        {
            /*
            //Meant to check if the player collision is above the teleporter 
            Vector2 contactPoint = collision.transform.position; 
            Vector2 teleporterPoint = transform.position;  

            //Only works if players feet are above teleporter's center 
            if (contactPoint.y > teleporterPoint.y + topDetectionThreshold)
            {
                TeleportPlayer(collision.gameObject);
            }
            */
            TeleportPlayer(collision.gameObject);
        }
    } 
     
    private void TeleportPlayer(GameObject player)
    {
        if (destination != null)
        {
            lastTeleportTime = Time.time; 
             
            //Spawn slightly above after teleporting to avoid getting stuck 
            Vector3 spawnOffset = new Vector3(0, 1.2f, 0); 
            player.transform.position = destination.transform.position + spawnOffset; 
             
            Debug.Log("Teleporter Online");
        }
    } 


}
