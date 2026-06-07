using UnityEngine;

public class BuzzsawRotation : MonoBehaviour
{
    [SerializeField] private float speed = -100f;
    private PolygonCollider2D col;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
