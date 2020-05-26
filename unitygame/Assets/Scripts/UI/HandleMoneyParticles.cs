//using MyBox;
using System.Collections;
using TMPro;
using UnityEngine;

public class HandleMoneyParticles : MonoBehaviour
{
    public bool AddSymbol = true;
    //[ConditionalField("AddSymbol")]
    public string addedSymbol = " $";

    [SerializeField]
    ParticleSystem add, subtract;
    [SerializeField]
    TextMeshProUGUI addMoneyText, subMoneyText;

    bool isPlaying;
    public void addMoney(int amount)
    {
        StartCoroutine(waitForParticleSystem("+ ", amount, add, addMoneyText));
    }
    public void subtractMoney(int amount)
    {
        StartCoroutine(waitForParticleSystem("- ", amount, subtract, subMoneyText));
    }
    IEnumerator waitForParticleSystem(string sign, int amount, ParticleSystem particleSystem, TextMeshProUGUI moneyText)
    {
        while (particleSystem.isPlaying)
        {
            yield return null;
        }
        moneyText.text = sign + amount + addedSymbol;
        particleSystem.Play();
    }
}
