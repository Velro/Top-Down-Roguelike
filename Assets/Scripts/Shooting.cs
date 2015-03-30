using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour 
{
    public GameObject bullet;
    public float cooldown;

    private float lastShootTime;

    Rigidbody rigidbody;

	// Use this for initialization
	void Start () 
    {
        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
	    if (Time.time > lastShootTime + cooldown)
        {
            Vector3 aimDirection = new Vector3(Input.GetAxisRaw("Aim Horizontal"), 0, Input.GetAxisRaw("Aim Vertical"));
            if (Mathf.Abs(aimDirection.x) > 0.8f)
            {
                Vector3 bulletDirection = new Vector3(aimDirection.x, 0, 0);
                SpawnBullet(bulletDirection);
            } 
            else if (Mathf.Abs(aimDirection.z) > 0.8f)
            {
                Vector3 bulletDirection = new Vector3(0, 0, aimDirection.z);
                SpawnBullet(bulletDirection);
            }
        }
	}

    void SpawnBullet (Vector3 direction)
    {
        GameObject bulletInstance = Instantiate(bullet, transform.position + Vector3.up, Quaternion.identity) as GameObject;
        bulletInstance.GetComponent<Bullet>().Construct(rigidbody.velocity, direction);
        lastShootTime = Time.time;
    }
}
