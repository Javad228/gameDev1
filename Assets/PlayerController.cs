using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //public GameObject UI;
    public bool opening;
    private float speedNew;
    private bool moving;
    public bool isDead = false;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    public Transform attackPoint;
    public Transform usePoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public LayerMask useLayers;
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
    [SerializeField] private float hurtForce = 7f;
    [SerializeField] private Transform lifeParent;
    [SerializeField] private GameObject lifePrefab;
    public GameObject enemy;
    private Animator animator;
    //static PlayerUI instance;
 
    // void Awake() {
    //     //Singleton method
    //     if (instance == null) {
    //         //First run, set the instance
    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //
    //     } else if (instance != this) {
    //         //Instance is not the same as the one we have, destroy old one, and reset to newest one
    //         Destroy(instance.gameObject);
    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    // }

    
    // Start is called before the first frame update


   

    

    private void Start()
    {
        //Awake();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();


    }
    
    public void LoseLife()
    {
        if (PlayerUI.perm.livesRemaining == 0)
        {
            return;
        }
        PlayerUI.perm.livesRemaining--;
        PlayerUI.perm.lives[PlayerUI.perm.livesRemaining].enabled = false;
        if (PlayerUI.perm.livesRemaining == 0)
        {   
            anim.SetBool("Death", true);
            Debug.Log("dead");
            //this.enabled = false;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            PlayerUI.perm.coins++;
            PlayerUI.perm.coinsText.text = PlayerUI.perm.coins.ToString();

        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            LoseLife();

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
    private void OnCollisionStay2D(Collision2D other){
             
        if(other.gameObject.tag == "Moving_Platform"){
            transform.parent = other.transform;
            print("hey");
 
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Moving_Platform")
        {
            transform.parent = null;
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        
        
        
        if (state != State.hurt)
        {
            Movement();
        }
        if(Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D))
        {
            moving = true;
            
        } else{moving = false;}
        
        if (Input.GetKey(KeyCode.W) && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jumping;

        }
        
        AnimationState();
        anim.SetInteger("state", (int)state);
    }

    private void Update()
    {
        // if (enemy.GetComponent<Enemy_Script>().enemyDead == false)
        // {
        // 
        // }
        if (anim.GetBool("Death")==true)
        {
            isDead = true;
            this.enabled = false;
        }

        
        
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                anim.SetTrigger("hit");
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                foreach (Collider2D enemy in hitEnemies)
                {
                    print("Hit " + enemy.name);
                    if (enemy.tag == "Barrel")
                    {
                        enemy.GetComponent<TakeDamage>().GetHit(attackDamage);
                        
                    }
                    else
                    {
                        enemy.GetComponent<Enemy_Script>().TakeDamage(attackDamage);
                    }
                    
                    
                }

                nextAttackTime = Time.time + 1f / attackRate;
                
                
            }
        }

        if (GameObject.Find("DialogueManager") != null)
        {
            if (FindObjectOfType<DialogueManager>().animator.GetBool("isOpen"))
            {
                if (Mathf.Abs(this.transform.position.x - GameObject.Find("Sign").transform.position.x)>=4)
                {
                    FindObjectOfType<DialogueManager>().EndDialogue();
                }
            }
        }
        
        

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Collider2D[] useArea = Physics2D.OverlapCircleAll(usePoint.position, attackRange, useLayers);
            float compare = Mathf.Abs(this.transform.position.x - useArea[0].gameObject.transform.position.x);
            GameObject chest1 = useArea[0].gameObject;
            foreach (Collider2D chest in useArea)
            {
                if (Mathf.Abs(this.transform.position.x - chest.gameObject.transform.position.x) <= compare)
                {
                    compare = Mathf.Abs(this.transform.position.x - chest.gameObject.transform.position.x);
                    chest1 = chest.gameObject;
                }
                else
                {
                    continue;
                }
            }
            
                
            
            
            if (chest1.name == "ChestCommon")
            {    
                if (PlayerUI.perm.coins - 3 >= 0)
                {
                    PlayerUI.perm.coins = PlayerUI.perm.coins-3;
                    PlayerUI.perm.coinsText.text = PlayerUI.perm.coins.ToString();
                }
                
                
            }else if (chest1.name == "ChestRare")
            {
                if (PlayerUI.perm.coins - 6 >= 0)
                {
                    PlayerUI.perm.coins=PlayerUI.perm.coins-6;
                    PlayerUI.perm.coinsText.text = PlayerUI.perm.coins.ToString();
                }
                
            }else if (chest1.name == "ChestLegend")
            {
                if (PlayerUI.perm.coins - 13 >= 0)
                {
                    PlayerUI.perm.coins=PlayerUI.perm.coins-13;
                    PlayerUI.perm.coinsText.text = PlayerUI.perm.coins.ToString();
                }
                
            }
            else if (chest1.name != "Sign")
            {
                anim.SetTrigger("open");
                opening = true;
                this.enabled = false;
            }
            else
            {
                if (FindObjectOfType<DialogueManager>().conversationStarted == true)
                {
                    FindObjectOfType<DialogueManager>().DisplayNextSentence();
                }
                else
                {
                    chest1.GetComponent<DialogueTrigger>().TriggerDialogue();
                }
                
                
            }
        }

        
        
        
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

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
        
        if (usePoint == null)
        {
            return;
        }
        
        Gizmos.DrawWireSphere(usePoint.position,attackRange);
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
