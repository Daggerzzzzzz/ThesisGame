using UnityEngine;

public enum EquipmentType
{
    ARMOR,
    WEAPON
}
public class EquipmentDataSO : ItemDataSO
{
    public EquipmentType equipmentType;
    public Vector2 position;

    protected int minDescLength;
    protected void AddItemDescription(int value, string name)
    {
        if (value == 0)
        {
            return;
        }
        
        if (sb.Length > 0)
        {
            sb.AppendLine();
        }

        if (value > 0)
        {
            sb.Append("+ " + value + " " + name);
        }

        minDescLength++;
    }
}
