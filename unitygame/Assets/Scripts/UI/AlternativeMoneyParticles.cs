//using MyBox;
using UnityEngine;

public class AlternativeMoneyParticles : MonoBehaviour
{
    public bool AddSymbol = true;
    //[ConditionalField("AddSymbol")]
    public string addedSymbol = " $";

    [SerializeField]
    GameObject add, sub;
    [SerializeField]
    float LifeTime = 1F;
    private void Start()
    {
        //addMoney(100);
        //subtractMoney(100);

    }
    float currentMoney;

    public void SetMoney(float money)
    {
        if (currentMoney!= money)
        {
            if (currentMoney > money)
            {
                subtractMoney((int)(currentMoney - money));
            }
            else
            {
                addMoney((int)(money - currentMoney));
            }
            currentMoney = money;
        }
    }
    public void addMoney(int amount)
    {
        GameObject tempGO = Instantiate(add, transform);
        ColorOverTime colorOverTime = tempGO.GetComponent<ColorOverTime>();
        colorOverTime.text.text = "+ " + amount + addedSymbol;
        colorOverTime.LifeTime = LifeTime;
    }
    public void subtractMoney(int amount)
    {
        GameObject tempGO = Instantiate(sub, transform);
        ColorOverTime colorOverTime = tempGO.GetComponent<ColorOverTime>();
        colorOverTime.text.text = "- " + amount + addedSymbol;
        colorOverTime.LifeTime = LifeTime;
    }
}

