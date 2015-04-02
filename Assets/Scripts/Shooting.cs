using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{

    public void SpawnBullet(Bullet bullet, Vector3 direction)
    {
            Quaternion bulletSpawnRotation = Quaternion.AngleAxis(Vector3.Angle(Vector3.forward, direction), Vector3.up);
            //add Vector3.up to move bullet off the ground
            //add direction/2 to keep the bullet from spawning inside the player
            Vector3 bulletSpawnPosition = transform.position + Vector3.up + direction / 2;
            Bullet bulletInstance = Instantiate(bullet,
                                                    bulletSpawnPosition,
                                                    bulletSpawnRotation) as Bullet;
            bulletInstance.Construct(direction);
    }
}
