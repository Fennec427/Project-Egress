using UnityEngine;


public class Tile : MonoBehaviour
{
    public Sprite[] tiles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int rand = Random.Range(0,tiles.Length);
        gameObject.GetComponent<SpriteRenderer>().sprite = tiles[rand];
    }
}
