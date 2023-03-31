using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Minion : MonoBehaviour // will work similar to card
{
    public CoreLoop turnHandler;

    public bool attackedThisTurn = false;

    public Board boardRef;
    public Hand playerHand;
    public int health;
    public int strength;
    public string minionName;
    
    [SerializeField] public Text healthText;
    [SerializeField] public Text strengthText;
    [SerializeField] public Text nameText;

    private void Start()
    {
        UpdateTextFields();
        ResetAttack();
    }

    public void ResetAttack()
    {
        attackedThisTurn = false;
    }

    // private Hand playerHand;
    // Start is called before the first frame update
    public void ButtonResponse()
    {
        if (playerHand.friendly)
        { 
            boardRef.AddAttacker(gameObject);
        }
        else
        {
            boardRef.AddTarget(gameObject);
        }
       
    }

    public void DeltaHealth(int delta)
    {
        if (health + delta <= 0)
        {
            Kill();
        }
        else
        {
            health += delta;
        }
        UpdateTextFields();
    }

    public void Attack(GameObject target)
    {
        if (attackedThisTurn)
        {
            return;
        }
        Debug.Log("attack done");
        Minion miniontargetScript;
        PlayerCharacter playerTargetScript;
        if (target.TryGetComponent(out miniontargetScript)) // this could be solved by having a more general "healthComponent"
        {
            miniontargetScript.DeltaHealth(-strength);
            DeltaHealth(-miniontargetScript.strength);
        }
        else if (target.TryGetComponent(out playerTargetScript))
        {
            playerTargetScript.DeltaHealth(-strength);
        }

        attackedThisTurn = true;
    }

    private void Kill() // this would handle killing any "shadow" in multiplayer as well
    {
        Debug.Log("starting kill");
        boardRef.RemoveMinon(gameObject);
    }
    private void UpdateTextFields()
    {   
        healthText.text = health.ToString();
        strengthText.text = strength.ToString();
        nameText.text = minionName; // unlikely to change
    }
}
