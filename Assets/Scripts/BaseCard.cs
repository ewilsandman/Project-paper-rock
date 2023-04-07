using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour // particularly special cards will have additional scripts and tags
{

    public bool minionSpawning;
    public Minion minionRef; // for handling if minion has special conditions
    public Hand handRef;
    public string minionName;
    public string descriptionString;

    [SerializeField] private Text healthText;
    [SerializeField] private Text strengthText;
    [SerializeField] private Text costText;
    [SerializeField] private Text nameText;
    [SerializeField] private Text describeText;

    private bool _highlighted;
    private bool _glowLight;

    public int health; // these could change in hand
    public int strength; //
    public int cost;

    private void OnMouseOver() // TODO:visualize that player can/cant deploy card
    {
        if (handRef.friendly) // active turn check
        {
            if (handRef.playerFunds >= cost)
            {
                Debug.Log("Yep you can play this card" + gameObject.name);
                _glowLight = true;
            }
        }
    }

    private void OnMouseExit()
    {
        _glowLight = false;
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
            gameObject.GetComponent<Image>().color = Color.green;
        }
        else
        {
            if (gameObject != null)
            {
                gameObject.GetComponent<Image>().color = Color.white;
            }
        }
    }


    public void OnMouseDown()
    {
        handRef.OnCardClick(this);
    }

    public void UpdateTextFields() // changes text based on a change in data
    {
        healthText.text = health > 0 ? health.ToString() : ""; // these are to not display anything if it is not needed, for instance a direct attacking card will not have a health
        strengthText.text = strength > 0 ? strength.ToString() : "";
        costText.text = cost > 0 ? cost.ToString() : "";
        nameText.text = minionName;
        describeText.text = descriptionString;
    }
}
