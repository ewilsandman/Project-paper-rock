using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CoreLoop : MonoBehaviour
{ // this is all local to the current player
    // public float playerNumber;
    public float timeLimit = 120; // seconds
    public float turnTime;
    public float turnsElapsed; // turns since start
    public bool TurnActive = false;
    public Hand handRef;
    public CoreLoop OtherPlayer;

    [SerializeField] private Text turnTimer;
    [SerializeField] private PlayerCharacter playerCharacter;
    
    // Start is called before the first frame update
    void Start()
    {
        //StartTurn(); // temp workaround
    }

    void StartTurn()
    {
        turnTime = 0;
        TurnActive = true;
        if (turnsElapsed >0)
        { 
            handRef.DrawCards();
        }
        handRef.ResetFunds();
    }

    public void EndTurn()
    {
        turnsElapsed++;
        TurnActive = false;
        OtherPlayer.StartTurn();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TurnActive)
        {
            if (turnTime > timeLimit)
            {
                Debug.Log("Times up");
                EndTurn();
            }
            else // gives you one extra frame but whatever
            {
                turnTime += Time.deltaTime;
            }

            float timeToDisplay = timeLimit - turnTime;
            turnTimer.text = "Time left:" + timeToDisplay.ToString().Substring(0, 2);
        }
    }
}
