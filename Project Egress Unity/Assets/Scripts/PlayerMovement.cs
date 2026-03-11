using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputAction move;
    private InputAction jump; //N - J
    [SerializeField] private float movespeed = 5f; // Just Added serialize field  technically N - J
    [SerializeField] private float jumpForce = 10f; // Force applied when jumping N - J
    //private bool canJump; - G - J

        // --- NEW VARIABLES FOR GROUND DETECTION - G - J ---
    [SerializeField] private Transform groundCheck; // The "sensor" object at your feet
    [SerializeField] private float checkRadius = 0.2f; // Size of the detection circle
    [SerializeField] private LayerMask groundLayer; // Set this to "Ground" in the Inspector
    private bool canJump;
    // -------------------------------------------

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
        //G - J Added this line to update canJump by checking if feet touch the groundLayer
        canJump = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        Vector2 moveValue = move.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveValue.x*movespeed, rb.linearVelocity.y);

        // WasPressedThisFrame is ideal for jumping so it only triggers once per tap
        if (jump.WasPressedThisFrame() && canJump) //All stuff from here to 4 lines below is N - J
        {
            // Simple Jump: sets the vertical velocity directly
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }       

    }
    private void FixedUpdate()
    {
        // coyote time + jump checking here

    }
    //All below is some of the g - j. My bad. Brain hurt.
       private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
