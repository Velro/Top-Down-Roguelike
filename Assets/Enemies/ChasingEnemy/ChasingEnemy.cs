﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class ChasingEnemy : Enemy
{
    private Transform target;

    [SerializeField]
    private ChasingEnemy_Stats chasingEnemyStats;
    private Animator animator;

    private void Start()
    {
        // Cache agent component and destination
        target = GameObject.FindGameObjectWithTag("Player").transform;
        PopulateStats(chasingEnemyStats);

        animator = GetComponent<Animator>();
    }
    
    protected void PopulateStats(ChasingEnemy_Stats stats)
    {
        base.PopulateStats(chasingEnemyStats);
    }
    

    protected void Update()
    {
        base.Update();
        transform.LookAt(target);

        Vector3 moveTowardsAmount = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);

        rigidbody.MovePosition(moveTowardsAmount);

        animator.SetFloat("Forward", moveTowardsAmount.magnitude);
    }
    

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Stats/ChasingEnemy_Stats")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<ChasingEnemy_Stats>();
    }
#endif
}
