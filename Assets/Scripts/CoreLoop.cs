using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CoreLoop : MonoBehaviour
{
    // this is all local to the current player
    // public float playerNumber;
    [SerializeField] private float TimeToChange;
    public float timeLimit = 120; // seconds
    public float turnTime; // time turn has elapsed, counts up

    public float turnsElapsed; // turns since start
    public bool TurnActive = false;
    private bool WaitingToSwap;
    [SerializeField] private Hand handRef;
    [SerializeField] private CoreLoop OtherPlayer;

    [SerializeField] private Board boardRef;
    [SerializeField] private Text turnTimer;
    [SerializeField] private PlayerCharacter playerCharacter;
    
    // Start is called before the first frame update
    void Start()
    {
        //StartTurn(); // temp workaround
    }

    public void StartTurn()
    {
        Debug.Log("now player" + gameObject.name);
        turnTime = 0;
        TurnActive = true;
        if (turnsElapsed >0)
        { 
            handRef.DrawCards();
        }
        handRef.ResetFunds();
    }

    private void EndTurn()
    {
        turnsElapsed++;
        TurnActive = false;
        StartSwap();
         //OtherPlayer.StartTurn();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TurnActive)
        {
            if (turnTime > timeLimit)
            {
                turnTime = 0;
                Debug.Log("Times up");
                EndTurn();
            }
            else // gives you one extra frame but whatever
            {
                turnTime += Time.deltaTime;
            }

            float timeToDisplay = timeLimit - turnTime;
            if (timeToDisplay.ToString().Length > 1)
            {
                turnTimer.text = "Time left:" + timeToDisplay.ToString().Substring(0, 2);
            }
            turnTimer.text = "Time left:" + timeToDisplay.ToString();
        }
        else if (WaitingToSwap)
        {
            if (turnTime > TimeToChange)
            {
                turnTime = 0;
                SwapPlayers();
                WaitingToSwap = false;
            }
            else
            {
                turnTime += Time.deltaTime;
            }
            
            float timeToDisplay = TimeToChange - turnTime;
            if (timeToDisplay.ToString().Length > 1)
            {
                turnTimer.text = "Changing in:" + timeToDisplay.ToString().Substring(0, 2);
            }
            turnTimer.text = "Changing in:" + timeToDisplay.ToString();
        }
    }

    void StartSwap()
    {
        WaitingToSwap = true;
    }

    void SwapPlayers()
    {
        Debug.Log("swapping players");
        // needs if statement based on game mode
        handRef.swapHand();
        boardRef.SwapMinons();
        OtherPlayer.StartTurn();
    }
}
