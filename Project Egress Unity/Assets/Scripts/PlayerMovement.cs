using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputAction move;
    public InputAction jump; //N - J
       [SerializeField] float movespeed = 5f; // Just Added serialize field  technically N - J
        [SerializeField] float jumpForce = 10f; // Force applied when jumping N - J
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump"); // N - J

        move.Enable(); // N - J
        jump.Enable(); // N - J

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = move.ReadValue<Vector2>();
        //print(moveValue);
        rb.linearVelocity = new Vector2(moveValue.x*movespeed, rb.linearVelocity.y);

         // 2. Jump Logic - Both comment are N - J
        // WasPressedThisFrame is ideal for jumping so it only triggers once per tap
        if (jump.WasPressedThisFrame()) //All stuff from here to 4 lines below is N - J
        {
            // Simple Jump: sets the vertical velocity directly
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }       

    }
    private void FixedUpdate()
    {
        //rb.linearVelocity = new Vector2(horizontalInput*movespeed, rb.linearVelocity.y);
        
    }
}
