using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour // particularly special cards will have additional scripts and tags
{
    public CoreLoop turnHandler;
    
    [FormerlySerializedAs("_cost")] public int cost;
    public bool minionSpawning;
    public Minion minionRef;
    public Hand handRef;
    public Board boardRef;
    public string minionName;

    [SerializeField] private Text healthText;
    [SerializeField] private Text strenghtText;
    [SerializeField] private Text costText;
    [SerializeField] private Text nameText;


    public int health; // these could change in hand
    public int strength; //

    private void OnMouseOver() // TODO:visualize that player can/cant deploy card
    {
       /* if ()
        {
            
        }*/
    }

    public bool CheckCost(int available)
    {
        if (available >= cost)
        {
            return true;
        }

        return false;
    }

   /* public void AiAction()
    {
        if (minionSpawning)
        {
            if (boardRef.PlaceForMinion(handRef.friendly))
            {
                Minion templateMinion = minionRef;
                boardRef.AddMinion(templateMinion, health, strength, minionName, handRef.friendly);
                handRef.UpdateFunds(-cost);
                handRef.RemoveCard(this);
            }
        }
    }*/

    public void OnMouseDown()
    {
        if (turnHandler.turnActive)
        {
            if (!CheckCost(cost)) // reduce compounding 
                return;  
            
            if (minionSpawning)
            {
                if (boardRef.PlaceForMinion(handRef.friendly))
                {
                    Minion templateMinion = minionRef;
                    boardRef.AddMinion(templateMinion, health, strength, minionName, handRef.friendly);
                    handRef.UpdateFunds(-cost);
                    handRef.RemoveCard(this);
                }
            }
        }
    }

   public void UpdateTextFields()
    {
        healthText.text = health.ToString();
        strenghtText.text = strength.ToString();
        costText.text = cost.ToString();
        nameText.text = minionName;
    }
}
