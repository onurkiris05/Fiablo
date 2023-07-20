using RPG.BehaviourTree;
using RPG.Control;

public class EnemyController : ControllerBase
{
    private EnemyPatrolBT _enemyPatrolBt;

    protected override void Awake()
    {
        base.Awake();

        _enemyPatrolBt = GetComponent<EnemyPatrolBT>();
        _enemyPatrolBt.Init(this);
    }
    
}