using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Hand : MonoBehaviour // really this should be called player
{
    public int maxCards;

    private Vector3 Originpos;
    [SerializeField] private Vector3 OfsetPos;
    [SerializeField] private Board board;
    [SerializeField] private Hand otherHand;
    [SerializeField] private List<BaseCard> cardsInHand;
    [SerializeField] private CoreLoop TurnHandler;
    [SerializeField] private PlayerCharacter PlayerCharacter;
    
    private int PlayerFunds;
    [SerializeField] private int BaseFunds;
    public Pile pile;

    public bool Friendly;
    
    
    // Start is called before the first frame update
    void Start()
    {
        cardsInHand = new List<BaseCard>();
        Originpos = transform.position;
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
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            if (cardsInHand[i])
            {
                cardsInHand[i].gameObject.transform.position = transform.GetChild(i).position;
                cardsInHand[i].UpdateTextFields(); 
            }
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
    }
    public void ResetFunds()
    {
        PlayerFunds = BaseFunds;
    }

    public void swapHand() // done by active player before turn swaps
    {
        otherHand.Friendly = true;
        (cardsInHand, otherHand.cardsInHand) = (otherHand.cardsInHand, cardsInHand); // no Idea how this works
        otherHand.CardPositions();
        Friendly = false;
        CardPositions();
        
    }
}
