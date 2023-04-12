using System.Net.Mime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Minion : MonoBehaviour // will work similar to card
{

    private bool HasAttacked = true; // makes so that minions cant attack instantly
    
    public Board boardRef;
    public Hand playerHand;
    public int strength;
    public string minionName;
    public string descriptionString;
    
    private bool _highlighted;
    
    [SerializeField] public Text healthText;
    [SerializeField] public Text strengthText;
    [SerializeField] public Text nameText;
    [SerializeField] public Text describeText;
    [SerializeField] public Image image;
    [SerializeField] public Targetable healthPool;

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
            image.color = Color.red;
        }
        else
        {
            image.color = Color.white;
        }
    }
    

    public virtual void Attack(Targetable target)
    {
        if (HasAttacked)
        {
            return;
        }
        Debug.Log("attack done");
               
        if (target) // this could be solved by having a more general "healthComponent"
        {
            target.DeltaHealth(-strength);
            if (target.TryGetComponent( out Minion minion))
            {
                healthPool.DeltaHealth(-minion.strength);
            }
        }
        HasAttacked = true;
    }

    public virtual void Kill() // this would handle killing any "shadow" in multiplayer as well
    {
        Debug.Log("starting kill");
        boardRef.RemoveMinon(gameObject);
    }
    public virtual void UpdateTextFields()
    {
        healthText.text = healthPool.ReturnHealth().ToString();
        strengthText.text = strength.ToString();
        nameText.text = minionName; // unlikely to change
        describeText.text = descriptionString;
        ChangeColour(false);
    }
}
