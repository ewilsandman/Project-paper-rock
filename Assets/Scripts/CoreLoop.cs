using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CoreLoop : MonoBehaviour
{
    // this is all local to the current player
    // public float playerNumber;
    [FormerlySerializedAs("TimeToChange")] [SerializeField] private float timeToChange;
    public float timeLimit = 120; // seconds
    public float turnTime; // time turn has elapsed, counts up

    public float turnsElapsed; // turns since start
    [FormerlySerializedAs("TurnActive")] public bool turnActive;
    private bool _waitingToSwap;
    [SerializeField] private Hand handRef;
    [FormerlySerializedAs("OtherPlayer")] [SerializeField] private CoreLoop otherPlayer;

    [SerializeField] private Board boardRef;
    [SerializeField] private Text turnTimer;
    [SerializeField] private PlayerCharacter playerChar;
    [SerializeField] private PlayerCharacter hostileChar;

    [FormerlySerializedAs("BlueBackground")] [SerializeField] private GameObject blueBackground;
    [FormerlySerializedAs("OrangeBackground")] [SerializeField] private GameObject orangeBackground;
    [SerializeField] private GameObject blocker;

    public void StartTurn()
    {
        Debug.Log("now player" + gameObject.name);
        turnTime = 0;
        turnActive = true;
        if (turnsElapsed >0)
        { 
            handRef.DrawCards();
        }
        handRef.ResetFunds();
        blocker.SetActive(false);
    }

    public void EndTurn()
    {
        turnTime = 0;
        turnsElapsed++;
        turnActive = false;
        StartSwap();
         //OtherPlayer.StartTurn();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (turnActive)
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
            if (timeToDisplay.ToString().Length > 1)
            {
                turnTimer.text = "Time left:" + timeToDisplay.ToString().Substring(0, 2);
            }
            turnTimer.text = "Time left:" + timeToDisplay.ToString();
        }
        else if (_waitingToSwap)
        {
            if (turnTime > timeToChange)
            {
                turnTime = 0;
                SwapPlayers();
                _waitingToSwap = false;
            }
            else
            {
                turnTime += Time.deltaTime;
            }
            
            float timeToDisplay = timeToChange - turnTime;
            if (timeToDisplay.ToString().Length > 1)
            {
                turnTimer.text = "Changing in:" + timeToDisplay.ToString().Substring(0, 2);
            }
            turnTimer.text = "Changing in:" + timeToDisplay.ToString();
        }
    }

    void ToggleGameMode() // changes between hot seat and PvE
    {
        
    }

    void StartSwap()
    {
        (blueBackground.GetComponent<Image>().color, orangeBackground.GetComponent<Image>().color) = 
            (orangeBackground.GetComponent<Image>().color, blueBackground.GetComponent<Image>().color);

        blocker.SetActive(true);
        
        _waitingToSwap = true;
    }

    void SwapPlayers()
    {
        Debug.Log("swapping players");
        // needs if statement based on game mode
        playerChar.SwapSides(hostileChar);
        handRef.SwapHand();
        boardRef.SwapMinons();
        otherPlayer.StartTurn();
    }
}
