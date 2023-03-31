using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Hand : MonoBehaviour // really this should be called player
{
    public int maxCards;


    [SerializeField] private Board board;
    [SerializeField] private Hand otherHand;
    [SerializeField] private PlayerCharacter playerCharacter;
    [FormerlySerializedAs("TurnHandler")] [SerializeField] private CoreLoop turnHandler;
    
    [FormerlySerializedAs("_playerFunds")] public int playerFunds;
    [FormerlySerializedAs("BaseFunds")] [SerializeField] private int baseFunds;
    public Pile pile;
    public List<BaseCard> cardsInHand;

    [FormerlySerializedAs("Friendly")] public bool friendly;
    
    
    // Start is called before the first frame update
    void Start()
    {
        cardsInHand = new List<BaseCard>();
        DrawCards();
    }

    void GetCardToHand()
    {
        BaseCard template = pile.PileToHand();
        if (template == null)
        {
            
        }
        else
        {        
            BaseCard toBeAdded = Instantiate(template, transform);
            toBeAdded.handRef = this;
            toBeAdded.boardRef = board;
            toBeAdded.turnHandler = turnHandler;
            Debug.Log("Card added: " + toBeAdded.name);
            cardsInHand.Add(toBeAdded);
        }

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
        if (cardsInHand.Count < maxCards) // TODO: change to limit pickups
        {
            Debug.Log("Getting cards");
            for (int i = cardsInHand.Count; i < maxCards; i++)
            {
                GetCardToHand();
            }
            CardPositions();
        }
    }
    

    public void UpdateFunds(int deltaFunds)
    {
        playerFunds += deltaFunds;
        playerCharacter.deployPoints = playerFunds;
        playerCharacter.UpdateTextFields();
    }
    public void ResetFunds()
    {
        playerFunds = baseFunds;
        playerCharacter.deployPoints = playerFunds;
        playerCharacter.UpdateTextFields();
    }

    public void SwapHand() // done by active player before turn swaps
    {
        otherHand.friendly = true;
        (cardsInHand, otherHand.cardsInHand) = (otherHand.cardsInHand, cardsInHand); // no Idea how this works
        otherHand.CardPositions();
        friendly = false;
        CardPositions();
        
    }
}
