using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SpawnCoins : MonoBehaviour
{
    public GameObject Collectable;
    private int i;
    private int a;
    
    // Start is called before the first frame update
    void Start()
    {
        i = Random.Range(3, 5);
        a = i;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<TakeDamage>().currentHealth <= 0)
        {   
            print("broken spawn");
            
            while (i > 0)
            {
                GameObject coin = Instantiate(Collectable, transform.position, Quaternion.identity);
                if (a <100)
                {
                    coin.GetComponent<Rigidbody2D>().velocity = new Vector2(1, Random.Range(4,5));
                    a = 100;
                }
                else
                {
                    a = 0;
                    coin.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, Random.Range(4,5));
                }
                
                
                i--;
            }
            Destroy(gameObject,3);
            
            
        }
        
    }
}
