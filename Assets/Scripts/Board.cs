using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Board : MonoBehaviour // this will handle enemy attacks
{
    [SerializeField] private int maxMinions = 5;

    [FormerlySerializedAs("CoreLoop1Ref")] [SerializeField] private CoreLoop CurrentLoopRef; // active player
    [FormerlySerializedAs("CoreLoop2Ref")] [SerializeField] private CoreLoop OtherLoopRef; // inactive player
    [SerializeField] private Hand player1Hand;
    [SerializeField] private Hand player2Hand;

    private GameObject _targetRef;
    private GameObject _attackerRef;

    public Minion[] PlayerMinions;
    public Transform playerMinionPositions; // cursed way of doing things
    public Minion[] HostileMinions;
    public Transform hostileMinionPositions;
    private Vector3 Originpos;
    
    [SerializeField] private Vector3 OffsetPos;
    [SerializeField] private Vector3 HostileOffset;
    [SerializeField] private Minion HostileMinionExample;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerMinions = new Minion[maxMinions];
        HostileMinions = new Minion[maxMinions];
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
                if (!PlayerMinions[i])
                {
                    Debug.Log("Place found!");
                    return true;
                }
            }
            else
            {
                if (!HostileMinions[i])
                {
                    return true;
                }
            }
        }
        Debug.Log("No place");
        return false;
    }

    public void AddMinion(Minion template, int health, int strength, bool friendly) // could be remade to handle multiple spawns
    {
        Minion toBeCreated = Instantiate(template, transform.parent);
        toBeCreated.health = health;
        toBeCreated.strength = strength;
        toBeCreated.boardRef = this;
        toBeCreated.turnHandler = CurrentLoopRef;
        toBeCreated.playerHand = player1Hand;
        if (friendly)
        {
            for (int i = 0; i < maxMinions; i++)
            {
                if (!PlayerMinions[i])
                {
                    PlayerMinions[i] = toBeCreated;
                    toBeCreated.gameObject.transform.position = playerMinionPositions.GetChild(i).position;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < maxMinions; i++)
            {
                if (!HostileMinions[i])
                {
                    HostileMinions[i] = toBeCreated;
                    toBeCreated.gameObject.transform.position = hostileMinionPositions.GetChild(i).position;
                    break;
                }
            }
            //toBeCreated.gameObject.transform.localPosition = transform.parent.position + HostileOffset;
        }
    }

    public void RemoveMinon(GameObject target)
    {
        Destroy(target);
    }

    public void SpawnExampleEnemy()// debug purposes 
    {
        if (PlaceForMinion(false))
        {
            AddMinion(HostileMinionExample, 2,3,false);
        }
    }

    public void SwapMinons()
    {
        Minion[] tempMinions = new Minion[maxMinions];
        tempMinions = PlayerMinions;
        PlayerMinions = HostileMinions;
        HostileMinions = tempMinions;
        
        for (int i = 0; i < maxMinions; i++)
        {
            if (PlayerMinions[i])
            {
                PlayerMinions[i].transform.position = playerMinionPositions.GetChild(i).position;
                PlayerMinions[i].ResetAttack();
                //PlayerMinions[i].turnHandler = CoreLoop2Ref;
            }
            if (HostileMinions[i])
            {
                HostileMinions[i].transform.position = hostileMinionPositions.GetChild(i).position;
                HostileMinions[i].ResetAttack();
                //HostileMinions[i].turnHandler = CoreLoop1Ref;
            }
        }

        (CurrentLoopRef, OtherLoopRef) = (OtherLoopRef, CurrentLoopRef); 
        (player1Hand, player2Hand) = (player2Hand, player1Hand);// conditional on gamemode
    }
}
