using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerTurn : MonoBehaviour // naming
{
    [SerializeField] private CoreLoop coreLoop;

    public Text counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float toDisplay = coreLoop.timeLimit - coreLoop._turnTime;
        string toDisplayString = toDisplay.ToString(CultureInfo.InvariantCulture).Substring(0, 3); // "specify string culture"??
        counter.text = toDisplayString;
    }
}
