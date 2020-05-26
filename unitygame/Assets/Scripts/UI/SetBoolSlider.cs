
//using MyBox;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetBoolSlider : MonoBehaviour, ISliders
{
    public bool AddSymbol = false;
    //[ConditionalField("AddSymbol")]
    public string addedSymbol = " $";

    [SerializeField]
    Image sliders;
    [SerializeField]
    TextMeshProUGUI num1;
    [SerializeField]
    TextMeshProUGUI num2;
    [SerializeField]
    int[] oldValues;
    int oldtotal = 0;
    private void OnEnable()
    {
        oldValues = new int[2];
    }
    public void setSliders(int[] values)
    {
        int total = 0;
        for (int i = 0; i < values.Length; i++)
        {
            total += values[i];
        }
        if (values.Length > 2 )
        {
            Debug.LogError("values.Length > 2" + gameObject);
            return;
        }

        StartCoroutine(lerpSliderWithTime(sliders, oldValues[0], values[0], total));
        if (AddSymbol)
        {
            StartCoroutine(lerpTextWithTime(num1, oldValues[0], values[0], 0, 0.5F, addedSymbol));
            StartCoroutine(lerpTextWithTime(num2, oldValues[1], values[1], 1, 0.5F, addedSymbol));
        }
        else
        {
            StartCoroutine(lerpTextWithTime(num1, oldValues[0], values[0], 0, 0.5F));
            StartCoroutine(lerpTextWithTime(num2, oldValues[1], values[1], 1, 0.5F));
        }
    }

    IEnumerator lerpSliderWithTime(Image image, int oldnum, int newnum, int total, float overTime = 0.5F)
    {
        float elapsedTime = 0;
        int temp = oldnum;
        while (elapsedTime <= overTime)
        {
            temp = (int)Mathf.Lerp(oldnum, newnum, elapsedTime / overTime);

            elapsedTime += Time.deltaTime;
            image.fillAmount = 1F / Mathf.Lerp(oldtotal, total, elapsedTime / overTime) * temp;
            yield return null;
        }
        oldtotal = total;
    }

    IEnumerator lerpTextWithTime(TextMeshProUGUI text, int oldnum, int newnum, int i, float overTime = 0.5F, string addedSymbol = "")
    {
        float elapsedTime = 0;
        int temp = oldnum;
        while (elapsedTime <= overTime)
        {
            //Debug.Log(temp);
            temp = (int)Mathf.Lerp(oldnum, newnum, elapsedTime / overTime);
            elapsedTime += Time.deltaTime;
            text.text = temp.ToString() + addedSymbol;
            yield return null;
        }
        text.text = newnum.ToString() + addedSymbol;
        oldValues[i] = newnum;
        yield return null;
    }
}
