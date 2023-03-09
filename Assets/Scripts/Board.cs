using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour // this will handle enemy attacks
{
    [SerializeField] private int maxMinions = 5;
    
    [SerializeField] private Minion[] PlayerMinions;
    [SerializeField] private Minion[] HostileMinions;
    private Vector3 Originpos;
    [SerializeField] private Vector3 OffsetPos;
    [SerializeField] private Vector3 HostileOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerMinions = new Minion[maxMinions];
        HostileMinions = new Minion[maxMinions];
    }

    public bool PlaceforMinion() // assumes no desync
    {
        for (int i = 0; i < maxMinions; i++)
        {
            if (!PlayerMinions[i])
            {
                Debug.Log("Place found!");
                return true;
            }
        }
        Debug.Log("No place");
        return false;
    }

    public void AddMinion(Minion template, bool friendly) // could be remade to handle multiple spawns
    {
        Minion toBeCreated = Instantiate(template, transform.parent);
        if (friendly)
        {
            for (int i = 0; i < maxMinions; i++)
            {
                if (!PlayerMinions[i])
                {
                    PlayerMinions[i] = toBeCreated;
                    toBeCreated.gameObject.transform.position = transform.position + OffsetPos * i;
                    break;
                }
            }
            toBeCreated.GetComponent<Minion>().friendly = true;
        }
        else
        {
            toBeCreated.gameObject.transform.position = OffsetPos + HostileOffset;
        }
    }

    public void DestroyMinion()
    {
        throw new NotImplementedException();
    }
}
