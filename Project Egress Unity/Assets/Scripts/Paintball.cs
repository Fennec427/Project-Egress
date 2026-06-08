using System;
using UnityEngine;

public class Paintball : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private int spriteValue;
    private float moveSpeed;
    public int SpriteValue
    {
        get {return SpriteValue;}
        set
        {
            if(value > sprites.Length - 1 || value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), $"Sprite value must be at least 0 and at most {sprites.Length - 1}");
            else
                spriteValue = value;
        }
    }
    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Vector2 direction, float speed)
    {
        sr.sprite = sprites[spriteValue];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
