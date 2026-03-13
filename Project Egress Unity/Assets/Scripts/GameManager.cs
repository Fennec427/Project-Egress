using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Transform Respawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    public void PlayerDied()
    {
        SpawnPlayer();
    }
    void SpawnPlayer()
    {
        Instantiate(Player, Respawn.position, Quaternion.identity);
    }
}
