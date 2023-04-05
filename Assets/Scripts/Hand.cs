using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Hand : MonoBehaviour //should be called player but renaming is scary
{
    public int maxCards;

    [SerializeField] private Board board;
    public PlayerCharacter playerCharacter;
    [SerializeField] private PlayerCharacter otherCharacter; // AI reasons
    
   // [FormerlySerializedAs("TurnHandler")] [SerializeField] private CoreLoop turnHandler;
    [FormerlySerializedAs("_playerFunds")] public int playerFunds;
    [FormerlySerializedAs("BaseFunds")] [SerializeField] private int baseFunds;
    public Pile pile;
    public List<BaseCard> cardsInHand;
    
    //[SerializeField] private bool singlePlayer = true; // in this case singlePlayer means versus AI
    [SerializeField] private bool aiPlayer = false; // failsafe
    [SerializeField] private EasyAi aiRef;

    [FormerlySerializedAs("Friendly")] public bool friendly;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (aiPlayer)
        {
            aiRef.Setup(board, this, playerCharacter, otherCharacter);
        }
        cardsInHand = new List<BaseCard>();
        DrawCards();
    }

    public void StartTurn() // mostly for AI reasons
    {
        if (aiPlayer)
        {
            aiRef.TurnStart();
        }
    }

    void GetCardToHand()
    {
        BaseCard template = pile.PileToHand();
        if (template == null) // if no more cards exist
        {
            playerCharacter.DeltaHealth(-1);
        }
        else
        {        
            BaseCard toBeAdded = Instantiate(template, transform);
            toBeAdded.handRef = this;
            cardsInHand.Add(toBeAdded);
        }
    }

    public bool CheckCost(BaseCard toCheck) // also used for visuals
    {
        if (toCheck.cost <= playerFunds)
        {
            return true;
        }

        return false;
    }

    public void OnCardClick(BaseCard clicked)
    {
        if (board.activePlayer == this)
        {
            if (CheckCost(clicked))
            {
                if (board.HandleCard(clicked, this))
                {
                    RemoveCard(clicked);
                }
            }   
        }
    }

    private void RemoveCard(BaseCard card)
    {
        cardsInHand.Remove(card);
        Destroy(card.gameObject);
    }

    void CardPositions() // cosmetic, handles position of card in UI
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

    public void HandleMinionClick(Minion clicked)
    {
        if (friendly)
        { 
            board.AddAttacker(clicked.gameObject);
        }
        else
        { 
            board.AddTarget(clicked.gameObject);
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

    public void SwapHand(Hand otherHand) // done by active player before turn swaps
    {
        otherHand.friendly = true;
        (cardsInHand, otherHand.cardsInHand) = (otherHand.cardsInHand, cardsInHand); // no Idea how this works
        otherHand.CardPositions();
        friendly = false;
        CardPositions();
    }

    public void Swap(Hand otherHand, PlayerCharacter otherCharacter) // for "hot seat" game mode
    {
     SwapHand(otherHand);
     playerCharacter.SwapSides(otherCharacter);
    }
}
