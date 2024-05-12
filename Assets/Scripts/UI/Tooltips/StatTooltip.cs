using TMPro;
using UnityEngine;

public class StatTooltip : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI statDesc;
    [HideInInspector]
    public RectTransform statRectTransform;
    
    private void Awake()
    {
        statRectTransform = GetComponent<RectTransform>();
    }

    public void ShowStatTooltip(string text)
    {
        statDesc.text = text;
        
        gameObject.SetActive(true);
    }
    
    public void HideStatTooltip()
    {
        statDesc.text = "";
        
        gameObject.SetActive(false);
    }
}
