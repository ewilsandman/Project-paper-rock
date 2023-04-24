using System.Collections.Generic;
using UnityEngine;

public class Pile : MonoBehaviour
{
    [SerializeField] private List<BaseCard> possibleCards; 
    private List<BaseCard> _pileCards;
    private bool PileCreated = false;
    private int maxPile = 30;
    
    void CreatePile()
    {
        _pileCards = new List<BaseCard>();
        for (int i = 0; i < maxPile; i++)
        {
            BaseCard card = possibleCards[Random.Range(0, possibleCards.Count)];
            _pileCards.Add(card);
        }
        PileCreated = true;
    }

    public int cardsLeft()
    {
        if (PileCreated)
        {
            return _pileCards.Count;
        }
        else
        {
            return maxPile;
        }
 
    }

    public BaseCard PileToHand()
    {
        if (!PileCreated)
        {
            CreatePile();
        }

        if (_pileCards.Count < 1)
        {
            return null;
        }
        else
        {
            BaseCard cardToGive = _pileCards[0];
            _pileCards.Remove(cardToGive);
            return cardToGive;
        }
    }
}
