using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

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
    private Vector2 moveValue;
    private float jumpValue;
    [SerializeField] private float gravityWeight = -9.81f; //how heavy gravity is
    private Vector2 gravity;
    public Vector2 Gravity => gravity; //make the player's gravity publically accessible
    private float movementDisabled = 0f;

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
    readonly Collider2D[] results = new Collider2D[10];
    readonly List<ContactPoint2D> contactPoints = new List<ContactPoint2D>();
    private bool stopOrangeBounce = true;
    private float stopOrangeBounceTime = 0f;
    private Vector2 currentRelVelocity = new Vector2(0,0);

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
            if(overlap.gameObject.TryGetComponent<Tile>(out Tile tile))
            {
                //
            }
        }
        else
        {
            canJump = false;
            speedBoost = false;
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
        if ((jump.WasPressedThisFrame() || (jumpBuffer > 0f && enableJumpBuffer)) && (canJump || (coyoteTime > 0f && Vector2.Dot(rb.linearVelocity, transform.up) <= 0 && enableCoyote)))
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
        movementDisabled -= Time.deltaTime;
        stopOrangeBounceTime -= Time.deltaTime;

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
            if(movementDisabled <= 0)
            {
                horizontalVelocity = transform.right * moveSpeed * moveValue.x; //set left-right velocity if movement key is pressed
            }
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
            FindAnyObjectByType<GameManager>().PlayerDied();
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
    public void OnCollisionStay2D(Collision2D collision)
    {
        /*GameObject hitTile = null;
        if(collision.gameObject.TryGetComponent<TileMapCollision>(out TileMapCollision tilemap))
        {
            hitTile = tilemap.GetCollisionLocation(collision);
            
        }*/

        Array.Clear(results, 0, results.Length);
        stopOrangeBounce = false;
        contactPoints.Clear();

        if(!collision.gameObject.TryGetComponent<Tilemap>(out _) || !collision.gameObject.TryGetComponent<CompositeCollider2D>(out _)) return; //make sure we are getting a Tilemap or CompositeCollider2D, or we exit

        Tilemap tilemap = collision.collider.GetComponent<Tilemap>();
        if(tilemap != null)
        {

            collision.GetContacts(contactPoints);
            foreach(ContactPoint2D contact in contactPoints)
            {
                Vector3 contactPoint = contact.point - contact.normal * 0.03f;
                int pointHits = Physics2D.OverlapCircle(contactPoint, 0.05f, ContactFilter2D.noFilter, results);
                //print(pointHits);
                foreach(var collider in results)
                {
                    if(collider == null) continue;
                    //print(collider.gameObject.name);
                    Tile colTile = collider.GetComponent<Tile>();
                    if(colTile != null)
                    {
                        if(colTile.tileName == "red")
                        {
                            moveSpeed = colTile.SpeedBoost;
                            speedBoost = true;
                        }
                        else speedBoost = false;

                        if(colTile.tileName == "orange")
                        {
                            if(colTile.gameObject.transform.parent == null) continue;
                            if(!stopOrangeBounce && stopOrangeBounceTime < 0f)
                            {
                                OrangeTile(collision, colTile, rb.linearVelocity);
                                stopOrangeBounce = true;
                                stopOrangeBounceTime = 0.3f;
                            }
                        }

                        if(colTile.tileName == "green")
                        {
                            Vector2 collisionDirection = contact.normal;

                            Quaternion newRotation;
                            if(Mathf.Abs(collisionDirection.x) > Mathf.Abs(collisionDirection.y))
                            {
                                if(collisionDirection.x > 0) newRotation = Quaternion.Euler(0, 0, -90f); //collided right, rotate z to -90 deg
                                else newRotation = Quaternion.Euler(0, 0, 90f); //collided left, rotate z to 90 deg
                            }
                            else
                            {
                                if(collisionDirection.y > 0) newRotation = Quaternion.Euler(0, 0, 0); //collided up, reset z to 0 deg
                                else newRotation = Quaternion.Euler(0, 0, 180f); //collided down, rotate z to 180 deg
                            }
                            if(newRotation != transform.rotation)
                            {
                                transform.rotation = newRotation;
                                movementDisabled = colTile.MovementDisableTime;
                                DisableJumps(colTile.MovementDisableTime, true);
                            }
                        }
                    }
                    
                }
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        currentRelVelocity = collision.relativeVelocity;
        //if(collision.gameObject.GetComponent<Tilemap>() != null || collision.gameObject.GetComponent<CompositeCollider2D>() != null) return;
        if(collision.gameObject.TryGetComponent<Tilemap>(out _) || collision.gameObject.TryGetComponent<CompositeCollider2D>(out _))
        {
            print("ran");
            collision.GetContacts(contactPoints);
            foreach(ContactPoint2D contact in contactPoints)
            {
                Vector3 contactPoint = contact.point - contact.normal * 0.03f;
                int pointHits = Physics2D.OverlapCircle(contactPoint, 0.05f, ContactFilter2D.noFilter, results);
                //print(pointHits);
                foreach(var collider in results)
                {
                    if(collider == null) continue;
                    //print(collider.gameObject.name);
                    Tile colTile = collider.GetComponent<Tile>();
                    if(colTile != null)
                    {
                        if(colTile.tileName == "orange")
                        {
                            if(colTile.gameObject.transform.parent == null) continue;
                            if(!stopOrangeBounce && stopOrangeBounceTime < 0f)
                            {
                                OrangeTile(collision, colTile, collision.relativeVelocity);
                                stopOrangeBounce = true;
                                stopOrangeBounceTime = 0.3f;
                            }
                        }
                    }
                }
            }
            return;
        }
        if(collision.gameObject.TryGetComponent<Tile>(out Tile tile))
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
            if(tile.tileName == "orange")
            {
                /*
                float yVelocity = Mathf.Abs(collision.relativeVelocity.y) + tile.BounceForce;
                if(yVelocity >= tile.MaxBounceForce)
                {
                    yVelocity = tile.MaxBounceForce;
                }
                GetComponent<CapsuleCollider2D>().sharedMaterial = movementMaterials[1];
                rb.linearVelocityY = yVelocity;
                */
                /*
                float localUpVelocity = Vector2.Dot(collision.relativeVelocity, transform.up);
                //float localRightVelocity = Vector2.Dot(collision.relativeVelocity, transform.right);
                Vector2 newVelocity = transform.up * localUpVelocity + transform.up * tile.BounceForce;
                if(Vector2.Dot(newVelocity, transform.up) > Vector2.Dot(transform.up * tile.MaxBounceForce, transform.up))
                {
                    newVelocity = transform.up * tile.MaxBounceForce;
                }
                rb.AddForce(newVelocity, ForceMode2D.Impulse);
                */
                OrangeTile(collision, tile, collision.relativeVelocity);
            }
            else if (tile.tileName == "green")
            {
                //rb.gravityScale = 0;
                //print(transform.InverseTransformPoint(collision.transform.position));
                //print(collision.GetContact(0).normal);
                Vector2 collisionDirection = collision.GetContact(0).normal;

                Quaternion newRotation;

                if(Mathf.Abs(collisionDirection.x) > Mathf.Abs(collisionDirection.y))
                {
                    if(collisionDirection.x > 0)
                    {
                        //collided right, rotate z to -90 deg
                        newRotation = Quaternion.Euler(0, 0, -90f);
                    }
                    else
                    {
                        //collided left, rotate z to 90 deg
                        newRotation = Quaternion.Euler(0, 0, 90f);
                    }
                }
                else
                {
                    if(collisionDirection.y > 0)
                    {
                        //collided up, reset z to 0 deg
                        newRotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        //collided down, rotate z to 180 deg
                        newRotation = Quaternion.Euler(0, 0, 180f);
                    }
                }
                if(newRotation != transform.rotation)
                {
                    transform.rotation = newRotation;
                    movementDisabled = tile.MovementDisableTime;
                    DisableJumps(tile.MovementDisableTime, true);
                }
            }
        }
    }

    private void OrangeTile(Collision2D collision, Tile colTile, Vector2 playerVel)
    {
        //float localUpVelocity = 1;
        //float localUpVelocity = Mathf.Abs(Vector2.Dot(currentRelVelocity, transform.up));
        float localUpVelocity = Mathf.Abs(Vector2.Dot(playerVel, transform.up));
        //if(Mathf.FloorToInt(Vector2.Dot(currentRelVelocity, transform.up)) == 0)
        print(playerVel);
        Vector2 newVelocity = (Vector2)transform.up * localUpVelocity + (Vector2)transform.up * colTile.BounceForce;
        if(Vector2.Dot(newVelocity, transform.up) > Vector2.Dot(transform.up * colTile.MaxBounceForce, transform.up))
        {
            newVelocity = transform.up * colTile.MaxBounceForce;
        }
        GetComponent<CapsuleCollider2D>().sharedMaterial = movementMaterials[1];
        rb.linearVelocity = newVelocity;
        //print(localUpVelocity);
        //print(newVelocity);
        
        /*
                        float yVelocity = Mathf.Abs(collision.relativeVelocity.y) + colTile.BounceForce;
                if(yVelocity >= colTile.MaxBounceForce)
                {
                    yVelocity = colTile.MaxBounceForce;
                }
                GetComponent<CapsuleCollider2D>().sharedMaterial = movementMaterials[1];
                rb.linearVelocityY = yVelocity;
                print(yVelocity);
        */
    }
}
