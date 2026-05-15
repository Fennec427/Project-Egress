using Unity.VisualScripting;
using UnityEngine;

public class FireSpitter : MonoBehaviour
{
    private float idleTime;
    [SerializeField] private float idleWait = 5f;
    private float warnTime;
    [SerializeField] private float warnWait = 2f;
    private float activeTime;
    [SerializeField] private float activeWait = 3f;
    private float deactivateTime;
    [SerializeField] private float deactivateWait = 1f;
    public Animator animator;
    [SerializeField] private bool checkedForPerviousIntersect = false;
    [SerializeField] private LayerMask playerLayer;

    private SpriteRenderer sr;
    private PolygonCollider2D col;
    private Sprite oldSprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        idleTime = idleWait;
        warnTime = 0f;
        activeTime = 0f;
        deactivateTime = 0f;
    }
    
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(idleTime <= 0f && animator.GetInteger("currentState") == 0)
        {
            animator.SetTrigger("warn");
            warnTime = warnWait;
            animator.SetInteger("currentState", 1);
        }
        else if(warnTime <= 0f && animator.GetInteger("currentState") == 1)
        {
            animator.SetTrigger("active");
            activeTime = activeWait;
            animator.SetInteger("currentState", 2);
            //start kill
            gameObject.tag = "Death";
            checkedForPerviousIntersect = false;
        }
        else if(activeTime <= 0f && animator.GetInteger("currentState") == 2)
        {
            animator.SetTrigger("deactivate");
            deactivateTime = deactivateWait;
            animator.SetInteger("currentState", 3);
            //end kill
        }
        else if(deactivateTime <= 0f && animator.GetInteger("currentState") == 3)
        {
            animator.SetTrigger("idle");
            idleTime = idleWait;
            animator.SetInteger("currentState", 0);
        }
        if(sr.sprite != oldSprite)
        {
            col.CreateFromSprite(sr.sprite);
            oldSprite = sr.sprite;
        }
        if(!checkedForPerviousIntersect){
            if(gameObject.tag == "Death")
            {
                //Physics2D.OverlapCapsule((Vector2)gameObject.transform.position, new Vector2(0.1f, 0.2f), 0, 0, playerLayer)
                //Physics2D.OverlapBox((Vector2)gameObject.transform.position, new Vector2(0.1f, 0.2f), 0, playerLayer)
                if(Physics2D.OverlapBox((Vector2)gameObject.transform.position, new Vector2(0.1f, 0.2f), 0, playerLayer))
                {
                    Object.FindAnyObjectByType<GameManager>().PlayerDied();
                }
                checkedForPerviousIntersect = true;
            }
        }
    }

    void FixedUpdate()
    {
        idleTime -= Time.deltaTime;
        warnTime -= Time.deltaTime;
        activeTime -= Time.deltaTime;
        deactivateTime -= Time.deltaTime;
    }

    public void StopKill()
    {
        gameObject.tag = "Untagged";
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(gameObject.transform.position, new Vector3(0.1f, 0.2f, 0f));
    }
}
