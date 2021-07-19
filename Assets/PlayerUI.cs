using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{   
    public int coins = 0;
    public Text coinsText;
    public Image[] lives;
    public int livesRemaining;

    public static PlayerUI perm;
    void Start()
    {   
        DontDestroyOnLoad(gameObject);
        livesRemaining = lives.Length;
        if (!perm)
        {
            perm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
