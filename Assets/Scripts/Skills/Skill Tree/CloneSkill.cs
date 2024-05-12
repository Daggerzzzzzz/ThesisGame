using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")] 
    [SerializeField]
    private GameObject clonePrefab;
    [SerializeField] 
    private float cloneDuration;
    [SerializeField] 
    private bool canAttack;

    public void CreateClone(Vector2 _clonePosition)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetUpClone(_clonePosition, cloneDuration, canAttack);
    }
}
