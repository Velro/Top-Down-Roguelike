using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour 
{
    public Bullet bullet;
    public float cooldown;

    private float lastShootTime;

    Shooting shooting;
	
    void Start()
    {
        shooting = GetComponent<Shooting>();
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
                shooting.SpawnBullet(bullet, bulletDirection);
                lastShootTime = Time.time;
            } 
            else if (Mathf.Abs(aimDirection.z) > 0.8f)
            {
                Vector3 bulletDirection = new Vector3(0, 0, aimDirection.z);
                shooting.SpawnBullet(bullet, bulletDirection);
                lastShootTime = Time.time;
            }
        }
	}

}
