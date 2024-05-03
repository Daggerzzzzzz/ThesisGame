using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Stats
{
    [SerializeField]
    private int baseValue;
    [HideInInspector]
    public List<int> modifiers;
    
    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        
        return finalValue;   
    }

    public void AddModifiers(int modifiers)
    {
        this.modifiers.Add(modifiers);
    }

    public void RemoveModifiers(int modifiers)
    {
        this.modifiers.RemoveAt(modifiers);
    }

    public void SetDefaultValue(int value)
    {
        baseValue = value;
    }
}
