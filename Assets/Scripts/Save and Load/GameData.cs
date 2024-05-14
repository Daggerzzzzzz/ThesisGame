using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int soulCurrency;

    public SerializableDict<string, int> stash;
    public SerializableDict<string, bool> skillTree;
    public SerializableDict<string, int> equipment;
    
    public GameData()
    {
        soulCurrency = 0;
        stash = new SerializableDict<string, int>();
        equipment = new SerializableDict<string, int>();
        skillTree = new SerializableDict<string, bool>();
    }
}
