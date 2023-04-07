using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EasyAi : MonoBehaviour // might rename to just AI and use enum for parameters
{
    
    /// <summary>
    /// AI should:
    /// take into account the cards it has,
    /// the cards it could have,
    /// the board status,
    /// its health
    /// and the enemy's health
    /// </summary>
     private Board boardReference;
     private Hand handReference;
     private PlayerCharacter friendlyPlayerChar; // unused for now, usefull to have the AI check if it can be killed next round
     private PlayerCharacter hostilePlayerChar;
     //private CoreLoop loopReference;

    private List<BaseCard> _currentPlayableCards;
    //private int _currentFunds;

    private List<Minion> _hostileMinions = new List<Minion>(); // acts as bool depending on empty
    private List<Minion> _friendlyMinions = new List<Minion>();

    public void Setup(Board board, Hand hand, PlayerCharacter friendly, PlayerCharacter hostile)
    {
        Debug.Log("AI "+ gameObject.name +" setting up");
        boardReference = board;
        handReference = hand;
        friendlyPlayerChar = friendly;
        hostilePlayerChar = hostile;
    }
    
    public void TurnStart()
    {
        StartCoroutine(PerformTurn());
    }

    private IEnumerator PerformTurn()
    {
        Debug.Log("Ai performing turn");
        GetCardConditions(); // oh well
        while (_currentPlayableCards.Count > 0) // possible infinite loop
        {
            GetCardConditions();
            if (_currentPlayableCards.Count > 0)
            {
                PlayCard();
            }
            yield return new WaitForSeconds(.2f);
        }
        GetBoardConditions();
        yield return new WaitForSeconds(1f);
        MakeAttacks();
        boardReference.EndTurn();
        StopCoroutine(PerformTurn());
    }

    private void PlayCard()
    {
        if (boardReference.PlaceForMinion(false))
        {
            Debug.Log("found position, playing card");
            
            _currentPlayableCards[0].OnMouseDown();
        }
        else
        {
            Debug.Log("Ai found no position");
            _currentPlayableCards.Clear();
        }
    }

    private int countOutgoingDamage()
    {
        int outgoing = 0;
        if (_friendlyMinions.Count > 0)
        {
            foreach (Minion FM in _friendlyMinions)
            {
                outgoing += FM.strength;
            }
        }
        return outgoing;
    }

    private int countIcomingDamage()
    {
        int incoming = 0;
        if (_hostileMinions.Count > 0)
        {
            foreach (Minion HM in _hostileMinions)
            {
                incoming += HM.strength;
            }
        }
        return incoming;
    }

    private void MakeAttacks()
    {
        GetBoardConditions();
        if (countOutgoingDamage() >= hostilePlayerChar.health) // could be toggled to make a less ruthless AI
        {   // "going for the throat"
            Debug.Log("Can kill, will do so");
            foreach (Minion minion in _friendlyMinions) // should break if invalid?
            {
                AIAttack(minion.gameObject, hostilePlayerChar.gameObject);
            }
        }
        /*else if (countIcomingDamage() >= friendlyPlayerChar.health) TODO
        {
            // be more passive, prioritize healing and defensive cards
        }*/
        else 
        {
            Debug.Log("Committing attack");
            while (_hostileMinions.Count > 0) // while scary, TODO: fix/remove loops
            {
                if (_friendlyMinions.Count > 0)
                {
                    if (_hostileMinions.First() != null && _friendlyMinions.First() != null)
                    {
                        Debug.Log("Attacking enemy minion: " + _hostileMinions.First().name);
                        Debug.Log(_friendlyMinions[0]);
                        AIAttack(_friendlyMinions[0].gameObject, _hostileMinions.First().gameObject);
                    }

                    GetBoardConditions(); // very ineffective
                }
                else
                {
                    _hostileMinions.Clear();
                }
            }
            foreach (Minion minion in _friendlyMinions) // should break if invalid?
            {
                Debug.Log("No enemy minions left, attacking head");
                AIAttack(minion.gameObject, hostilePlayerChar.gameObject);
            }
        }
    }

    private void AIAttack(GameObject attacker, GameObject target)
    {
        attacker.GetComponent<Minion>().ButtonResponse(); // assuming attacker is always minion or inherits from minion
        boardReference.AddTarget(target);
    }

    private void GetCardConditions() // condition in hand
    {
        Debug.Log("Fetching Card conditions");
        List<BaseCard> allCards = handReference.cardsInHand;
        List<BaseCard> sortedCards = new List<BaseCard>(); // could sort by cost?
        foreach (BaseCard card in allCards)
        {
            if (handReference.CheckCost(card))
            {
                sortedCards.Add(card);
            }
        }
        _currentPlayableCards = sortedCards;
    }

    private void GetBoardConditions() //TODO: effectivize
    {
        Debug.Log("Fetching board conditions");
        List<Minion> possibleMinions = new List<Minion>(boardReference.activePlayerMinions);
        if (_hostileMinions.Count > 0)
        {
            _hostileMinions.Clear();
        }

        List<Minion> sortedMinions = new List<Minion>();
        sortedMinions.Clear();
        if (possibleMinions.Count > 0)
        {
            foreach (Minion minion in possibleMinions)
            {
                if (minion != null) 
                {
                    if (!minion.CheckAttack())
                    {
                        sortedMinions.Add(minion);
                    }
                }
            }
        }
        _hostileMinions = new List<Minion>(boardReference.passivePlayerMinions); 
        _friendlyMinions = sortedMinions;
    }
}
