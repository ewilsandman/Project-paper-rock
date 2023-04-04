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

    // Start is called before the first frame update
    void Start()
    {
        UpdateTextFields();
    }

    public void ButtonResponse()
    {
        if (!friendly)
        {
            board.AddTarget(gameObject);
        }
    }

    public void DeltaHealth(int delta)
    {
        if (health + delta < 0)
        {
            Debug.Log("ono I am kill");
            SceneManager.LoadScene(2);
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
