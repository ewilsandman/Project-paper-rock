using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour // particularly special cards will have additional scripts and tags
{
    public CoreLoop turnHandler;
    
    public int cost;
    public bool minionSpawning;
    public Minion minionRef;
    public int minionCount; // currently unused
    public Hand handRef;
    public Board boardRef;

    [SerializeField] public Text healthText;
    [SerializeField] public Text StrenghtText;
    [SerializeField] public Text CostText;
    

     public int health; // these could change in hand
    public int strength; //

    private void OnMouseOver() // TODO:visualize that player can/cant deploy card
    { }

    private void OnMouseDown()
    {
        if (turnHandler.TurnActive)
        {
            if (!handRef.CheckCost(cost)) // reduce compounding 
                return;  
            
            if (minionSpawning)
            {
                if (boardRef.PlaceForMinion(handRef.Player1))
                {
                    Minion templateMinion = minionRef;
                    boardRef.AddMinion(templateMinion, health, strength, handRef.Player1);
                    handRef.UpdateFunds(-cost);
                    handRef.RemoveCard(this);
                }
            }
        }
        else
        {
            Debug.Log("Wait for your turn");
        }
    }

   public void UpdateTextFields()
    {
        healthText.text = health.ToString();
        StrenghtText.text = strength.ToString();
        CostText.text = cost.ToString();
    }
}
