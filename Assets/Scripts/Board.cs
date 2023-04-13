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
    private Transform _activePlayerMinionPositions;

    private Targetable _targetRef;
    private Minion _attackerRef;

    [FormerlySerializedAs("playerMinions")] public List<Minion> activePlayerMinions;
    [FormerlySerializedAs("playerMinionPositions")] public Transform player1MinionPositions; // cursed way of doing things
    [FormerlySerializedAs("hostileMinions")] public List<Minion> passivePlayerMinions;
    [FormerlySerializedAs("hostileMinionPositions")] public Transform player2MinionPositions;
    
    
    [FormerlySerializedAs("HostileMinionExample")] [SerializeField] private Minion hostileMinionExample;

    [SerializeField]
    public enum GameMode
    {
        over,
        singleplayer,
        hotSeat,
        multiPlayer,
    }

    [SerializeField] public GameMode mode;
   /* [SerializeField] private bool singlePlayer;
    [SerializeField] private bool hotSeat;*/
    
    [SerializeField] private float timeLimit = 120;
    [SerializeField] private int turnsElapsed;
    [SerializeField] private float timeThisTurn; 
    [SerializeField] private Text turnTimer;
    
    // hot seat stuff
    [SerializeField] private GameObject blocker; // to block while hot seat swaps
    private bool _waitingToSwap;
    [SerializeField] private float swapTimeLimit;
    [SerializeField] private GameObject blueBackground;
    [SerializeField] private GameObject orangeBackground;

    private BaseCard _activeSpell;
    
    [SerializeField]private Text _killText;

    [SerializeField] private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        activePlayerMinions = new List<Minion>();
        passivePlayerMinions = new List<Minion>();
        //Time.timeScale = 0;
        _killText.enabled = false;
        paused = true;
    }

    public void stopGame( PlayerCharacter hasDied)
    {
        blocker.SetActive(true); // not working????
        _killText.enabled = true;
        _killText.text = hasDied.hand.gameObject.name + " Has lost, other player wins!";
        turnTimer.text = "";
        mode = GameMode.over;
        //Time.timeScale = 0;
    }

    public void EndTurn()
    {
        if (mode == GameMode.over)
        {
            return;
        }
        
        if (activePlayer)
        {
            activePlayer.friendly = false;
        }
        timeThisTurn = 0;
        if (turnsElapsed == 0)
        {
            Debug.Log("game started");
            int random = Random.Range(0, 2);
            switch (random)
            {
                case 0:
                    activePlayer = player1Hand;
                    _activePlayerMinionPositions = player1MinionPositions;
                    break;
                    case 1: 
                        activePlayer = player2Hand;
                        _activePlayerMinionPositions = player2MinionPositions;
                        List<Minion> tempMinions = new List<Minion>(activePlayerMinions); // to ensure continuity between friendly or enemy
                        activePlayerMinions = new List<Minion>(passivePlayerMinions);
                        passivePlayerMinions = new List<Minion>(tempMinions);
                        break;

            }
        }
        else
        {
            Debug.Log("next turn");
            if (mode == GameMode.hotSeat)
            {
                StartSwap();
            }
            else if (mode == GameMode.singleplayer)
            {
                Debug.Log("Ai turn swap");
                activePlayer = activePlayer == player1Hand ? player2Hand : player1Hand;
                //activePlayer = activePlayer == player1Hand ? player2Hand : player1Hand;
            }
            else
            {
                Debug.LogError("No game mode selected!");
            }
            foreach (Minion m in passivePlayerMinions)
            {
                if (m != null)
                {
                    m.ResetAttack();
                }
            }
            _activePlayerMinionPositions = _activePlayerMinionPositions == player1MinionPositions ? player2MinionPositions : player1MinionPositions;
            List<Minion> tempMinions = new List<Minion>(activePlayerMinions); // to ensure continuity between friendly or enemy
            activePlayerMinions = new List<Minion>(passivePlayerMinions);
            passivePlayerMinions = new List<Minion>(tempMinions);
        }
        _activeSpell = null;
        StartTurn();
        turnsElapsed++;
    }

    private void StartTurn()
    {
        paused = false;
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
        if (mode == GameMode.over)
        {
            turnTimer.text = "Game over";
            timeThisTurn = 0;
            return;
        }

        if (paused)
        {
            turnTimer.text = "Press the arrow to start";
            timeThisTurn = 0;
            return;
        }

        if (timeThisTurn > timeLimit)
        {
            Debug.Log("Times up"); 
            EndTurn();
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
        

        if (mode == GameMode.hotSeat)
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

    public void AddAttacker(Minion input)
    {
        if (_activeSpell != null)
        {
            SpellAction(_activeSpell, input.GetComponent<Targetable>()); // hacky
        }
        else
        {
            if (_attackerRef)
            {
                _attackerRef.TryGetComponent<Minion>(out Minion minion);
                minion.ChangeColour(false);
            }
            Debug.Log("attacker added");
            _attackerRef = input;
            HandleAttack();
            if (_activeSpell != null)
            {
                _activeSpell.ChangeColour(false);
            }
            _activeSpell = null; 
        }
    }
    public void AddTarget(Targetable input)
    {
        if (_activeSpell != null)
        {
            SpellAction(_activeSpell, input);
        }
        else 
        {
            if (_targetRef)
            {
                _targetRef.gameObject.TryGetComponent<Minion>(out Minion minion);
                minion.ChangeColour(false);
            }
            
            Debug.Log("target added");
            _targetRef = input;
            HandleAttack();
            if (_activeSpell != null)
            {
                _activeSpell.ChangeColour(false);
            }
            _activeSpell = null;
        }
    }

    private void SpellAction(BaseCard spell, Targetable target)
    {
        if (spell.handRef.playerFunds >= spell.cost)
        {
            spell.handRef.UpdateFunds(-spell.cost);
          target.GetComponent<Minion>();

          if (spell.strength > 0)
          {
              target.DeltaHealth(-spell.strength);
          }

          else // assume healing
          { 
              target.DeltaHealth(spell.health);
          }

          Destroy(spell.gameObject);
        }
    }

    private void HandleAttack()
    {
        
        if (_targetRef != null & _attackerRef != null)
        {
            if (_targetRef.GetComponent<Minion>() as ShieldMinion)
            {
                
            }

            if (_targetRef.TryGetComponent(out ShieldMinion shieldminionComponent)) // check to see if target is shieldMinion
            {
                
            }
            else
            {
                foreach (Minion minion in passivePlayerMinions)
                {
                    if (minion != null)
                    {
                        minion.TryGetComponent(out shieldminionComponent); // if there is another minion with ShieldMinion it will be targeted instead
                        {
                            _targetRef = minion.GetComponent<Targetable>();
                            break;
                        }
                    }
                }  
            }
            _attackerRef.GetComponent<Minion>().Attack(_targetRef);
            _attackerRef = null;
            _targetRef = null;
        }
    }

    public bool PlaceForMinion(bool friendly)  // should be local to player
    {
        for (int i = 0; i < maxMinions; i++)
        {
            if (_activePlayerMinionPositions.GetChild(i).childCount == 0) // risk of failing
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

    public void AddMinion(Minion template, int health, int strength, string minionName,string describeString, bool friendly) // could be remade to handle multiple spawns
    {
        Minion toBeCreated = Instantiate(template, transform.parent);
        toBeCreated.healthPool.maxHealth = health;
        toBeCreated.healthPool.setup();
        toBeCreated.strength = strength;
        toBeCreated.minionName = minionName;
        toBeCreated.descriptionString = describeString;
        toBeCreated.boardRef = this;
        toBeCreated.playerHand = activePlayer;
        if (friendly) // unused
        {
            activePlayerMinions.Add(toBeCreated);
            for (int i = 0; i < maxMinions; i++)
            {
                if (_activePlayerMinionPositions.GetChild(i).childCount == 0)
                {
                    toBeCreated.gameObject.transform.position = _activePlayerMinionPositions.GetChild(i).position;
                    toBeCreated.transform.SetParent( _activePlayerMinionPositions.GetChild(i), true);
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

    public void RemoveMinon(GameObject target)
    {
        Debug.Log("removing minion");
        Minion minionRef = target.GetComponent<Minion>();
        if (activePlayerMinions.Contains(minionRef))
        {
            activePlayerMinions.Remove(minionRef);
            Debug.Log("also from list");
        }
        else
        {
            passivePlayerMinions.Remove(minionRef);
            Debug.Log("also from list");
        }
        
        Destroy(target);
    }

    public void SpawnExampleEnemy()// debug purposes 
    {
        if (PlaceForMinion(false))
        {
            AddMinion(hostileMinionExample, 2,3,"Example" ,"I'm for testing purposes" ,false);
        }
    }

    public bool HandleCard(BaseCard toHandle, Hand from)
    {
        if (toHandle.minionSpawning)
        {
            AddMinion(toHandle.minionRef, toHandle.health,toHandle.strength, toHandle.minionName, toHandle.descriptionString,from.friendly);
            from.UpdateFunds(-toHandle.cost);
            if (from.friendly)
            {
                //TODO: send to other player in multiplayer to add minion from 
            }
            return true;
        }
        else // assuming only spells and minions
        {
         HandleSpellCard(toHandle);
        }
        return false;
    }

    private void HandleSpellCard(BaseCard input)
    {
        _activeSpell = input;
        _activeSpell.ChangeColour(true);
        _targetRef = null;
        _attackerRef = null;
    }

    public void SwapMinons()
    {

        foreach (Minion pm in activePlayerMinions)
        {
            pm.transform.position = player1MinionPositions.GetChild(activePlayerMinions.IndexOf(pm)).position;
            pm.transform.SetParent(player1MinionPositions.GetChild(activePlayerMinions.IndexOf(pm)));
            pm.ResetAttack();
        }
        
        foreach (Minion hm in passivePlayerMinions)
        {
            if (hm != null)
            {
                hm.transform.position = player2MinionPositions.GetChild(passivePlayerMinions.IndexOf(hm)).position;
                hm.transform.SetParent(player2MinionPositions.GetChild(passivePlayerMinions.IndexOf(hm)));
                hm.ResetAttack();
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
