using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour // this will handle enemy attacks
{
    [SerializeField] private int maxMinions = 5;

    [SerializeField] private CoreLoop CoreLoop;
    [SerializeField] private Hand playerHand; //TODO: will need to be swapped when player swap
    
    public Minion[] PlayerMinions;
    public Minion[] HostileMinions;
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
        if (friendly)
        {
            for (int i = 0; i < maxMinions; i++)
            {
                if (!PlayerMinions[i])
                {
                    Minion toBeCreated = Instantiate(template, transform.parent);
                    PlayerMinions[i] = toBeCreated;
                    toBeCreated.gameObject.transform.localPosition = transform.parent.position + -HostileOffset + OffsetPos * i;
                    toBeCreated.health = health;
                    toBeCreated.strength = strength;
                    toBeCreated.boardRef = this;
                    toBeCreated.turnHandler = CoreLoop;
                    toBeCreated.playerHand = playerHand;
                    toBeCreated.friendly = true;
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
                    Minion toBeCreated = Instantiate(template, transform.parent);
                    HostileMinions[i] = toBeCreated;
                    toBeCreated.gameObject.transform.localPosition = transform.parent.position + HostileOffset + OffsetPos * i;
                    toBeCreated.health = health;
                    toBeCreated.strength = strength;
                    toBeCreated.boardRef = this;
                    toBeCreated.turnHandler = CoreLoop;
                    toBeCreated.playerHand = playerHand;
                    toBeCreated.friendly = false;
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

    public void SpawnWeakExampleEnemy()// debug purposes 
    {
        if (PlaceForMinion(false))
        {
            AddMinion(HostileMinionExample, 1, 1, false);
        }
    }

    public void DestroyMinion()
    {
        throw new NotImplementedException();
    }
}
