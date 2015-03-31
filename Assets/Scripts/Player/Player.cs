using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour {

    public float health;
    [HideInInspector]public float maxHealth;

    public float invincibilityDurationAfterDamage;
    private float lastTimeDamageTaken;

    new private Renderer renderer;
    private Color originalColor;

	// Use this for initialization
	void Start () {
        maxHealth = health;
        renderer = GetComponentInChildren<Renderer>();
        originalColor = renderer.material.GetColor("_Color");
	}
	
	// Update is called once per frame
	void Update () {
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
	}

    public void Damage(float damage)
    {
        if (Time.time > lastTimeDamageTaken + invincibilityDurationAfterDamage || lastTimeDamageTaken == 0)
        {
            health -= damage;
            lastTimeDamageTaken = Time.time;
        }
    }
}
