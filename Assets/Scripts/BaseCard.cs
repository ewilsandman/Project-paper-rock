using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCard : MonoBehaviour
{
    public int cost;
    public bool minion;

    public int health;
    public int strength;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseOver()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
