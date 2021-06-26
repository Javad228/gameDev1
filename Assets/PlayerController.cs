using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private enum State {idle, running, jumping, falling};
    private State state = State.idle;
    private Animator anim;
    private Collider2D coll;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

    }
    // Update is called once per frame
    private void Update()
    {
        
        
        float hdirection = Input.GetAxis("Horizontal");
        
        if (hdirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }else if (hdirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        

        if (Input.GetKey(KeyCode.W) && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jumping;

        }

        VelocityState();
        anim.SetInteger("state", (int)state);
    }

    private void VelocityState()
    {
        if (state == State.jumping)
        {   
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }
    
}
