using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneChanger : MonoBehaviour
{

    [SerializeField] private string sceneName;
    public GameObject player;
    private Animator animator;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void sceneChange()
    {
        SceneManager.LoadScene(sceneName);
    }

    // private void goDown()
    // {
    //     
    // }
    
    // Update is called once per frame
    void Update()
    {
        //print(player.GetComponent<>().IsPlaying("doorOpen"));
        if (player.GetComponent<PlayerController>().opening == true)
        {
            animator.SetTrigger("Open");
            Vector3 temp = new Vector3(this.transform.position.x,1,0);
            player.transform.position = temp;
            //player.transform.position = this.transform.position;
            Invoke("sceneChange",0.6f);
            
        }
    }
}
