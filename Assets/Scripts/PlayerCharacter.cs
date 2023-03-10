using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]private uint _deployPoints; // never negative
    public int health;
    [SerializeField] private Text _display;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
      /*  float toDisplay = _deployPoints;
        _display.text = toDisplay.ToString(CultureInfo.InvariantCulture);*/
    }
}
