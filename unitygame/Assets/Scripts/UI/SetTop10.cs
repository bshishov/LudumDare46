//using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetTop10 : MonoBehaviour
{
    public bool AddSymbol = false;
    //[ConditionalField("AddSymbol")]
    public string symbol = " $";
    [SerializeField]
    TextMeshProUGUI[] m_names, m_money;
    public void Setup(string[] names, int[] money)
    {
        for (int i = 0; i < names.Length; i++)
        {
            m_names[i].text = names[i];
            m_money[i].text = AddSpaces(money[i].ToString()) + symbol;
        }

    }

    string AddSpaces(string str)
    {
        string temp = str;
        if (temp.Length > 3)
        {
            temp = temp.Insert(temp.Length - 3, " ");
        }
        if (temp.Length > 7)
        {
            temp = temp.Insert(temp.Length - 7, " ");
        }
        return temp;
    }
}
