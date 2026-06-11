using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Paintball : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private int _spriteValue;
    private float moveSpeed;
    public int SpriteValue
    {
        get {return _spriteValue;}
        set
        {
            if(value > sprites.Length - 1 || value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), $"Sprite value must be at least 0 and at most {sprites.Length - 1}");
            else
                _spriteValue = value;
        }
    }
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //
    }

    public void Initialize(Vector2 direction, float speed, Vector3 goTo)
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        sr.enabled = false;
        rb.simulated = false;

        sr.sprite = sprites[_spriteValue];
        moveSpeed = speed;
        gameObject.transform.up = direction.normalized;

        gameObject.transform.position = goTo;

        sr.enabled = true;
        rb.simulated = true;
        rb.AddForce(transform.up * moveSpeed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] colliders = new Collider2D[10];
        if(collision.gameObject.TryGetComponent<Tilemap>(out _))
        {
            ContactPoint2D contact = collision.GetContact(0);
            Vector3 contactPoint = contact.point - contact.normal * 0.03f;
            int pointHits = Physics2D.OverlapCircle(contactPoint, 0.04f, ContactFilter2D.noFilter, colliders);
            foreach (var collider in colliders)
            {
                if(collider == null) continue;

                if(collider.gameObject.TryGetComponent<Tile>(out Tile tile))
                {
                    if (tile.Normal)
                    {
                        Vector3 rotation;
                        if(Mathf.Abs(contact.normal.x) > Mathf.Abs(contact.normal.y))
                        {
                            if(contact.normal.x > 0) rotation = new Vector3(0, 0, -90f); //collided right
                            else rotation = new Vector3(0, 0, 90f); //collided left
                        }
                        else
                        {
                            if(contact.normal.y > 0) rotation = new Vector3(0, 0, 0); //collided up
                            else rotation = new Vector3(0, 0, 180f); //collided down
                        }
                        tile.Paint(_spriteValue, rotation);
                    }
                }
            }
        }
        GameManager.activePaintballs.Remove(gameObject);
        Destroy(gameObject);
    }
}
