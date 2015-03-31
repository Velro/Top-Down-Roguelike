using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Animator))]

public class ChasingEnemyAI : Enemy
{
    Transform target;
    
    public ChasingEnemyAI_Stats stats;
    private float speed;

    private Vector3 destination;

    new Rigidbody rigidbody;

    void Start()
    {
        health = stats.health;
        speed = stats.speed;
        damageToPlayerOnCollision = stats.damageToPlayerOnCollision;

        // Cache agent component and destination
        target = GameObject.FindGameObjectWithTag("Player").transform;


        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rigidbody.MovePosition(Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed));

        if (health < 0)
        {
            Die();
        }
    }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Stats/ChasingEnemyAI_Stats")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<ChasingEnemyAI_Stats>();
    }
#endif
}


//DATA CLASS
public class ChasingEnemyAI_Stats : Enemy_Stats 
{
    public float damageToPlayerOnCollision;
}



