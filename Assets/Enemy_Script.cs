using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    [SerializeField] Transform player;
    public int maxHealth = 100;
    public Animator animator;
    public int currentHealth;
    [SerializeField] float agroRange;
    [SerializeField] float moveSpeed;
    private Rigidbody2D rb2d;
    private float FireRate = 2f;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    //The actual time the player will be able to fire.
    private float NextFire;

    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {   
        
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        //hurt
        if (currentHealth <= 0)
        {   
            animator.SetBool("IsDead", true);
            
            //GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
            //die
            //Destroy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        print("distance: "+ distToPlayer);
        if (distToPlayer < agroRange && distToPlayer > 3)
        {
            //chase player
            
            //print("walk = true");
            
            if (!animator.GetBool("Attack")&&!animator.GetCurrentAnimatorStateInfo(0).IsName("hit")&&player.GetComponent<PlayerController>().isDead == false)
            {
                animator.SetBool("Walk", true);
                ChasePlayer();
            }
            else
            {
                animator.SetBool("Attack", false);
            }
            
        }
        else if (distToPlayer < 3)
        {
            //stop chasing

            if (Time.time >= nextAttackTime)
            {
                animator.SetBool("Attack", true);
                nextAttackTime = Time.time + 1f / attackRate;
            }

            StopChasing();
        }
        else
        {   
            animator.SetBool("Attack", false);
            animator.SetBool("Walk", false);
            //print("walk = false");
        }
    }

    void ChasePlayer()
    {
        if (transform.position.x < player.position.x)
        {   
            
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed*Time.deltaTime);
            print("moving with " + moveSpeed);
            
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            print("moving with " + -moveSpeed);
            rb2d.velocity =  new Vector2(-moveSpeed, rb2d.velocity.y);
            //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed*Time.deltaTime);
            transform.localScale = new Vector2(-1, 1);
            
        }
    }
    void StopChasing()
    {   
        animator.SetBool("Walk", false);
        //print("walk = false");
        rb2d.velocity = new Vector2(0, 0);
    }
    

    
}
