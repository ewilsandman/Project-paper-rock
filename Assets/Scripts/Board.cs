using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour // this will handle enemy attacks
{
    [SerializeField] private int maxMinions = 5;
    [Header("core")]
   /* [FormerlySerializedAs("CurrentLoopRef")] [SerializeField] private CoreLoop currentLoopRef; // active player
    [FormerlySerializedAs("OtherLoopRef")] [SerializeField] private CoreLoop otherLoopRef; // inactive player*/
    [SerializeField] private Hand player1Hand;
    [SerializeField] private Hand player2Hand;
    //[SerializeField] private  player1Character
    [SerializeField]

    public Hand activePlayer;
    private Transform _activeMinionPositions;

    private GameObject _targetRef;
    private GameObject _attackerRef;

    public List<Minion> playerMinions;
    public Transform playerMinionPositions; // cursed way of doing things
    public List<Minion> hostileMinions;
    public Transform hostileMinionPositions;
    
    
    [FormerlySerializedAs("HostileMinionExample")] [SerializeField] private Minion hostileMinionExample;

    [SerializeField] private bool singlePlayer;
    [SerializeField] private bool hotSeat;
    
    [SerializeField] private float timeLimit = 120;
    [SerializeField] private int _turnsElapsed = 0;
    [SerializeField] private float timeThisTurn; 
    [SerializeField] private Text turnTimer;
    
    // hot seat stuff
    [SerializeField] private GameObject blocker; // to block while hot seat swaps
    private bool _waitingToSwap;
    [SerializeField] private float swapTimeLimit;
    [SerializeField] private GameObject blueBackground;
    [SerializeField] private GameObject orangeBackground;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        playerMinions = new List<Minion>();
        hostileMinions = new List<Minion>();
    }

    public void EndTurn()
    {
        if (activePlayer)
        {
            activePlayer.friendly = false;
        }
        timeThisTurn = 0;
        if (_turnsElapsed == 0)
        {
            Debug.Log("game started");
            int random = Random.Range(0, 2);
            switch (random)
            {
                case 0:
                    activePlayer = player1Hand;
                    _activeMinionPositions = playerMinionPositions;
                    break;
                    case 1: 
                        activePlayer = player2Hand;
                        _activeMinionPositions = hostileMinionPositions;
                        List<Minion> tempMinions = new List<Minion>(); // to ensure continuity between friendly or enemy
                        tempMinions = playerMinions;
                        playerMinions = hostileMinions;
                        hostileMinions = tempMinions;
                        break;

            }
        }
        else
        {
            Debug.Log("next turn");
            if (hotSeat)
            {
                StartSwap();
            }
            else if (singlePlayer)
            {
                Debug.Log("Ai turn swap");
                activePlayer = activePlayer == player1Hand ? player2Hand : player1Hand;
                //activePlayer = activePlayer == player1Hand ? player2Hand : player1Hand;
            }
            else
            {
                Debug.LogError("No game mode selected!");
            }
            foreach (Minion m in hostileMinions)
            {
                if (m != null)
                {
                    m.ResetAttack();
                }
            }
            _activeMinionPositions = _activeMinionPositions == playerMinionPositions ? hostileMinionPositions : playerMinionPositions;
            List<Minion> tempMinions = new List<Minion>();
            tempMinions = playerMinions;
            playerMinions = hostileMinions;
            hostileMinions = tempMinions;
        }
        StartTurn();
        _turnsElapsed++;
    }

    private void StartTurn()
    {
        activePlayer.friendly = true;
        Debug.Log("now player" + activePlayer);
        timeThisTurn = 0;
        _attackerRef = null;
        _targetRef = null;
        activePlayer.DrawCards();
        activePlayer.ResetFunds();
        blocker.SetActive(false);
        activePlayer.StartTurn(); // Activates AI if ai controlled
    }
    void StartSwap()// multiplayer does not need this
    {
        (blueBackground.GetComponent<Image>().color, orangeBackground.GetComponent<Image>().color) = 
            (orangeBackground.GetComponent<Image>().color, blueBackground.GetComponent<Image>().color);

        blocker.SetActive(true);
        
        _waitingToSwap = true;
    }

    void FixedUpdate()
    {

        if (timeThisTurn > timeLimit)
        {
            Debug.Log("Times up"); 
            //endTurn();
        }
        else // gives you one extra frame but whatever
        { 
            timeThisTurn += Time.deltaTime;
        }

        float timeToDisplay = timeLimit - timeThisTurn;
        if (timeToDisplay.ToString().Length > 1)
        { 
            turnTimer.text = "Time left:" + timeToDisplay.ToString().Substring(0, 2);
        }
        turnTimer.text = "Time left:" + timeToDisplay.ToString();
        

        if (!singlePlayer)
        {
            if (_waitingToSwap)
            {
                if (timeThisTurn > swapTimeLimit)
                {
                    timeThisTurn = 0;
                    SwapMinons();
                    
                    //Hand nextHand = activePlayer == player1Hand ? player2Hand : player1Hand;
                    //activePlayer.Swap(nextHand, activePlayer.playerCharacter);
                    _waitingToSwap = false;
                }
                else
                {
                    timeThisTurn += Time.deltaTime;
                }

                timeToDisplay = swapTimeLimit - timeThisTurn;
                if (timeToDisplay.ToString().Length > 1)
                {
                    turnTimer.text = "Changing in:" + timeToDisplay.ToString().Substring(0, 2);
                }

                turnTimer.text = "Changing in:" + timeToDisplay;
            }
        }
    }

    public void AddAttacker(GameObject input)
    {
        _attackerRef = input;
        HandleAttack();
    }
    public void AddTarget(GameObject input)
    {
        _targetRef = input;
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (_targetRef != null & _attackerRef != null)
        {
            _attackerRef.GetComponent<Minion>().Attack(_targetRef);
            _attackerRef = null;
            _targetRef = null;
        }
    }

    public bool PlaceForMinion(bool friendly)  // should be local to player
    {
        for (int i = 0; i < maxMinions; i++)
        {
            if (_activeMinionPositions.GetChild(i).childCount == 0) // risk of failing
            {
                return true;
            }
            
           /* else
            {
                if (hostileMinionPositions.GetChild(i).childCount == 0)
                {
                    return true;
                }
            }*/
        }
        return false;
    }

    public void AddMinion(Minion template, int health, int strength, string minionName, bool friendly) // could be remade to handle multiple spawns
    {
        Minion toBeCreated = Instantiate(template, transform.parent);
        toBeCreated.health = health;
        toBeCreated.strength = strength;
        toBeCreated.minionName = minionName;
        toBeCreated.boardRef = this;
        toBeCreated.playerHand = activePlayer;
        if (friendly)
        {
            playerMinions.Add(toBeCreated);
            for (int i = 0; i < maxMinions; i++)
            {
                if (_activeMinionPositions.GetChild(i).childCount == 0)
                {
                    toBeCreated.gameObject.transform.position = _activeMinionPositions.GetChild(i).position;
                    toBeCreated.transform.SetParent( _activeMinionPositions.GetChild(i), true);
                    break;
                }
            }
        }
       /* else
        {
            hostileMinions.Add(toBeCreated);
            for (int i = 0; i < maxMinions; i++)
            {
                if (hostileMinionPositions.GetChild(i).childCount == 0)
                {
                    toBeCreated.gameObject.transform.position = hostileMinionPositions.GetChild(i).position;
                    toBeCreated.transform.SetParent( hostileMinionPositions.GetChild(i), true);
                    break;
                }
            }
            //toBeCreated.gameObject.transform.localPosition = transform.parent.position + HostileOffset;
        }*/
    }

    public void TurnButton()
    {
       EndTurn();
    }

    public static void RemoveMinon(GameObject target)
    {
        Destroy(target);
    }

    public void SpawnExampleEnemy()// debug purposes 
    {
        if (PlaceForMinion(false))
        {
            AddMinion(hostileMinionExample, 2,3,"example" ,false);
        }
    }

    public bool HandleCard(BaseCard toHandle, Hand from)
    {
        if (toHandle.minionSpawning)
        {
            AddMinion(toHandle.minionRef, toHandle.health,toHandle.strength, toHandle.minionName, from.friendly);
            from.UpdateFunds(-toHandle.cost);
            if (from.friendly)
            {
                //TODO: send to other player in multiplayer to add minion from 
            }
            return true;
        }
        else if (false)
        {
          //TODO: other types of cards   
        }
        return false;
    }

    public void SwapMinons()
    {

        foreach (Minion PM in playerMinions)
        {
            PM.transform.position = playerMinionPositions.GetChild(playerMinions.IndexOf(PM)).position;
            PM.transform.SetParent(playerMinionPositions.GetChild(playerMinions.IndexOf(PM)));
            PM.ResetAttack();
        }
        
        foreach (Minion HM in hostileMinions)
        {
            if (HM != null)
            {
                HM.transform.position = hostileMinionPositions.GetChild(hostileMinions.IndexOf(HM)).position;
                HM.transform.SetParent(hostileMinionPositions.GetChild(hostileMinions.IndexOf(HM)));
                HM.ResetAttack();
            }
        }
        
        /*for (int i = 0; i < maxMinions; i++)
        {
            if (playerMinions[i] !=)
            {
                playerMinions[i].transform.position = playerMinionPositions.GetChild(i).position;
                playerMinions[i].transform.SetParent(playerMinionPositions.GetChild(i));
                playerMinions[i].ResetAttack();
                //PlayerMinions[i].turnHandler = CoreLoop2Ref;
            }
            if (hostileMinions[i] != null)
            {
                hostileMinions[i].transform.position = hostileMinionPositions.GetChild(i).position;
                hostileMinions[i].transform.SetParent(hostileMinionPositions.GetChild(i));
                hostileMinions[i].ResetAttack();
                //HostileMinions[i].turnHandler = CoreLoop1Ref;
            }
        }*/
        //(currentLoopRef, otherLoopRef) = (otherLoopRef, currentLoopRef); 
        (player1Hand, player2Hand) = (player2Hand, player1Hand);
    }
}
