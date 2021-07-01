using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float speedNew;
    private bool moving;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;
    private Rigidbody2D rb;
    private int takingSwordOut;
    private enum State {idle, running, jumping, falling, hurt};
    private State state = State.idle;
    private Animator anim;
    private Collider2D coll;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] public int coins = 0;
    [SerializeField] private Text coinsText;
    [SerializeField] private float hurtForce = 7f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            coins++;
            coinsText.text = coins.ToString();

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            state = State.hurt;
            if (other.gameObject.transform.position.x > transform.position.x)
            {
                //enemy is on the right
                rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
            }
            else
            {
                //enemy is on the left
                rb.velocity = new Vector2(hurtForce, rb.velocity.y);
            }
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D))
        {
            moving = true;
            
        } else{moving = false;}

        if (state != State.hurt)
        {
            Movement();
        }
        
        
        AnimationState();
        anim.SetInteger("state", (int)state);
    }

    private void Movement()
    {
        float hdirection = Input.GetAxis("Horizontal");
        if (moving == true)
        {
            if (hdirection < 0)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                transform.localScale = new Vector2(-1, 1);
            }
            else if (hdirection > 0)
            {
            
                rb.velocity = new Vector2(speed, rb.velocity.y);
                transform.localScale = new Vector2(1, 1);
            }
        }
        else
        {
            if (rb.velocity.y > 0.2f)
            {
                speedNew = speed;
            } 
            else
            {
                speedNew = speed / 2.5f;
            }
            
            if (hdirection < 0)
            {
                rb.velocity = new Vector2(-speedNew, rb.velocity.y);
                transform.localScale = new Vector2(-1, 1);
            }
            else if (hdirection > 0)
            {
            
                rb.velocity = new Vector2(speedNew, rb.velocity.y);
                transform.localScale = new Vector2(1, 1);
            }
        }
        
        

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                anim.SetTrigger("hit");
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                foreach (Collider2D enemy in hitEnemies)
                {
                    Debug.Log("Hit " + enemy.name);
                    enemy.GetComponent<Enemy_Script>().TakeDamage(attackDamage);
                }

                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if (Input.GetKey(KeyCode.W) && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jumping;

        }
        
        
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }

    private void AnimationState()
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
        }else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            
            
            state = State.running;
            
        }
        else
        {
            
            state = State.idle;
            
            
        }
    }
}
