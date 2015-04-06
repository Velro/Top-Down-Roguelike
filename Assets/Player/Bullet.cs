using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    public float speed;
    public float damage;

    public bool piercing = false;

    private Vector3 direction;

    new Rigidbody rigidbody;

    public GameObject particleSystemOnInstantiate;
    public GameObject particleSystemOnDestroy;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if (particleSystemOnInstantiate != null)
            Instantiate(particleSystemOnInstantiate, transform.position, Quaternion.identity);
    }

	// Use this for initialization
    public void Construct(Vector3 direction) 
    {
        this.direction = direction;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        rigidbody.velocity = (direction.normalized * speed);// +inheritedVelocity;
	}

    void OnCollisionEnter (Collision other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("Damage", damage);
            if (!piercing)
            {
                if (particleSystemOnDestroy != null) Instantiate(particleSystemOnDestroy, transform.position, Quaternion.identity);
                    Destroy(gameObject);
            }   
        }
        else
        {
            if (particleSystemOnDestroy != null) Instantiate(particleSystemOnDestroy, transform.position, Quaternion.identity);
                Destroy(gameObject);
        }
    }
}
