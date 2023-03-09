using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseCard : MonoBehaviour
{
    public CoreLoop turnHandler;
    
    public int cost;
    public bool minionSpawning;
    public Minion minionRef;
    public int minionCount;
    public Hand handRef;
    public Board boardRef;

    public int health;
    public int strength;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseOver()
    {
        
    }

    public void NewPos(float x, float y)
    {
        Debug.Log(transform.position); 
        //transform.position = new Vector3(x, y, 0);
    }

    private void OnMouseDown()
    {
        if (turnHandler.TurnActive)
        {
            if (minionSpawning)
            {
                if (boardRef.PlaceforMinion())
                {
                    for (int i = 0; i < minionCount; i++)
                    {
                        Minion templateMinion = minionRef;
                        boardRef.AddMinion(templateMinion, true);
                    }

                    handRef.RemoveCard(this);
                    //Destroy(gameObject);
                }
            }
        }
        else
        {
            Debug.Log("Wait for your turn");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
