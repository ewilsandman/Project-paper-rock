using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public int maxCards = 7;

    private Vector3 Originpos;
    [SerializeField] private Vector3 OfsetPos;
    [SerializeField] private List<BaseCard> CurrentHand;
    public Pile pile;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHand = new List<BaseCard>();
        Originpos = transform.position;
    }

    void GetCardToHand()
    {
        BaseCard template = pile.PileToHand();
        BaseCard toBeAdded = Instantiate(template, transform);
        Debug.Log("Card added: " + toBeAdded.name);
        CurrentHand.Add(toBeAdded);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CardPositions()
    {
        
        Debug.Log("Start loop");
        for (int i = 0; i < maxCards; i++)
        {
            Debug.Log("uh");
            CurrentHand[i].gameObject.transform.localPosition = Originpos + OfsetPos * i;
            //CurrentHand[i].gameObject.GetComponent<RectTransform>().pivot = Originpos + OfsetPos;
            float x = Originpos.x + (OfsetPos.x * i);
            CurrentHand[i].NewPos(x, 0);
        }
    }

    private void FixedUpdate()
    {
        if (CurrentHand.Count < maxCards)
        {
            Debug.Log("Getting cards");
            for (int i = CurrentHand.Count; i < maxCards; i++)
            {
                GetCardToHand();
            }
            CardPositions();
        }
    }
}
