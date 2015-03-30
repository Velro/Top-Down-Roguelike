using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    public float speed;
    public float damage;

    private bool piercing = false;

    private Vector3 inheritedVelocity;
    private Vector3 direction;

    Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

	// Use this for initialization
    public void Construct(Vector3 inheritedVelocity, Vector3 direction) 
    {
        this.direction = direction;
        this.inheritedVelocity = inheritedVelocity;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        rigidbody.velocity = (direction.normalized * speed);// +inheritedVelocity;
	}

    void OnCollisionEnter (Collision other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.SendMessage("Damage", damage);
            if (!piercing)
            {
                Destroy(gameObject);
            }
        }
    }
}
