using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Minion : MonoBehaviour // will work similar to card
{

    private bool HasAttacked = true;
    
    public Board boardRef;
    public Hand playerHand;
    public int health;
    public int strength;
    public string minionName;
    
    [SerializeField] public Text healthText;
    [SerializeField] public Text strengthText;
    [SerializeField] public Text nameText;

    private void Start()
    {
        UpdateTextFields();
        ResetAttack();
    }

    public bool CheckAttack()
    {
        return HasAttacked;
    }
    

    public void ResetAttack()
    {
        HasAttacked = false;
    }

    // private Hand playerHand;
    // Start is called before the first frame update
    public void ButtonResponse()
    {
        //playerHand.HandleMinionClick();
        if (playerHand.friendly)
        {
            Debug.Log("have not attacked");
            boardRef.AddAttacker(gameObject);
        }
        else
        {
            boardRef.AddTarget(gameObject);
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
        if (HasAttacked)
        {
            return;
        }
        Debug.Log("attack done");
        Minion miniontargetScript;
        PlayerCharacter playerTargetScript;
        if (target.TryGetComponent(out miniontargetScript)) // this could be solved by having a more general "healthComponent"
        {
            miniontargetScript.DeltaHealth(-strength);
            DeltaHealth(-miniontargetScript.strength);
        }
        else if (target.TryGetComponent(out playerTargetScript))
        {
            playerTargetScript.DeltaHealth(-strength);
        }

        HasAttacked = true;
    }

    private void Kill() // this would handle killing any "shadow" in multiplayer as well
    {
        Debug.Log("starting kill");
        Board.RemoveMinon(gameObject);
    }
    private void UpdateTextFields()
    {   
        healthText.text = health.ToString();
        strengthText.text = strength.ToString();
        nameText.text = minionName; // unlikely to change
    }
}
