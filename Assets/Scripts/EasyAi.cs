using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EasyAi : MonoBehaviour // might rename to just AI and use enum for parameters
{
     private Board boardReference;
     private Hand handReference;
     private PlayerCharacter friendlyPlayerChar; // unused for now
     private PlayerCharacter hostilePlayerChar;
     private CoreLoop loopReference;

    private List<BaseCard> _currentPlayableCards;
    private int _currentFunds;

    private Minion[] _hostileMinions; // acts as bool depending on empty
    private Minion[] _friendlyMinions;

    public void Setup(Board board, Hand hand, PlayerCharacter friendly, PlayerCharacter hostile, CoreLoop loop)
    {
        Debug.Log("AI "+ gameObject.name +" setting up");
        boardReference = board;
        handReference = hand;
        friendlyPlayerChar = friendly;
        hostilePlayerChar = hostile;
        loopReference = loop;
    }
    
    public void TurnStart()
    {
        Debug.Log("AI starting turn");
        GetCardConditions(); // oh well
        while (_currentPlayableCards.Count > 0) // possible infinite loop
        {
            Debug.Log("in card loop");
            GetCardConditions();
            if (_currentPlayableCards.Count > 0)
            {
                PlayCard();
            }
        }
        GetBoardConditions();
        MakeAttacks();
        loopReference.EndTurn();
    }
    
    private void PlayCard()
    {
        if (boardReference.PlaceForMinion(true))
        {
            Debug.Log("found position, playing card");

            _currentFunds -= _currentPlayableCards[0].cost;
            _currentPlayableCards[0].OnMouseDown();
        }
        else
        {
            Debug.Log("Ai found no position");
            _currentPlayableCards.Clear();
        }
    }

    private void MakeAttacks()
    {
        Debug.Log("Committing attack");
        while (_hostileMinions.Length > 0) 
        {
            if (_friendlyMinions.Length > 0)
            {
                Debug.Log("Attacking enemy minion: " + _hostileMinions[0].name);
                _friendlyMinions[0].Attack(_hostileMinions[0].gameObject);
                GetBoardConditions(); // very ineffective
            }
            else
            {
                _hostileMinions = Array.Empty<Minion>();
            }
        }
        //Debug.Log(_friendlyMinions[0]);
        
        foreach (Minion minion in _friendlyMinions) // should break if invalid?
        {
            Debug.Log("No enemy minions left, attacking head");
            minion.Attack(hostilePlayerChar.gameObject);
        }
    }

    private void GetCardConditions() // condition in hand
    {
        Debug.Log("Fetching Card conditions");
        List<BaseCard> allCards = handReference.cardsInHand; // I hate the antichrist (VAR)
        List<BaseCard> sortedCards = new List<BaseCard>(); // could sort by cost?
        foreach (BaseCard card in allCards)
        {
            if (card.CheckCost(handReference.playerFunds))
            {
                sortedCards.Add(card);
            }
        }
        _currentPlayableCards = sortedCards;
        _currentFunds = handReference.playerFunds;
    }

    private void GetBoardConditions() // get after cards are placed,
                                      // does not need to be handled by Interface
    {
        Debug.Log("Fetching board conditions");
        Minion[] possibleMinions = boardReference.playerMinions;
        
        List<Minion> sortedMinions = new List<Minion>();
        if (possibleMinions.Length > 0)
        {
            foreach (Minion minion in possibleMinions)
            {
                Debug.Log("for each minion");
                if (minion != null) 
                {
                    Debug.Log("null-check passed");
                    if (!minion.attackedThisTurn)
                    {
                        sortedMinions.Add(minion);
                    }
                }
            }
        }
        _hostileMinions = possibleMinions.Length > 0 ? boardReference.hostileMinions : Array.Empty<Minion>(); // very cool but dumb
        _friendlyMinions = sortedMinions.ToArray(); // always null :(
    }
}
