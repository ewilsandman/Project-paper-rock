using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Hand : MonoBehaviour
{
    public int maxCards = 7;

    private Vector3 Originpos;
    [SerializeField] private Vector3 OfsetPos;
    [SerializeField] private Board board;
    [SerializeField] private List<BaseCard> cardsInHand;
    [SerializeField] private CoreLoop TurnHandler;
    public Pile pile;
    
    // Start is called before the first frame update
    void Start()
    {
        cardsInHand = new List<BaseCard>();
        Originpos = transform.position;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    void CardPositions()
    {
        
        //Debug.Log("Start loop");
        for (int i = 0; i < maxCards; i++)
        {
            Debug.Log("uh");
            cardsInHand[i].gameObject.transform.localPosition = Originpos + OfsetPos * i;
            //CurrentHand[i].gameObject.GetComponent<RectTransform>().pivot = Originpos + OfsetPos;
            float x = Originpos.x + (OfsetPos.x * i);
            cardsInHand[i].NewPos(x, 0);
        }
    }

    private void FixedUpdate()
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
}
