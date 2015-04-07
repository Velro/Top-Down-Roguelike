using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour 
{

    public float health;
    public float maxHealth;

    public bool isInvincible;

    public float invincibilityDurationAfterDamage;
    private float lastTimeDamageTaken;

    new private Renderer renderer;
    private Color originalColor;

    [HideInInspector]public Bullet bullet;
    private Shooting shooting;
    private float cooldown;
    private float tShot;

    public Player_Stats stats;

    private ThirdPersonCharacter motor;
    private float speedMultiplier;
    
	// Use this for initialization
	void Start () 
    {
        motor = GetComponent<ThirdPersonCharacter>();
        motor.m_AnimSpeedMultiplier = stats.moveSpeedMultiplier;
        motor.m_MoveSpeedMultiplier = stats.moveSpeedMultiplier;
        
        renderer = GetComponentInChildren<Renderer>();
        originalColor = renderer.material.GetColor("_Color");

        shooting = gameObject.AddComponent<Shooting>();
        cooldown = stats.cooldown;


        maxHealth = stats.startingHealth;
        health = stats.startingHealth;
        
        invincibilityDurationAfterDamage = stats.invincibilityDurationAfterDamage;
        bullet = Instantiate(stats.bullet, new Vector3(1000, 1000, 1000), Quaternion.identity) as Bullet;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Debug.Log(Random.seed);
	    if (health <= 0)
        {
            GamePlayManager.Instance.PlayerDied();
        }

        if (Time.time > lastTimeDamageTaken + invincibilityDurationAfterDamage || lastTimeDamageTaken == 0)
        {
            renderer.material.SetColor("_Color", originalColor);
        }
        else
        {
            renderer.material.SetColor("_Color", Color.black);
        }

        Vector3 aimDirection = new Vector3(Input.GetAxisRaw("Aim Horizontal"), 0, Input.GetAxisRaw("Aim Vertical"));
        if (Time.time > tShot + cooldown)
        {
            if (Mathf.Abs(aimDirection.x) > 0.8f)
            {
                Vector3 bulletDirection = new Vector3(aimDirection.x, 0, 0);
                shooting.SpawnBullet(bullet, bulletDirection);
                tShot = Time.time;
            }
            else if (Mathf.Abs(aimDirection.z) > 0.8f)
            {
                Vector3 bulletDirection = new Vector3(0, 0, aimDirection.z);
                shooting.SpawnBullet(bullet, bulletDirection);
                tShot = Time.time;
            }
        }

	}

    public void Damage(float damage)
    {
        if ((Time.time > lastTimeDamageTaken + invincibilityDurationAfterDamage || lastTimeDamageTaken == 0)
            && !isInvincible)
        {
            health -= damage;
            lastTimeDamageTaken = Time.time;
        }
    }

    public void Upgrade(StatUpgrade_Stats upgradeStats)
    {
        maxHealth += upgradeStats.maxHealthChange;
        if (upgradeStats.maxHealthChange > 0)//give player free health when their max health increases
        {
            health += upgradeStats.maxHealthChange;
        }
        else if (upgradeStats.maxHealthChange < 0) //dont let health become greater than max health
        {
            if (health > maxHealth)
                health = maxHealth;
        }

        cooldown += upgradeStats.cooldownChange;
        bullet.GetComponent<Bullet>().damage += upgradeStats.damageChange;
        bullet.transform.localScale += upgradeStats.bulletScale;
        bullet.piercing = upgradeStats.piercing;

        motor.m_AnimSpeedMultiplier += upgradeStats.speedMultiplierChange;
        motor.m_MoveSpeedMultiplier += upgradeStats.speedMultiplierChange;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Stats/Player_Stats")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<Player_Stats>();
    }
#endif

}

//DATA CLASS
//we'll leave this as a stub and to keep things strict, might fill it out later
public class Player_Stats : ScriptableObject
{
    public float startingHealth;

    public Bullet bullet;
    public float cooldown;

    public float invincibilityDurationAfterDamage;

    public float moveSpeedMultiplier;
    
}