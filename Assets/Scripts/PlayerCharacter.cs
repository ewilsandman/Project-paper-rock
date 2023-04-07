using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public int deployPoints; // never negative
    public int health;
    public bool friendly;
    [SerializeField] private Text deployPointDisplay;
    [SerializeField] private Text healthPointDisplay;
    [SerializeField] private Text pileCardsDisplay; // unused
    [FormerlySerializedAs("_pile")] [SerializeField] private Pile pile;
    [FormerlySerializedAs("_board")] [SerializeField] private Board board;
    [SerializeField]private Text _killText;
    public Hand hand;

    private bool _highlighted;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTextFields();
    }

    public void ButtonResponse()
    {
        board.AddTarget(gameObject);
        //ChangeColour(false);
    }
    
    public void ChangeColour(bool input)
    {
        _highlighted = input;
        HighlightToggle();
    }

    protected void HighlightToggle()
    {
        if (_highlighted)
        {
            gameObject.GetComponent<Image>().color = Color.green;
        }
        else
        {
            gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    public void DeltaHealth(int delta)
    {
        if (health + delta <= 0)
        {
            Debug.Log("ono I am kill");
           // _killText.enabled = true;
            //_killText.text = gameObject.name + " Has died, other player wins ";
            board.stopGame(this);
            // die and mark other player victory
        }
        else
        {
            health += delta;
        }
        UpdateTextFields();
    }

    public void SwapSides(PlayerCharacter otherChar)
    {
        (transform.position, otherChar.transform.position) = 
            (otherChar.transform.position, transform.position);
        friendly = false;
        otherChar.friendly = true;
    }

    public void UpdateTextFields()
    {
        deployPointDisplay.text = "Funds: " + deployPoints;
        healthPointDisplay.text = "Health: " + health;
        pileCardsDisplay.text = "Cards: " + pile.cardsLeft();
    }
}
