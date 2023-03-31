using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInterface : MonoBehaviour // unused, might return
{
    [SerializeField] private Hand hand;
    [SerializeField] private Board boardReference;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public List<BaseCard> GetAvailableCards() // to be called on start and after every play,
                                              // could be done via a dictionary
    {
        List<BaseCard> allCards = hand.cardsInHand; // I hate the antichrist (VAR)
        List<BaseCard> sortedCards = new List<BaseCard>(); // could sort by cost?
        foreach (BaseCard card in allCards)
        {
            if (card.CheckCost(hand.playerFunds))
            {
                sortedCards.Add(card);
            }
        }
        return sortedCards;
    }

    public void DeployCard(BaseCard toDeploy)
    {
        toDeploy.OnMouseDown();
    }
    

    public int getFunds()
    {
        return hand.playerFunds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
