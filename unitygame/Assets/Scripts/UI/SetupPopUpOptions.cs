using TMPro;
using UnityEngine;

public class SetupPopUpOptions : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI[] optionsDescriptions;

    public void Setup(string[] optionText)
    {
        for (int i = 0; i < optionsDescriptions.Length; i++)
        {
            optionsDescriptions[i].text = optionText[i];
        }
    }
}
