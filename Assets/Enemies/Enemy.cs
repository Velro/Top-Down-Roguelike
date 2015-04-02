﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour {

    [HideInInspector]
    public float health;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float damageToPlayerOnCollision;

    new private Renderer renderer;
    
    protected Rigidbody rigidbody;

    private void Awake ()
    {
        renderer = GetComponentInChildren<Renderer>();
        rigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {

    }

    public void Damage(float damage)
    {
        health -= damage;
        StartCoroutine(Flash());

    }

    public void Die()
    {
        Destroy(gameObject);
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
}

public class Enemy_Stats : ScriptableObject
{
    public float health;
    public float speed;
    public float rigidbodyMass;
    public float damageToPlayerOnCollision;
}