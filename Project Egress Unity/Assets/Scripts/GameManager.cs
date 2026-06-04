//using System.Numerics; - Branch Error
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Transform Respawn;
    
    private InputAction jump;
    public Tilemap map;
    public TileBase toPlace;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        jump = InputSystem.actions.FindAction("Jump");
    }

    public void UpdateRespawnPoint(UnityEngine.Vector3 newPosition)
    {
        Respawn.position = newPosition;
    }
    
    void Update()
    {
        /*
        if (jump.WasPressedThisFrame())
        {
            map.SetTile(new Vector3Int(1,1,1), toPlace);
            map.SetTile(new Vector3Int(0,1,1), toPlace);
            map.SetTile(new Vector3Int(-1,1,1), toPlace);
            map.SetTile(new Vector3Int(-2,1,1), toPlace);
        }
        */
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
