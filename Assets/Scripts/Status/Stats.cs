using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Stats
{
    [SerializeField]
    private int baseValue;
    [SerializeField]
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

    public void AddModifiers(int _modifiers)
    {
        modifiers.Add(_modifiers);
    }

    public void RemoveModifiers(int _modifiers)
    {
        modifiers.Remove(_modifiers);
    }

    public void SetDefaultValue(int value)
    {
        baseValue = value;
    }
}
