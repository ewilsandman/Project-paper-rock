using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public int deployPoints; // never negative
    //public int health;
    public bool friendly;
    [SerializeField] private Text deployPointDisplay;
    [SerializeField] private Text healthPointDisplay;
    [SerializeField] private Text pileCardsDisplay; // unused
    [FormerlySerializedAs("_pile")] [SerializeField] private Pile pile;
    [FormerlySerializedAs("_board")] [SerializeField] private Board board;
    [FormerlySerializedAs("_killText")] [SerializeField]private Text killText;

    [SerializeField] private Transform effectPosition;
    public Hand hand;

    private bool _highlighted;

    [SerializeField] private Image image;
    [FormerlySerializedAs("HealthPool")] [SerializeField] public Targetable healthPool;

    // Start is called before the first frame update
    void Start()
    {
        healthPool.Setup();
        UpdateTextFields();
    }

    public void ButtonResponse()
    {
        board.AddTarget(healthPool);
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
            image.color = Color.green;
        }
        else
        {
            image.color = Color.white;
        }
    }

   /* public void DeltaHealth(int delta)
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
    }*/

    public void Kill()
    {
        Debug.Log("ono I am kill");
        board.stopGame(this);
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
        healthPointDisplay.text = "Health: " + healthPool.ReturnHealth();
        pileCardsDisplay.text = "Cards: " + pile.cardsLeft();
    }
}
