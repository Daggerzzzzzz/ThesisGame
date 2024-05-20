public class SkillManager : SingletonMonoBehavior<SkillManager>
{
    public DashSkill Dash { get; private set; }
    public CloneSkill Clone { get; private set; }
    public SwordSkill Sword { get; private set; }
    public BlackholeSkill Blackhole { get; private set; }
    public KunaiSkill Kunai { get; private set; }

    private void Start()
    {
        Dash = GetComponent<DashSkill>();
        Clone = GetComponent<CloneSkill>();
        Sword = GetComponent<SwordSkill>();
        Blackhole = GetComponent<BlackholeSkill>();
        Kunai = GetComponent<KunaiSkill>();
    }
}
