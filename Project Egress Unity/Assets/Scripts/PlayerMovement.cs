using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputAction move;
    float movespeed = 5f;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        move = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = move.ReadValue<Vector2>();
        //print(moveValue);
        rb.linearVelocity = new Vector2(moveValue.x*movespeed, rb.linearVelocity.y);

    }
    private void FixedUpdate()
    {
        //rb.linearVelocity = new Vector2(horizontalInput*movespeed, rb.linearVelocity.y);
        
    }
}
