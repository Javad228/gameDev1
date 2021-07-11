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
    Collider2D m_ObjectCollider;
    Collider2D m1_ObjectCollider;
    private GameObject ChildGameObject1;
    public Transform[] moveSpots;
    private int randomSpot;
    private float waitTime;
    public float startWaitTime;

    //public bool enemyDead = false;

    //The actual time the player will be able to fire.
    private float NextFire;

    
    // Start is called before the first frame update
    void Start()
    {
        randomSpot = Random.Range(0, moveSpots.Length);
        m_ObjectCollider = GetComponent<Collider2D>();
        ChildGameObject1 = transform.GetChild(0).gameObject;
        m1_ObjectCollider = ChildGameObject1.GetComponent<Collider2D>();
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
            //enemyDead = true;
            animator.SetBool("IsDead", true);
            gameObject.layer = LayerMask.NameToLayer("Water");
            ChildGameObject1.layer = LayerMask.NameToLayer("Water");
            tag = "Untagged";
            // m_ObjectCollider.isTrigger = true;
            // m1_ObjectCollider.isTrigger = true;
            Invoke("end", 0.5f);
            
            //GetComponent<Collider2D>().enabled = false;
            
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
            animator.SetBool("Walk", true);
            transform.position = Vector2.MoveTowards(transform.position,moveSpots[randomSpot].position, (moveSpeed-2)*Time.deltaTime);
            if (transform.position.x < moveSpots[randomSpot].position.x+ 0.2f)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else
            {
                transform.localScale = new Vector2(-1, 1);
            }
            if (Vector2.Distance(transform.position, moveSpots[randomSpot].position)<1f)
            {
                animator.SetBool("Walk", false);
                if (waitTime <= 0)
                {
                    randomSpot = Random.Range(0, moveSpots.Length);
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
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

    void end()
    {
        this.enabled = false;
    }

    
}
