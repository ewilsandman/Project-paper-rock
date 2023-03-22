using UnityEngine;
using UnityEngine.UI;

public class Minion : MonoBehaviour // will work similar to card
{
    //public bool friendly = false; // shit
    public CoreLoop turnHandler;

    private bool _attackedThisTurn;

    public Board boardRef;
    public Hand playerHand;
    public int health;
    public int strength;
    
    [SerializeField] public Text healthText;
    [SerializeField] public Text strengthText;

    private void Start()
    {
        UpdateTextFields();
        ResetAttack();
    }

    public void ResetAttack()
    {
        _attackedThisTurn = false;
    }

    // private Hand playerHand;
    // Start is called before the first frame update
    public void ButtonResponse()
    {
        if (turnHandler.TurnActive)
        {
            if (playerHand.Friendly)
            {
                boardRef.AddAttacker(gameObject);
            }
            else
            {
                boardRef.AddTarget(gameObject);
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
        UpdateTextFields();
    }

    public void Attack(GameObject target)
    {
        if (_attackedThisTurn)
        {
            return;
        }
        Debug.Log("attack done");
        Minion MiniontargetScript;
        PlayerCharacter PlayerTargetScript;
        if (target.TryGetComponent<Minion>(out MiniontargetScript)) // this could be solved by having a more general "healthComponent"
        {
            MiniontargetScript.DeltaHealth(-strength);
            DeltaHealth(-MiniontargetScript.strength);
        }
        else if (target.TryGetComponent<PlayerCharacter>(out PlayerTargetScript))
        {
            PlayerTargetScript.DeltaHealth(-strength);
        }

        _attackedThisTurn = true;
    }

    private void Kill() // this would handle killing any "shadow" in multiplayer as well
    {
        Debug.Log("starting kill");
        boardRef.RemoveMinon(gameObject);
    }
    private void UpdateTextFields()
    {   
        healthText.text = health.ToString();
        strengthText.text = strength.ToString();
    }
}
