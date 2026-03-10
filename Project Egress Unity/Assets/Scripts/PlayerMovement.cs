using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputAction move;
    private InputAction jump; //N - J
    [SerializeField] private float movespeed = 5f; // Just Added serialize field  technically N - J
    [SerializeField] private float jumpForce = 10f; // Force applied when jumping N - J
    private bool canJump;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");

        move.Enable(); // N - J
        jump.Enable(); // N - J

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = move.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveValue.x*movespeed, rb.linearVelocity.y);

        // WasPressedThisFrame is ideal for jumping so it only triggers once per tap
        if (jump.WasPressedThisFrame()) //All stuff from here to 4 lines below is N - J
        {
            // Simple Jump: sets the vertical velocity directly
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }       

    }
    private void FixedUpdate()
    {
        // coyote time + jump checking here
    }
}
