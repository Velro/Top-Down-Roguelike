using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class CaveWormBoss : Enemy
{
    private Transform target;

    [SerializeField]
    private CaveWormBoss_Stats caveWormBossStats;
    private Animator animator;

    private bool deathAnimationTriggered;

    private void Start()
    {
        // Cache agent component and destination
        target = GameObject.FindGameObjectWithTag("Player").transform;
        PopulateStats(caveWormBossStats);

        animator = GetComponent<Animator>();
    }

    protected void PopulateStats(CaveWormBoss_Stats stats)
    {
        base.PopulateStats(caveWormBossStats);
    }


    protected void Update()
    {
        //base.Update();
        if (health > 0)
        {
            Vector3 moveTowardsAmount = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, target.transform.position) < 2)
            {
                transform.LookAt(target);
                animator.SetTrigger("Attack");
            }
            else
            {
                transform.LookAt(target);
            
                rigidbody.MovePosition(moveTowardsAmount);
            
            }
            animator.SetFloat("Speed", moveTowardsAmount.magnitude);
        }
        else if (!deathAnimationTriggered)
        {
            animator.SetTrigger("Dead");
            deathAnimationTriggered = true;
        }
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Dead") && info.normalizedTime > 0.95f)
        {
            UIManager.Instance.DisableBossHealthBar();
            Die();
        }

 
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Stats/CaveWormBoss_Stats")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<CaveWormBoss_Stats>();
    }
#endif
}
