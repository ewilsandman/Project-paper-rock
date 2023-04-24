using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private Board _boardReference;

    private Hand _handReference;

    private PlayerCharacter
        _friendlyPlayerChar; // unused for now, useful to have the AI check if it can be killed next round

    private PlayerCharacter _hostilePlayerChar;
    //private CoreLoop loopReference;

    private List<BaseCard> _currentPlayableCards;

    private List<BaseCard> _currentSpellCards;
    //private int _currentFunds;

    private List<Minion> _hostileMinions = new List<Minion>(); // acts as bool depending on empty
    private List<Minion> _friendlyMinions = new List<Minion>();
    private int _inComingDamage;
    private int _outGoingDamage;

    public void Setup(Board board, Hand hand, PlayerCharacter friendly, PlayerCharacter hostile)
    {
        Debug.Log("AI " + gameObject.name + " setting up");
        _boardReference = board;
        _handReference = hand;
        _friendlyPlayerChar = friendly;
        _hostilePlayerChar = hostile;
    }

    public void TurnStart()
    {
        StartCoroutine(PerformTurn());
    }

    private IEnumerator PerformTurn() // recursive??
    {
        Debug.Log("Ai performing turn");
        GetCardConditions(); // oh well
        CountOutgoingDamage();
        CountInDamage();
        yield return new WaitForSeconds(1f);
        if (_outGoingDamage >= _hostilePlayerChar.healthPool.ReturnHealth()) // can probably kill, do so immediately
        {
           MakeKillingBlow();
        }
        else if (_inComingDamage >= _friendlyPlayerChar.healthPool.ReturnHealth())
        {
            MakeHeal();
        }
        
        
        while (_currentPlayableCards.Count > 0) // possible infinite loop
        {
            GetCardConditions();
            if (_currentPlayableCards.Count > 0)
            {
                yield return new WaitForSeconds(0.5f);
                PlayCard(_currentPlayableCards[0]);
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(.2f);
        }

        GetBoardConditions();
        yield return new WaitForSeconds(1f);
        MakeAttacks();
        _boardReference.EndTurn();
        StopCoroutine(PerformTurn());
    }

    private void PlayCard(BaseCard toPlay)
    {
        if (_boardReference.PlaceForMinion(false))
        {
            Debug.Log("found position, playing card");

            toPlay.OnMouseDown();
        }
        else
        {
            Debug.Log("Ai found no position");
            _currentPlayableCards.Clear();
        }
    }

    private void CountOutgoingDamage()
    {
        int outgoing = 0;
        if (_friendlyMinions.Count > 0)
        {
            foreach (Minion fm in _friendlyMinions)
            {
                outgoing += fm.strength;
            }
        }

        _outGoingDamage = outgoing;
    }

    private void CountInDamage()
    {
        int incoming = 0;
        if (_hostileMinions.Count > 0)
        {
            foreach (Minion hm in _hostileMinions)
            {
                incoming += hm.strength;
            }
        }

        _inComingDamage = incoming;
    }

    private void MakeKillingBlow()
    {
        bool enemyShielded = false;
        foreach (Minion hostileMinion in _hostileMinions)
        {
            if (hostileMinion.TryGetComponent(out ShieldMinion shield))
            {
                Debug.Log("Ai has detected enemy shield");
                enemyShielded = true;
                break;
            }
        }

        if (enemyShielded) // if enemy has shield card, check if you can kill with spells
        {
            int potentialBudget = _handReference.playerFunds;
            List<BaseCard> spellsToCast = new List<BaseCard>();
            foreach (BaseCard card in _currentSpellCards)
            {
                if (card.strength > 0)
                {
                    if (card.cost <= potentialBudget)
                    {
                        spellsToCast.Add(card);
                        potentialBudget -= card.cost;
                    }
                }
            }

            if (spellsToCast.Count > 0)
            {
                int totalDamage = 0;
                foreach (BaseCard card in spellsToCast)
                {
                    totalDamage += card.strength;
                }

                if (totalDamage >= _hostilePlayerChar.healthPool.ReturnHealth()) // if can kill with spells
                {
                    Debug.Log("Fireball!");
                    foreach (BaseCard spell in spellsToCast)
                    {
                        AISpell(spell, _hostilePlayerChar.healthPool);
                    }
                }
            }
        }
    }

    private void MakeHeal()
    {
        foreach (BaseCard spellCard in _currentSpellCards)
        {
            if (spellCard.health + _friendlyPlayerChar.healthPool.ReturnHealth() > _inComingDamage) // if the healing card will help, play it
            {
                AISpell(spellCard, _friendlyPlayerChar.healthPool);
                break;
            }
        }
    }

    private void MakeAttacks()
    {
        GetBoardConditions();
        CountOutgoingDamage();



        foreach (BaseCard minionCard in _currentPlayableCards) // otherwise, play defensive cards
        {
            if (minionCard.TryGetComponent(out ShieldMinion minion))
            {
                PlayCard(minionCard);
            }
        }


        foreach (Minion HM in _hostileMinions)
        {


        // (_hostileMinions.Count > 0) // while scary, TODO: fix/remove loops, ForEach instead of while
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
            AIAttack(minion.gameObject, _hostilePlayerChar.gameObject);
        }
        
    }

    private void AISpell(BaseCard healingCard,Targetable target)
    {
        healingCard.OnMouseDown();
        _boardReference.AddTarget(target);
    }

    private void AIAttack(GameObject attacker, GameObject target)
    {
        attacker.GetComponent<Minion>().ButtonResponse(); // assuming attacker is always minion or inherits from minion
        _boardReference.AddTarget(target.GetComponent<Targetable>());
    }

    private void GetCardConditions() // condition in hand
    {
        Debug.Log("Fetching Card conditions");
        List<BaseCard> allCards = _handReference.cardsInHand;
        List<BaseCard> sortedCards = new List<BaseCard>(); // could sort by cost?
        List<BaseCard> sortedSpellCards = new List<BaseCard>();
        foreach (BaseCard card in allCards)
        {
            if (_handReference.CheckCost(card))
            {
                if (!card.minionSpawning)
                {
                    sortedSpellCards.Add(card);
                }
                else
                {
                    sortedCards.Add(card);
                }
            }
            
        }
        _currentSpellCards = sortedSpellCards;
        _currentPlayableCards = sortedCards;
    }

    private void GetBoardConditions() //TODO: effectivize
    {
        Debug.Log("Fetching board conditions");
        List<Minion> possibleMinions = new List<Minion>(_boardReference.activePlayerMinions);
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
                if (minion != null && !minion.CheckAttack()) 
                {
                    sortedMinions.Add(minion);
                }
            }
        }
        _hostileMinions = new List<Minion>(_boardReference.passivePlayerMinions); 
        _friendlyMinions = sortedMinions;
    }
}
