using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Shooting))]
[RequireComponent(typeof(Rigidbody))]
public class WanderingShootingEnemy : Enemy 
{
    [SerializeField]private WanderingShootingEnemy_Stats wanderingShootingEnemyStats;
    Shooting shooting;
    
    private Vector3 perlinNoise;

    private Vector2 origin1;
    private Vector2 origin2;

    private Animator animator;

	// Use this for initialization
	void Start () 
    {
        //stats = Resources.Load("Enemy Stats/WanderingShootingEnemyAI_Stats") as WanderingShootingEnemyAI_Stats;
        shooting = GetComponent<Shooting>();
        animator = GetComponent<Animator>();
        if (!shooting)
            shooting = gameObject.AddComponent<Shooting>();

        origin1.x = Random.Range(-10, 10);
        origin2.y = Random.Range(-10, 10);
        origin1.x = Random.Range(-10, 10);
        origin2.y = Random.Range(-10, 10);

        timeToNextShot = Random.Range(wanderingShootingEnemyStats.minTimeBetweenShots, wanderingShootingEnemyStats.maxTimeBetweenShots);
        PopulateStats(wanderingShootingEnemyStats);
	}

    protected void PopulateStats(WanderingShootingEnemy_Stats wanderingShootingEnemyStats)
    {
        base.PopulateStats(wanderingShootingEnemyStats);
    }

    private float lastShotTime;
    private float timeToNextShot;

	// Update is called once per frame
	protected void Update () 
    {
        base.Update();
        perlinNoise = new Vector3(
            Mathf.Lerp(-1, 1, Mathf.PerlinNoise((Time.time + origin1.x) * wanderingShootingEnemyStats.sampleScale, (Time.time + origin1.y) * wanderingShootingEnemyStats.sampleScale)),
            0,
            Mathf.Lerp(-1, 1, Mathf.PerlinNoise((Time.time + origin2.x) * wanderingShootingEnemyStats.sampleScale, (Time.time + origin2.y) * wanderingShootingEnemyStats.sampleScale)))
            * speed * Time.deltaTime;

        transform.LookAt(rigidbody.position + perlinNoise);
        rigidbody.MovePosition(rigidbody.position + perlinNoise);//move to XZ space
        Debug.Log(perlinNoise.magnitude);

        //shooting
        if (Time.time > lastShotTime + timeToNextShot)
        {
            Vector3 randomShootDirection = new Vector3(Mathf.Lerp(-1, 1,Random.value), 
                                                       0, 
                                                       Mathf.Lerp(-1, 1,Random.value));
            shooting.SpawnBullet(wanderingShootingEnemyStats.bullet, randomShootDirection);
            lastShotTime = Time.time;
            timeToNextShot = Random.Range(wanderingShootingEnemyStats.minTimeBetweenShots, wanderingShootingEnemyStats.maxTimeBetweenShots);
        }

	}


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Stats/WanderingShootingEnemy_Stats")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<WanderingShootingEnemy_Stats>();
    }
#endif

}
