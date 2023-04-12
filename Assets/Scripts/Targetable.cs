using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    private int _health;
    public int maxHealth;
    private int _resistance;
    [SerializeField] private Minion minionRef;
    [SerializeField] private PlayerCharacter playerCharacterRef;

    public void DeltaHealth(int delta)
    {
        if (_health + delta <= 0)
        {
            CallKill();
        }
        else if ( _health + delta > maxHealth)
        {
            _health = maxHealth;
        }
        else
        {
            _health += delta;
        }
    }

    public void setup()
    {
        _health = maxHealth;
    }

    public int ReturnHealth()
    {
        return _health;
    }

    private void CallKill()
    {
        if (minionRef != null)
        {
            minionRef.Kill();
        }
        else if (playerCharacterRef != null)
        {
            playerCharacterRef.Kill();
        }
        else
        {
            Debug.LogError("Targetable tried to call Kill but no gameObject bound");
        }
    }

}
