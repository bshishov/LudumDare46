using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof (TextMeshProUGUI))]
public class ColorOverTime : MonoBehaviour
{
    public Gradient gradient;
    public float LifeTime = 0.5F,howFarDown = 200;
    [SerializeField] float currentTime = 0F;
    [SerializeField]  RectTransform rectTransform;
    [SerializeField]
    public TextMeshProUGUI text;
    float normalizedValue;
    void Start()
    {
        rectTransform.GetComponent<RectTransform>();
        text.GetComponent<TextMeshProUGUI>();
        currentTime = 0F;
        StartCoroutine(LerpObject());
    }
 
    IEnumerator LerpObject()
    {
        Vector3 startPosition = rectTransform.anchoredPosition;
        Vector3 endPosition = startPosition - new Vector3(0, howFarDown,0);
        while (currentTime <= LifeTime)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / LifeTime;  

            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, normalizedValue);
            text.color = gradient.Evaluate(normalizedValue);
            yield return null;
        }
        Destroy(gameObject);
    }
}
