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

    public Rigidbody2D rb2d;
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
        if (distToPlayer < agroRange && distToPlayer > 2)
        {
            //chase player
            ChasePlayer();
        }
        else if (distToPlayer < 2)
        {
            //stop chasing
            StopChasing();
        }
    }

    void ChasePlayer()
    {
        if (transform.position.x < player.position.x)
        {
            rb2d.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            rb2d.velocity = new Vector2(-moveSpeed, 0);
            transform.localScale = new Vector2(1, 1);
            
        }
    }
    void StopChasing()
    {
        rb2d.velocity = new Vector2(0, 0);
    }
}
