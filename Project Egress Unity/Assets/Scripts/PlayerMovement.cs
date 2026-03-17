using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Movement inputs
    private InputAction move;
    private InputAction jump;

    // Movement stats
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private bool canJump;
    private float coyoteTime = 0f; // default value at 0 to prevent players from possibly jumping immediately upon level load
    [SerializeField] private float coyoteLenience = 0.2f; // time before you're no longer Wile E Coyote
    private float jumpBuffer = 0f; // time 
    [SerializeField] private float jumpLenience = 0.07f; // time before buffer expires
    [SerializeField] private PhysicsMaterial2D[] movementMaterials;

    // Ground detection
    [SerializeField] private Transform groundCheck; // The "sensor" object at your feet
    [SerializeField] private float checkRadius = 0.07f; // Size of the detection circle
    [SerializeField] private LayerMask groundLayer; // Object layer for sensor to detect

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
            rb.linearVelocity = new Vector2(moveValue.x*moveSpeed, rb.linearVelocity.y); // Move left-right
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.8f, rb.linearVelocity.y);
        }
        


        // WasPressedThisFrame is ideal for jumping so it only triggers once per tap
        if ((jump.WasPressedThisFrame() || jumpBuffer > 0f) && (canJump || (coyoteTime > 0f && rb.linearVelocity.y <= 0)))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Jump
        }
        else if (jump.WasPressedThisFrame()) // input buffer
        {
            jumpBuffer = jumpLenience;
        }

    }

    private void FixedUpdate()
    {
        if (canJump)
        {
            coyoteTime = coyoteLenience; // reset as long as the player is on the ground
            GetComponent<CapsuleCollider2D>().sharedMaterial = movementMaterials[0];
        }
        else
        {
            coyoteTime -= Time.deltaTime; // reduce while in the air
            GetComponent<CapsuleCollider2D>().sharedMaterial = movementMaterials[1];
        }
        jumpBuffer -= Time.deltaTime; // constantly reduce
    }

    // Show the detection area when selected in editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Object.FindAnyObjectByType<GameManager>().PlayerDied();
        }
    }
}
