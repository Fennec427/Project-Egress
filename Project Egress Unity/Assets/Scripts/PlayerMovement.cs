using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Movement inputs
    private InputAction move;
    private InputAction jump;

    // Movement stats
    [Header("Movement Stats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody2D rb; // Used to set movement
    private bool canJump;
    private float coyoteTime = 0f; // default value at 0 to prevent players from possibly jumping immediately upon level load
    [SerializeField] private float coyoteLenience = 0.2f; // time before you're no longer Wile E Coyote
    private float jumpBuffer = 0f; // time 
    [SerializeField] private float jumpLenience = 0.07f; // time before buffer expires
    [SerializeField] private PhysicsMaterial2D[] movementMaterials;
    [SerializeField] private float slipperyness = 0.5f;
    private Vector2 moveValue;
    private float jumpValue;
    [SerializeField] private float gravityWeight = -9.81f; //how heavy gravity is
    private Vector2 gravity;
    public Vector2 Gravity => gravity; //make the player's gravity publically accessible

    // Ground detection
    [Header("Ground Checking")]
    [SerializeField] private Transform groundCheck; // The "sensor" object at your feet
    [SerializeField] private float checkRadius = 0.07f; // Size of the detection circle
    [SerializeField] private LayerMask groundLayer; // Object layer for sensor to detect
    private float noJumpCheck = 0f;

    [Header("Misc")]
    [SerializeField] private bool enableCoyote = true;
    [SerializeField] private bool enableJumpBuffer = true;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");

        move.Enable();
        jump.Enable();
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }

        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //canJump = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer); // Update canJump by checking if sensor is touching the groundLayer
        Collider2D overlap = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (overlap != null)
        {
            if (overlap.isTrigger)
            {
                canJump = false;
                Collider2D[] allColls = overlap.gameObject.GetComponents<Collider2D>();
                foreach (var item in allColls)
                {
                    if (item.isTrigger)
                    {
                        continue;
                    }
                    else
                    {
                        canJump = true;
                        break;
                    }
                }
            }
            else
            {
                canJump = true;
            }
        }
        else
        {
            canJump = false;
        }

        moveValue = move.ReadValue<Vector2>();
        
        if(Vector2.Dot(rb.linearVelocity, transform.right) >= 0.2)
        {
            animator.SetTrigger("right");
        }
        else if(Vector2.Dot(rb.linearVelocity, transform.right) <= -0.2)
        {
            animator.SetTrigger("left");
        }
        else
        {
            animator.SetTrigger("Proceed");
        }

        jumpValue = 0;
        // WasPressedThisFrame is ideal for jumping so it only triggers once per tap
        if ((jump.WasPressedThisFrame() || (jumpBuffer > 0f && enableJumpBuffer)) && (canJump || (coyoteTime > 0f && rb.linearVelocity.y <= 0 && enableCoyote)))
        {
            coyoteTime = 0f;
            jumpBuffer = 0f;
            GetComponent<CapsuleCollider2D>().sharedMaterial = movementMaterials[1];
            noJumpCheck = 0.2f;
            jumpValue = 1;
            //rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Jump
            //rb.linearVelocity = transform.up * jumpForce;
        }
        else if (jump.WasPressedThisFrame()) // input buffer
        {
            jumpBuffer = jumpLenience;
        }
    }

    private void FixedUpdate()
    {
        if (canJump && noJumpCheck <= 0)
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
        noJumpCheck -= Time.deltaTime;

        //rb.linearVelocity = new Vector2(moveValue.x*moveSpeed, rb.linearVelocity.y); // Move left-right
        gravity = transform.up * gravityWeight;
        rb.AddForce(gravity, ForceMode2D.Force); //apply gravity
        
        Vector2 verticalVelocity = (Vector2)transform.up * Vector2.Dot(rb.linearVelocity, transform.up); //keep vertical velocity
        if(jumpValue == 1)
        {
            verticalVelocity = transform.up * jumpForce; //set vertical velocity when jump key pressed
        }

        Vector2 horizontalVelocity = (Vector2)transform.right * Vector2.Dot(rb.linearVelocity, transform.right) * slipperyness; //keep left-right velocity
        if(moveValue.x != 0)
        {
            horizontalVelocity = transform.right * moveSpeed * moveValue.x; //set left-right velocity if movement key is pressed
        }
        
        rb.linearVelocity = horizontalVelocity + verticalVelocity; //add horizontal and vertical velocity variables to get the overall velocity
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
    
    public void DisableJumps(float time = 0.2f, bool resetCoyoteJumpBuffer = false)
    {
        noJumpCheck = time;
        if (resetCoyoteJumpBuffer)
        {
            coyoteTime = -1;
            jumpBuffer = -1;
        }
    }
}
