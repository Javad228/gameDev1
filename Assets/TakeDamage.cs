using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{   
    
    public int maxHealth = 50;
    public Animator animator;
    public int currentHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void GetHit(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("hit");
        //hurt
        if (currentHealth <= 0)
        {   
            //enemyDead = true;
            animator.SetBool("broken", true);
            //Invoke("end",2);
            
            
            //gameObject.layer = LayerMask.NameToLayer("Water");
            //ChildGameObject1.layer = LayerMask.NameToLayer("Water");
            //tag = "Untagged";
            // m_ObjectCollider.isTrigger = true;
            // m1_ObjectCollider.isTrigger = true;
            //Invoke("end", 0.5f);
            
            //GetComponent<Collider2D>().enabled = false;
            
            //die
            //Destroy();
        }
    }

    private void end()
    {
        DestroyImmediate(this);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
