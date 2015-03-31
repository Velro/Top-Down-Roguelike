using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour 
{
    public GameObject bullet;
    public float cooldown;

    private float lastShootTime;
	
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
        Quaternion bulletSpawnRotation = Quaternion.AngleAxis(Vector3.Angle(Vector3.forward, direction), Vector3.up);
        //add Vector3.up to move bullet off the ground
        //add direction/2 to keep the bullet from spawning inside the player
        Vector3 bulletSpawnPosition = transform.position + Vector3.up + direction/2;
        GameObject bulletInstance = Instantiate(bullet,
                                                bulletSpawnPosition,
                                                bulletSpawnRotation) as GameObject;
        bulletInstance.GetComponent<Bullet>().Construct(direction);
        lastShootTime = Time.time;
    }
}
