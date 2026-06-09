using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;


public class Tile : MonoBehaviour
{
    [Tooltip("DO NOT CHANGE, list of sprites to randomize through")] public Sprite[] tiles;
    [Tooltip("Important for special tiles")] public string tileName;
    [SerializeField] [Tooltip("Checkmark this if this tile does not apply special effects")] private bool normal;
    public bool Normal => normal;

    [Header("Red Tile Stats")]
    [SerializeField] [Tooltip("Sets the new max speed of the player after they touch the tile")] private float speedBoost = 10f;
    public float SpeedBoost => speedBoost;

    [Header("Orange Tile Stats")]
    [SerializeField] [Tooltip("How much force is added to the player after they hit this tile")] private float bounceForce = 3;
    public float BounceForce => bounceForce;
    [SerializeField] [Tooltip("The max Y force the player can have after Bounce Force is applied")] private float maxBounceForce = 10f;
    public float MaxBounceForce => maxBounceForce;

    [Header("Green Tile Stats")]
    [SerializeField] [Tooltip("How long the player will stop accepting input for, to counteract players walking right off after rotation")] private float movementDisableTime = 0.07f;
    public float MovementDisableTime => movementDisableTime;

    [Header("Painting")]
    [SerializeField] private PaintDroop paintDroop;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(normal)
        {
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
            //paintIndex[0] = new object[] {redPaint};
            //print(((Sprite[])paintIndex[0][0]).Length);
        }
    }

    public void Paint(int paintType, Vector3 rotation)
    {
        PaintDroop droop = Instantiate(paintDroop);
        droop.Initialize(paintType, rotation, transform.position);
        GameManager.activePaintDroops.Add(droop.gameObject);
    }
}
