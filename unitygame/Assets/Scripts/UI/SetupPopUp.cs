using TMPro;
using UnityEngine;

public class SetupPopUp : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Title, Description;

    public void Setup(string title, string description)
    {
        Title.text = title;
        Description.text = description;
    }
    public void Setup(string title, string description, string[] optionText)
    {
        Title.text = title;
        Description.text = description;
        GetComponent<SetupPopUpOptions>().Setup(optionText);
    }
}
