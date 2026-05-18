using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Movement inputs
    private InputAction move;
    private InputAction jump;

    // Movement stats
    [Header("Movement Stats")]
    [SerializeField] [Tooltip("The player's movement speed")] private float moveSpeed = 5f;
    [SerializeField] [Tooltip("The positive Y force applied to the player when they jump")] private float jumpForce = 5f;
    private Rigidbody2D rb; // Used to set movement
    private bool canJump;
    private float coyoteTime = 0f; // default value at 0 to prevent players from possibly jumping immediately upon level load
    [SerializeField] [Tooltip("How much time after leaving a platform the player has before jump is disabled")] [Min(0f)] private float coyoteLenience = 0.2f; // time before you're no longer Wile E Coyote
    private float jumpBuffer = 0f; // time 
    [SerializeField] private float jumpLenience = 0.07f; // time before buffer expires
    [SerializeField] [Tooltip("DO NOT CHANGE")] private PhysicsMaterial2D[] movementMaterials;
    [SerializeField] private float slipperyness = 0.5f;
    private bool speedBoost = false;

    // Ground detection
    [Header("Ground Checking")]
    [SerializeField] private Transform groundCheck; // The "sensor" object at your feet
    [SerializeField] private float checkRadius = 0.07f; // Size of the detection circle
    [SerializeField] private LayerMask groundLayer; // Object layer for sensor to detect
    private float noJumpCheck = 0f;

    [Header("Misc")]
    [SerializeField] [Tooltip("Enables coyote time")] private bool enableCoyote = true;
    [SerializeField] [Tooltip("Enables jump buffering")] private bool enableJumpBuffer = true;
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
            if(overlap.gameObject.TryGetComponent<Tile>(out Tile tile))
            {
                if (tile.tileName == "red") // speed tile
                {
                    moveSpeed = tile.SpeedBoost;
                    speedBoost = true;
                }
                else
                {
                    speedBoost = false;
                }
            }
        }
        else
        {
            canJump = false;
            speedBoost = false;
        }

        Vector2 moveValue = move.ReadValue<Vector2>();
        if(moveValue.x != 0)
        {
            rb.linearVelocity = new Vector2(moveValue.x*moveSpeed, rb.linearVelocity.y); // Move left-right
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * slipperyness, rb.linearVelocity.y);
        }
        
        if(rb.linearVelocity.x>=0.2)
        {
            animator.SetTrigger("right");
        }
        else if(rb.linearVelocity.x <=-0.2)
        {
            animator.SetTrigger("left");
        }
        else
        {
            animator.SetTrigger("Proceed");
        }

        // WasPressedThisFrame is ideal for jumping so it only triggers once per tap
        if ((jump.WasPressedThisFrame() || (jumpBuffer > 0f && enableJumpBuffer)) && (canJump || (coyoteTime > 0f && rb.linearVelocity.y <= 0 && enableCoyote)))
        {
            coyoteTime = 0f;
            jumpBuffer = 0f;
            GetComponent<CapsuleCollider2D>().sharedMaterial = movementMaterials[1];
            noJumpCheck = 0.2f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Jump
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
        if (!speedBoost)
        {
            if (canJump && moveSpeed > 5f)
            {
                moveSpeed -= 0.1f;
                if (moveSpeed < 5f)
                {
                    moveSpeed = 5f;
                }
            }
        }
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
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Tile>(out Tile tile))
        {
            if(tile.tileName == "orange")
            {
                float yVelocity = Mathf.Abs(collision.relativeVelocity.y) + tile.BounceForce; // TODO: fix 0 + tile.BounceForce
                if(yVelocity >= tile.MaxBounceForce)
                {
                    yVelocity = tile.MaxBounceForce;
                }
                GetComponent<CapsuleCollider2D>().sharedMaterial = movementMaterials[1];
                rb.linearVelocityY = yVelocity;
            }
            else if (tile.tileName == "green")
            {
                //rb.gravityScale = 0;
                //print(transform.InverseTransformPoint(collision.transform.position));
                //print(collision.GetContact(0).normal);
                Vector2 collisionDirection = collision.GetContact(0).normal;

                if(Mathf.Abs(collisionDirection.x) > Mathf.Abs(collisionDirection.y))
                {
                    if(collisionDirection.x > 0)
                    {
                        //collided right, rotate z to -90 deg
                    }
                    else
                    {
                        //collided left, rotate z to 90 deg
                    }
                }
                else
                {
                    if(collisionDirection.y > 0)
                    {
                        //collided up, reset z to 0 deg
                    }
                    else
                    {
                        //collided down, rotate z to 180 deg
                    }
                }
            }
        }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        //rb.gravityScale = 1;
    }
}
