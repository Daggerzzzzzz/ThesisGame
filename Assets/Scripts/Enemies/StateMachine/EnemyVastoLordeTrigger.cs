public class EnemyVastoLordeTrigger : EnemyTrigger
{
    private EnemyVastoLorde enemyVastoLorde => GetComponentInParent<EnemyVastoLorde>();

    private void Relocate()
    {
        enemyVastoLorde.FindPosition();
    }

    private void MakeInvisible()
    {
        enemyVastoLorde.OnEntityFx.MakeTransparent(true);
    }
    
    private void MakeVisible()
    {
        enemyVastoLorde.OnEntityFx.MakeTransparent(false);
    }
}
