using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int soulCurrency;
    public int statPoints;
    public int level;
    public float currentExp;
    public int strength;
    public int vitality;
    public int intelligence;
    public int agility;
    public int currentHealth;
    public bool haveKey;

    public SerializableDict<string, int> stash;
    public SerializableDict<string, bool> skillTree;
    public SerializableDict<string, int> equipment;
    public SerializableDict<string, float> volumeSettings;
    
    public List<RoomInfo> roomInfoSave;
    
    public GameData()
    {
        soulCurrency = 0;
        
        stash = new SerializableDict<string, int>();
        equipment = new SerializableDict<string, int>();
        skillTree = new SerializableDict<string, bool>();
        volumeSettings = new SerializableDict<string, float>();

        roomInfoSave = new List<RoomInfo>();
        
    }
}
