using EasyButtons;
using UnityEngine;

public class TestTestSliders : MonoBehaviour
{
    [SerializeField]
    int[] values;
    [SerializeField]
    ISliders setSliders;
    private void OnEnable()
    {
        if (setSliders == null)
        {
            setSliders = GetComponent<ISliders>();
        }
       
        TestNewValues();
    }
    void Start()
    {
        
        
    }
    [Button]
    public void TestNewValues()
    {
        setSliders.setSliders(values);
    }
}
