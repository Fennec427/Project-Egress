using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Movement inputs
    private InputAction move;
    private InputAction jump; //N - J

    // Movement stats
    [SerializeField] private float movespeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    // Ground detection
    [SerializeField] private Transform groundCheck; // The "sensor" object at your feet
    [SerializeField] private float checkRadius = 0.2f; // Size of the detection circle
    [SerializeField] private LayerMask groundLayer; // Object layer for sensor to detect
    private bool canJump;

    private Rigidbody2D rb; // Used to set movement

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");

        move.Enable();
        jump.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        canJump = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer); // Update canJump by checking if sensor is touching the groundLayer

        Vector2 moveValue = move.ReadValue<Vector2>();
        if(moveValue.x != 0)
        {
            rb.linearVelocity = new Vector2(moveValue.x*movespeed, rb.linearVelocity.y); // Move left-right
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.05f, rb.linearVelocity.y);
        }
        

        // WasPressedThisFrame is ideal for jumping so it only triggers once per tap
        if (jump.WasPressedThisFrame() && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Jump
        }       

    }

    private void FixedUpdate()
    {
        // coyote time + jump checking here

    }

    // Show the detection area when selected
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
