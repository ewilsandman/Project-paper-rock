using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile : MonoBehaviour
{
    [SerializeField] private List<BaseCard> possibleCards; // will probably make a system to customize deck later
    private List<BaseCard> _pileCards;
    private int maxPile = 30;
    // Start is called before the first frame update
    void Start()
    {
        _pileCards = new List<BaseCard>();
        for (int i = 0; i < maxPile; i++)
        {
            BaseCard card = possibleCards[Random.Range(0, possibleCards.Count)];
            _pileCards.Add(card);
        }
    }

    public BaseCard PileToHand()
    {
        BaseCard cardToGive = _pileCards[Random.Range(0, possibleCards.Count)];
        _pileCards.Remove(cardToGive);
        return cardToGive;
    }
}
