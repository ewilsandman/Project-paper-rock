using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]private uint _deployPoints; // never negative
    public int health;
    public bool friendly;
    [SerializeField] private Text deployPointDisplay;
    [SerializeField] private Text healthPointDisplay;
    [SerializeField] private Text pileCardsDisplay; // unused
    [SerializeField] private Board _board;
    [SerializeField] private Hand hand;
    [SerializeField] private CoreLoop loop;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTextFields();
    }

    public void ButtonResponse()
    {
        if (!friendly) // friendly status needs to change with the "active turn system"
        {
            _board.AddTarget(gameObject);
        }
    }

    public void DeltaHealth(int delta)
    {
        if (health + delta < 0)
        {
            Debug.Log("ono I am kill" + transform.name);
            // die and mark other player victory
        }
        else
        {
            health += delta;
        }
        UpdateTextFields();
    }

    private void FixedUpdate()
    {
      /*  float toDisplay = _deployPoints;
        _display.text = toDisplay.ToString(CultureInfo.InvariantCulture);*/
    }

    public void UpdateTextFields()
    {
        deployPointDisplay.text = "Funds: " + _deployPoints;
        healthPointDisplay.text = "Health: " + health;
    }
}
