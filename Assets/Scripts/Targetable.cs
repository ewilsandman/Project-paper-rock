using UnityEngine;

public class Targetable : MonoBehaviour
{ 
    [SerializeField] private int health;
    public int maxHealth;
    private int _resistance;
    [SerializeField] private Minion minionRef;
    [SerializeField] private PlayerCharacter playerCharacterRef;

    public void DeltaHealth(int delta)
    {
        if (health + delta <= 0)
        {
            CallKill();
        }
        else if ( health + delta > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += delta;
        }

        if (minionRef != null)
        {
            minionRef.UpdateTextFields();
        }
        else
        {
            playerCharacterRef.UpdateTextFields();
        }
    }

    public void Setup()
    {
        health = maxHealth;
    }

    public int ReturnHealth()
    {
        return health;
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
