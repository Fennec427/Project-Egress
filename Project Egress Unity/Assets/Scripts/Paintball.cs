using System;
using UnityEngine;

public class Paintball : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private int _spriteValue;
    private float moveSpeed;
    public int SpriteValue
    {
        get {return SpriteValue;}
        set
        {
            if(value > sprites.Length - 1 || value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), $"Sprite value must be at least 0 and at most {sprites.Length - 1}");
            else
                _spriteValue = value;
                print(_spriteValue);
        }
    }
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        sr.enabled = false;
        rb.simulated = false;
        print(sprites.Length);
    }

    public void Initialize(Vector2 direction, float speed)
    {
        sr.sprite = sprites[_spriteValue];
        moveSpeed = speed;
        gameObject.transform.up = direction.normalized;

        sr.enabled = true;
        rb.simulated = true;
        rb.AddForce(transform.up * moveSpeed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
