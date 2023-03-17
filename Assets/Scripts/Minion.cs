using System;
using System.Collections;
using UnityEngine;

public class Minion : MonoBehaviour // will work similar to card
{
    public bool friendly = false;
    public CoreLoop turnHandler;

    public Board boardRef;
    public Hand playerHand;
    public int health;
    public int strength;

    // private Hand playerHand;
    // Start is called before the first frame update
    public void ButtonResponse()
    {
        if (turnHandler.TurnActive)
        {
            if (friendly)
            {
                playerHand.setAttacker(gameObject);
            }
            else
            {
                playerHand.setTarget(gameObject);
            }
        } 
    }

    public void DeltaHealth(int delta)
    {
        if (health + delta <= 0)
        {
            Kill();
        }
        else
        {
            health += delta;
        }
    }

    public void Attack(GameObject target)
    {
        Debug.Log("attack done");
        Minion targetScript = target.GetComponent<Minion>();
        targetScript.DeltaHealth(-strength);
        DeltaHealth(-targetScript.strength);
    }

    private void Kill() // this would handle killing any "shadow" in multiplayer as well
    {
        Debug.Log("starting kill");
        boardRef.RemoveMinon(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
