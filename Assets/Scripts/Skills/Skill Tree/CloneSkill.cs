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

    public void CreateClone(Vector2 _clonePosition, Vector2 dashDirection, Vector2 hitBoxDirection)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetUpClone(_clonePosition, cloneDuration, canAttack, dashDirection, hitBoxDirection);
    }
}
