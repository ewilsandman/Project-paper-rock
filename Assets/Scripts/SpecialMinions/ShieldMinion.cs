public class ShieldMinion : Minion
{
    //public Text descriptionTag;
    // Start is called before the first frame update
    protected override void Start()
    {
        UpdateTextFields();
        ResetAttack();
    }
    

    public override void UpdateTextFields()
    {
        healthText.text = healthPool.ReturnHealth().ToString();
        strengthText.text = strength.ToString();
        describeText.text = "Shielding";
        nameText.text = minionName; // unlikely to change
        ChangeColour(false);
    }
}
