using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour
{
    public ItemTooltip itemTooltip;

    private void Start()
    {
        //itemTooltip = GetComponentInChildren<ItemTooltip>();
    }

    public void SwitchMenus(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
        {
            menu.SetActive(true);
        }
    }
}
