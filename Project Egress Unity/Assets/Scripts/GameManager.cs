using System.Numerics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Transform Respawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdateRespawnPoint(UnityEngine.Vector3 newPosition)
    {
        Respawn.position = newPosition;
    }

    // Update is called once per frame
    public void PlayerDied()
    {
        SpawnPlayer();
    }
    void SpawnPlayer()
    {
    
        Player.GetComponent<Rigidbody2D>().linearVelocity = UnityEngine.Vector2.zero;
        Player.transform.position = Respawn.position;
    }
}
