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

    private SpriteRenderer sr;
    private BoxCollider2D col;


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
        col = GetComponent<BoxCollider2D>();
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
    }

    void FixedUpdate()
    {
        idleTime -= Time.deltaTime;
        warnTime -= Time.deltaTime;
        activeTime -= Time.deltaTime;
        deactivateTime -= Time.deltaTime;
    }

    public void setIdle()
    {
        animator.SetTrigger("idle");
        idleTime = idleWait;
        animator.SetInteger("currentState", 0);
        gameObject.tag = "Untagged";
    }
}
