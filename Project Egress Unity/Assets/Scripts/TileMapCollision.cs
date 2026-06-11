using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapCollision : MonoBehaviour
{
    Tilemap tilemap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject GetCollisionLocation(Collision2D collision)
    {
        Vector3 hitPoint = collision.GetContact(0).point;
        print(hitPoint);
        Vector3Int cellPoint = tilemap.WorldToCell(hitPoint);
        print(cellPoint);
        GameObject hitTile = tilemap.GetInstantiatedObject(cellPoint);
        return hitTile;
    }
}
