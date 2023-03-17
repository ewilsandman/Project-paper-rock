using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Hand : MonoBehaviour // really this should be called player
{
    public int maxCards = 7;

    private Vector3 Originpos;
    [SerializeField] private Vector3 OfsetPos;
    [SerializeField] private Board board;
    [SerializeField] private List<BaseCard> cardsInHand;
    [SerializeField] private CoreLoop TurnHandler;
    [SerializeField] private PlayerCharacter PlayerCharacter;
    private int PlayerFunds;
    [SerializeField] private int BaseFunds;
    [SerializeField] private Text FundDisplay;
    public Pile pile;

    public bool Player1;

    private GameObject AttackerRef;
    private GameObject TargetRef;
    
    // Start is called before the first frame update
    void Start()
    {
        cardsInHand = new List<BaseCard>();
        Originpos = transform.position;
        UpdateDisplay();
        DrawCards();
    }

    void GetCardToHand()
    {
        BaseCard template = pile.PileToHand();
        BaseCard toBeAdded = Instantiate(template, transform);
        toBeAdded.handRef = this;
        toBeAdded.boardRef = board;
        toBeAdded.turnHandler = TurnHandler;
        Debug.Log("Card added: " + toBeAdded.name);
        cardsInHand.Add(toBeAdded);
    }

    public void RemoveCard(BaseCard card)
    {
        cardsInHand.Remove(card);
        Destroy(card.gameObject);
    }

    void CardPositions()
    {
        //Debug.Log("Start loop");
        for (int i = 0; i < maxCards; i++)
        {
            cardsInHand[i].gameObject.transform.localPosition = Originpos + OfsetPos * i;
            //CurrentHand[i].gameObject.GetComponent<RectTransform>().pivot = Originpos + OfsetPos;
            float x = Originpos.x + (OfsetPos.x * i);
            //cardsInHand[i].NewPos(x, 0);
            cardsInHand[i].UpdateTextFields();
        }
    }

    public void DrawCards()
    {
        if (cardsInHand.Count < maxCards)
        {
            Debug.Log("Getting cards");
            for (int i = cardsInHand.Count; i < maxCards; i++)
            {
                GetCardToHand();
            }
            CardPositions();
        }
    }

    private void UpdateDisplay()
    {
        FundDisplay.text = "Funds: " + PlayerFunds;
    }

    public bool CheckCost(int cost)
    {
        if (cost < PlayerFunds)
        {
            return true;
        }
        Debug.Log("poor lol");
        return false;
    }

    public void UpdateFunds(int deltaFunds)
    {
        PlayerFunds += deltaFunds;
        UpdateDisplay();
    }
    public void ResetFunds()
    {
        PlayerFunds = BaseFunds;
        UpdateDisplay();
    }
    public void HandleAttack()
    {
        
        if (AttackerRef != null && TargetRef != null)
        {
            AttackerRef.GetComponent<Minion>().Attack(TargetRef);
            AttackerRef = null;
            TargetRef = null;
        }
    }

    public void setAttacker(GameObject attacker)
    {
        Debug.Log("Attacker set");
        AttackerRef = attacker;
    }
    public void setTarget(GameObject target)
    {
        Debug.Log("Target Set");
        TargetRef = target;
        HandleAttack();
    }
}
