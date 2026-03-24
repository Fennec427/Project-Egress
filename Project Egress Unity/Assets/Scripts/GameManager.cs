using System.Numerics;
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
        jump = InputSystem.actions.FindAction("Jump");
    }
    void Update()
    {
        if (jump.WasPressedThisFrame())
        {
            map.SetTile(new Vector3Int(1,1,1), toPlace);
        }
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
