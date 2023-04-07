using System.Net.Mime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Minion : MonoBehaviour // will work similar to card
{

    private bool HasAttacked = true; // makes so that minions cant attack instantly
    
    public Board boardRef;
    public Hand playerHand;
    public int health;
    public int strength;
    public string minionName;
    public string descriptionString;
    
    private bool _highlighted;
    
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
        ChangeColour(false);
    }

    // private Hand playerHand;
    // Start is called before the first frame update
    public virtual void ButtonResponse()
    {
        if (!HasAttacked)
        {
            playerHand.HandleMinionClick(this);
            ChangeColour(true);
        }
    }

    public virtual void ChangeColour(bool input)
    {
        _highlighted = input;
        HighlightToggle();
    }

    protected virtual void HighlightToggle()
    {
        if (_highlighted)
        {
            gameObject.GetComponent<Image>().color = Color.red;
        }
        else
        {
            gameObject.GetComponent<Image>().color = Color.white;
        }
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
        ChangeColour(false);
        UpdateTextFields();
    }

    public virtual void Attack(GameObject target) // TODO: move to board
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
        ChangeColour(false);
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
