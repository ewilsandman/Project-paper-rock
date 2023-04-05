using System.Net.Mime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Minion : MonoBehaviour // will work similar to card
{

    private bool HasAttacked = false;
    
    public Board boardRef;
    public Hand playerHand;
    public int health;
    public int strength;
    public string minionName;
    public string descriptionString;
    
    [SerializeField] public Text healthText;
    [SerializeField] public Text strengthText;
    [SerializeField] public Text nameText;
    [SerializeField] public Text describeText;

    protected virtual void Start()
    {
        UpdateTextFields();
        ResetAttack();
    }

    public virtual bool CheckAttack()
    {
        return HasAttacked;
    }
    

    public virtual void ResetAttack()
    {
        HasAttacked = false;
    }

    // private Hand playerHand;
    // Start is called before the first frame update
    public virtual void ButtonResponse()
    {
        playerHand.HandleMinionClick(this);
    }

    public virtual void DeltaHealth(int delta)
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

    public virtual void Attack(GameObject target)
    {
        if (HasAttacked)
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

        HasAttacked = true;
    }

    protected virtual void Kill() // this would handle killing any "shadow" in multiplayer as well
    {
        Debug.Log("starting kill");
        boardRef.RemoveMinon(gameObject);
    }
    protected virtual void UpdateTextFields()
    {   
        healthText.text = health.ToString();
        strengthText.text = strength.ToString();
        nameText.text = minionName; // unlikely to change
        describeText.text = descriptionString;
    }
}
