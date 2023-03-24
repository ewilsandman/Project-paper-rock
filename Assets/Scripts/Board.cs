using UnityEngine;
using UnityEngine.Serialization;

public class Board : MonoBehaviour // this will handle enemy attacks
{
    [SerializeField] private int maxMinions = 5;

    [FormerlySerializedAs("CurrentLoopRef")] [SerializeField] private CoreLoop currentLoopRef; // active player
    [FormerlySerializedAs("OtherLoopRef")] [SerializeField] private CoreLoop otherLoopRef; // inactive player
    [SerializeField] private Hand player1Hand;
    [SerializeField] private Hand player2Hand;

    private GameObject _targetRef;
    private GameObject _attackerRef;

    public Minion[] playerMinions;
    public Transform playerMinionPositions; // cursed way of doing things
    public Minion[] hostileMinions;
    public Transform hostileMinionPositions;
    //private Vector3 _originpos;
    
    [FormerlySerializedAs("OffsetPos")] [SerializeField] private Vector3 offsetPos;
    [FormerlySerializedAs("HostileOffset")] [SerializeField] private Vector3 hostileOffset;
    [FormerlySerializedAs("HostileMinionExample")] [SerializeField] private Minion hostileMinionExample;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMinions = new Minion[maxMinions];
        hostileMinions = new Minion[maxMinions];
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

    public bool PlaceForMinion(bool friendly) 
    {
        for (int i = 0; i < maxMinions; i++)
        {
            if (friendly)
            {
                if (!playerMinions[i])
                {
                    return true;
                }
            }
            else
            {
                if (!hostileMinions[i])
                {
                    return true;
                }
            }
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
        toBeCreated.turnHandler = currentLoopRef;
        toBeCreated.playerHand = player1Hand;
        toBeCreated.transform.SetParent(transform, true);
        if (friendly)
        {
            for (int i = 0; i < maxMinions; i++)
            {
                if (!playerMinions[i])
                {
                    playerMinions[i] = toBeCreated;
                    toBeCreated.gameObject.transform.position = playerMinionPositions.GetChild(i).position;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < maxMinions; i++)
            {
                if (!hostileMinions[i]) // only used for debug
                {
                    hostileMinions[i] = toBeCreated;
                    toBeCreated.gameObject.transform.position = hostileMinionPositions.GetChild(i).position;
                    break;
                }
            }
            //toBeCreated.gameObject.transform.localPosition = transform.parent.position + HostileOffset;
        }
    }

    public void TurnButton()
    {
        currentLoopRef.EndTurn();
    }

    public void RemoveMinon(GameObject target)
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

    public void SwapMinons()
    {
        Minion[] tempMinions = new Minion[maxMinions];
        tempMinions = playerMinions;
        playerMinions = hostileMinions;
        hostileMinions = tempMinions;
        
        for (int i = 0; i < maxMinions; i++)
        {
            if (playerMinions[i])
            {
                playerMinions[i].transform.position = playerMinionPositions.GetChild(i).position;
                playerMinions[i].ResetAttack();
                //PlayerMinions[i].turnHandler = CoreLoop2Ref;
            }
            if (hostileMinions[i])
            {
                hostileMinions[i].transform.position = hostileMinionPositions.GetChild(i).position;
                hostileMinions[i].ResetAttack();
                //HostileMinions[i].turnHandler = CoreLoop1Ref;
            }
        }

        (currentLoopRef, otherLoopRef) = (otherLoopRef, currentLoopRef); 
        (player1Hand, player2Hand) = (player2Hand, player1Hand);// conditional on gamemode
    }
}
