using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI itemNameText;
    [SerializeField] 
    private TextMeshProUGUI itemDescription;

    private void Start()
    {
        throw new NotImplementedException();
    }

    public void ShowTooltip(ItemDataSO itemDataSo)
    {
        itemNameText.text = itemDataSo.itemName;
        
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
