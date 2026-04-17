using UnityEngine;


public class Tile : MonoBehaviour
{
    public Sprite[] tiles;
    [Header("Tile Detection")]
    [SerializeField] private GameObject[] detectors;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float extendAmt = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Randomization of sprites
        int rand = Random.Range(0, 101);
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

        // Extend hitbox to overlap on other tiles
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        /*
        Collider2D[] detected;
        foreach(var detector in detectors)
        {
            detected = Physics2D.OverlapCircleAll(detector.transform.position, 0.07f, groundLayer); // this may eat up all computer memory when there are a lot of tiles at once
            foreach(var detectedObject in detected)
            {
                if(detectedObject == collider) // check if the detector found it's own tile
                {
                    continue;
                }
                else
                {
                    if(detector.name == "12") // top
                    {
                        collider.size = new Vector2(collider.size.x, collider.size.y + extendAmt);
                        collider.offset = new Vector2(collider.offset.x, collider.offset.y + (extendAmt / 2f));
                    }
                    else if(detector.name == "3") // right
                    {
                        collider.size = new Vector2(collider.size.x + extendAmt, collider.size.y);
                        collider.offset = new Vector2(collider.offset.x + (extendAmt / 2f), collider.offset.y);
                    }
                    else if(detector.name == "6") // bottom
                    {
                        collider.size = new Vector2(collider.size.x, collider.size.y + extendAmt);
                        collider.offset = new Vector2(collider.offset.x, collider.offset.y - (extendAmt / 2f));
                    }
                    else if(detector.name == "9") // left
                    {
                        collider.size = new Vector2(collider.size.x + extendAmt, collider.size.y);
                        collider.offset = new Vector2(collider.offset.x - (extendAmt / 2f), collider.offset.y);
                    }
                }
            }
            detected = null; // reset "detected" for next iteration
        }
        */
    }
}
