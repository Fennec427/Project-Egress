using UnityEngine;


public class Tile : MonoBehaviour
{
    public Sprite[] tiles;
    public string tileName;
    [SerializeField] private bool normal;
    [SerializeField] private float bounceForce = 3;
    public float BounceForce => bounceForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(normal)
        {
            int rand = Random.Range(0, 101);
            print(rand);
            if(rand >= 0 && rand <= 90)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = tiles[0]; // Normal grey tile
            }
            if(rand > 90 && rand <= 95)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = tiles[1]; // Clean tile
            }
            if(rand > 95 && rand <= 100)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = tiles[2]; // Rusted tile
            }
        }
    }
}
