using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CoreLoop : MonoBehaviour
{ // this is all local to the current player
    // public float playerNumber;
    public float timeLimit = 120; // seconds
    public float _turnTime;
    public float _turnsElapsed; // turns since start
    public bool TurnActive = false;

    [SerializeField] private PlayerCharacter playerCharacter;
    
    // Start is called before the first frame update
    void Start()
    {
        StartTurn(); // temp workaround
    }

    void StartTurn()
    {
        _turnTime = 0;
        TurnActive = true;
    }

    void EndTurn() // this would in other player call start turn
    {
        _turnsElapsed++;
        TurnActive = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TurnActive)
        {
            if (_turnTime > timeLimit)
            {
                Debug.Log("Times up");
                EndTurn();
            }
            else // gives you one extra frame but whatever
            {
                _turnTime += Time.deltaTime;
            }
        }
    }
}
