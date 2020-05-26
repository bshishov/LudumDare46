using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetTechnologySlider : MonoBehaviour
{
    public string addedSymbol = "%";

    [SerializeField]
    TextMeshProUGUI MarketingText, TechnologyText, RelevanceText;
    [SerializeField]
    Image MarketingImage, RelevanceImage;
    RectTransform MarketingRectTransform, TechnologyRectTransform, RelevanceRectTransform, thisRectTransform;

    float oldMarketingFillAmount, oldRelevanceFillAmount;

    [SerializeField]
    float LerpTime = 1F;
    float currentTime;
    //[SerializeField]
    float total;
    //[SerializeField]
    float Marketing, Technology, Relevance;
    //[SerializeField]
    float oldMarketing, oldTechnology, oldRelevance;

    bool Updated = false;

    private void OnEnable()
    {
        thisRectTransform = GetComponent<RectTransform>();
        setSliders(20F,30F,50F);
    }
    private void Start()
    {
        MarketingRectTransform = MarketingText.GetComponent<RectTransform>();
        TechnologyRectTransform = TechnologyText.GetComponent<RectTransform>();
        RelevanceRectTransform = RelevanceText.GetComponent<RectTransform>();
    }
    public void setSliders(float m_Marketing, float m_Technology, float m_Relevance)
    {
        
        Marketing = m_Marketing;
        Technology = m_Technology;
        Relevance = m_Relevance;
        total = Marketing + Technology + Relevance;
        currentTime = 0;

        Updated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime <= LerpTime)
        {
            float FillRatio = currentTime / LerpTime;
            MarketingText.text = "" + (int)Mathf.Lerp(oldMarketing, Marketing, FillRatio) + addedSymbol;
            MarketingImage.fillAmount = Mathf.Lerp(oldMarketingFillAmount, Marketing / total, FillRatio);

            TechnologyText.text = "" + (int)Mathf.Lerp(oldTechnology, Technology, FillRatio) + addedSymbol;

            RelevanceText.text = "" + (int)Mathf.Lerp(oldRelevance, Relevance, FillRatio) + addedSymbol;
            RelevanceImage.fillAmount = Mathf.Lerp(oldRelevanceFillAmount, Relevance / total, FillRatio);
            currentTime += Time.deltaTime;

            TechnologyRectTransform.anchoredPosition = new Vector2(TechnologyRectTransform.anchoredPosition.x, -thisRectTransform.rect.height * MarketingImage.fillAmount);
            RelevanceRectTransform.anchoredPosition = new Vector2(TechnologyRectTransform.anchoredPosition.x, -thisRectTransform.rect.height * (1F - RelevanceImage.fillAmount));
        }
        else if(!Updated)
        {
            MarketingText.text = "" + Marketing + addedSymbol;
            MarketingImage.fillAmount = Marketing / total;
            oldMarketingFillAmount = Marketing / total;
            oldMarketing = Marketing;

            TechnologyText.text = "" + Technology + addedSymbol;
            oldTechnology = Technology;

            RelevanceText.text = "" + Relevance + addedSymbol;
            RelevanceImage.fillAmount = Relevance / total;
            oldRelevanceFillAmount = Relevance / total;
            oldRelevance = Relevance;

            Updated = true;
        }
        
    }
}
