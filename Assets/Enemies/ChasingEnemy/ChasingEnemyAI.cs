using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Animator))]

public class ChasingEnemyAI : Enemy
{
    private Transform target;
    
    [HideInInspector]
    public ChasingEnemyAI_Stats stats;
    
    void Start()
    {
        health = stats.health;
        speed = stats.speed;
        damageToPlayerOnCollision = stats.damageToPlayerOnCollision;

        rigidbody.mass = stats.rigidbodyMass;

        // Cache agent component and destination
        target = GameObject.FindGameObjectWithTag("Player").transform;
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
//we'll leave this as a stub and to keep things strict, might fill it out later
public class ChasingEnemyAI_Stats : Enemy_Stats 
{

}



