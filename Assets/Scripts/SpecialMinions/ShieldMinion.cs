using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShieldMinion : Minion
{
    //public Text descriptionTag;
    // Start is called before the first frame update
    protected override void Start()
    {
        UpdateTextFields();
        ResetAttack();
    }
    
    public override void DeltaHealth(int delta)
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

    protected override void UpdateTextFields()
    {
        healthText.text = health.ToString();
        strengthText.text = strength.ToString();
        describeText.text = "Shielding";
        nameText.text = minionName; // unlikely to change
    }
}
