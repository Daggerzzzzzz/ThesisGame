using TMPro;
using UnityEngine;

public class SkillTooltip : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI skillName;
    [SerializeField] 
    private TextMeshProUGUI skillDesc;
    [HideInInspector]
    public RectTransform skillRectTransform;

    private void Awake()
    {
        skillRectTransform = GetComponent<RectTransform>();
    }

    public void ShowTooltip(string name, string desc)
    {
        skillName.text = name;
        skillDesc.text = desc;
        
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
