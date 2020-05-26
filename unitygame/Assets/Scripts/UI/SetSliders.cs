//using MyBox;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetSliders : MonoBehaviour, ISliders
{
    public bool AddSymbol = false;
    //[ConditionalField("AddSymbol")]
    public string addedSymbol = " $";

    [SerializeField]
    Image[] sliders, circles;
    [SerializeField]
    TextMeshProUGUI[] num1, numberOfSlider;
    [SerializeField]
    int[] oldValues;
    int oldtotal = 0;
    [SerializeField]
    Color32 colorOfNormalCircle = new Color32(237, 255, 255, 255), colorOfTopCircle = new Color32(232, 114, 89, 255), colorOfNormalNumber = new Color32(0, 0, 0, 255), colorOfTopNumber = new Color32(255, 255, 255, 255);
    private void OnEnable()
    {
        ResetColors();
        oldValues = new int[num1.Length];
    }

    private void ResetColors()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            circles[i].color = colorOfNormalCircle;
            numberOfSlider[i].color = colorOfNormalNumber;
        }
    }

    public void setSliders(int[] values)
    {
        if (!CheckThatThereAreNoErrors(values))
        {
            return;
        }
        int total = 0;
        int top = 0;
        for (int i = 0; i < values.Length; i++)
        {
            if (values[top] < values[i])
            {
                top = i;
            }
            total += values[i];
        }

        
        for (int i = 0; i < sliders.Length; i++)
        {
            StartCoroutine(lerpSliderWithTime(sliders[i],oldValues[i], values[i],total));

            if (AddSymbol)
            {
                StartCoroutine(lerpTextWithTime(num1[i], oldValues[i], values[i], i, 0.5F, addedSymbol));
            }
            else
            {
                StartCoroutine(lerpTextWithTime(num1[i], oldValues[i], values[i], i, 0.5F));
            }
        }
        
        ResetColors();
        circles[top].color = colorOfTopCircle;
        numberOfSlider[top].color = colorOfTopNumber;
    }


    private bool CheckThatThereAreNoErrors(int[] values)
    {
        if (values.Length != sliders.Length)
        {
            Debug.LogError("values.Length != sliders.Length" + gameObject);
            return false;
        }
        if (values.Length != num1.Length)
        {
            Debug.LogError("values.Length != num.Length" + gameObject);
            return false;
        }
        if (values.Length != numberOfSlider.Length)
        {
            Debug.LogError("values.Length != numberOfSlider.Length" + gameObject);
            return false;
        }
        if (values.Length != circles.Length)
        {
            Debug.LogError("values.Length != circles.Length" + gameObject);
            return false;
        }
        return true;
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


public interface ISliders
{
    void setSliders(int[] values);
}
