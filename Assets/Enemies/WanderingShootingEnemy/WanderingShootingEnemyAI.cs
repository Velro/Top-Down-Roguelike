using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Shooting))]
public class WanderingShootingEnemyAI : Enemy 
{
    [HideInInspector]
    public WanderingShootingEnemyAI_Stats stats;
    Shooting shooting;
    
    private Vector3 perlinNoise;

    private Vector2 origin1;
    private Vector2 origin2;

	// Use this for initialization
	void Start () 
    {
        //stats = Resources.Load("Enemy Stats/WanderingShootingEnemyAI_Stats") as WanderingShootingEnemyAI_Stats;
        shooting = GetComponent<Shooting>();

        health = stats.health;
        speed = stats.speed;
        damageToPlayerOnCollision = stats.damageToPlayerOnCollision;
        rigidbody.mass = stats.rigidbodyMass;

        origin1.x = Random.Range(-10, 10);
        origin2.y = Random.Range(-10, 10);
        origin1.x = Random.Range(-10, 10);
        origin2.y = Random.Range(-10, 10);

        timeToNextShot = Random.Range(stats.minTimeBetweenShots, stats.maxTimeBetweenShots);
	}

    private float lastShotTime;
    private float timeToNextShot;

	// Update is called once per frame
	void Update () 
    {
          perlinNoise = new Vector3(
              Mathf.Lerp(-1, 1, Mathf.PerlinNoise((Time.time + origin1.x) * stats.sampleScale, (Time.time + origin1.y) * stats.sampleScale)),
              0,
              Mathf.Lerp(-1, 1, Mathf.PerlinNoise((Time.time + origin2.x) * stats.sampleScale, (Time.time + origin2.y) * stats.sampleScale)))
              * speed * Time.deltaTime;

        rigidbody.MovePosition(rigidbody.position + perlinNoise);//move to XZ space

        
        if (health < 0)
        {
            Die();
        }

        //shooting
        if (Time.time > lastShotTime + timeToNextShot)
        {
            Vector3 randomShootDirection = new Vector3(Mathf.Lerp(-1, 1,Random.value), 
                                                       0, 
                                                       Mathf.Lerp(-1, 1,Random.value));
            shooting.SpawnBullet(stats.bullet, randomShootDirection);
            lastShotTime = Time.time;
            timeToNextShot = Random.Range(stats.minTimeBetweenShots, stats.maxTimeBetweenShots);
        }

	}


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Stats/WanderingShootingEnemyAI_Stats")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<WanderingShootingEnemyAI_Stats>();
    }
#endif

}

//DATA CLASS
//we'll leave this as a stub and to keep things strict, might fill it out later
public class WanderingShootingEnemyAI_Stats : Enemy_Stats
{
    public Bullet bullet;
    public float sampleScale;
    public float minTimeBetweenShots;
    public float maxTimeBetweenShots;
}