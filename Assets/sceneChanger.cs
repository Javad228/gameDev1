using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneChanger : MonoBehaviour
{

    [SerializeField] private string sceneName;
    
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.gameObject.tag == "Untagged")
        {
            SceneManager.LoadScene(sceneName);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
