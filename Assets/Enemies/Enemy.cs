//Not to actually be instantiated

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour 
{

    [HideInInspector]
    public float health;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float damageToPlayerOnCollision;

    new private Renderer renderer;
    
    protected Rigidbody rigidbody;

    private bool calledDie;

    private void Awake ()
    {
        renderer = GetComponentInChildren<Renderer>();
        rigidbody = GetComponent<Rigidbody>();
    }

    /*private void Start ()
    {
        PopulateStats(enemyStats);
    }*/

    protected void PopulateStats(Enemy_Stats stats)
    {
        health = stats.health;
        speed = stats.speed;
        damageToPlayerOnCollision = stats.damageToPlayerOnCollision;
    }

    protected void Update()
    {
        if (health <= 0)
        {
            Die();
        }

        if (health < 0)
        {
            health = 0;
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
        StartCoroutine(Flash());

    }

    public void Die()
    {
        if (!calledDie)
        {
            Invoke("StartSinking", 1);
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider c in colliders)
            {
                c.enabled = false;
            }
            calledDie = true;
        }
    }

    public void Pierced ()
    {
        gameObject.layer = LayerMask.NameToLayer("EnemyNoBulletCollision");
        Invoke("BackToEnemyLayer", 0.25f);
    }

    private void BackToEnemyLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
    }

    private void StartSinking()
    {
        StartCoroutine(Sink());
    }

    private IEnumerator Sink()
    {
        for (; ; )
        {
            transform.position -= Vector3.up * (Time.deltaTime/6);
            if (transform.position.y < -3)
            {
                Destroy(gameObject);
            }
            yield return 0;
        }
    }

    private IEnumerator Flash()
    {
        Color originalColor = renderer.material.GetColor("_Color");
        renderer.material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(0.1f);
        renderer.material.SetColor("_Color", originalColor);
    }

    void OnCollisionEnter (Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("Damage", damageToPlayerOnCollision);
        }
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Stats/EnemyAI_Stats")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<Enemy_Stats>();
    }
#endif


}

public class Enemy_Stats : ScriptableObject
{
    public float health;
    public float speed;
    public float rigidbodyMass;
    public float damageToPlayerOnCollision;
}
