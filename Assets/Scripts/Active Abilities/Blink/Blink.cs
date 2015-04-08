using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour 
{
    public float distance;
    public float cooldown;

    private float tBlinked;

    private Player player;
    private Rigidbody rigidbody;

	// Use this for initialization
	void Start () 
    {
        player = GetComponent<Player>();
        rigidbody = GetComponent<Rigidbody>();
	}
    Vector3 moveDirection;

	// Update is called once per frame
	void Update () 
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
	    if (Time.time > tBlinked + cooldown || tBlinked == 0)
        {
            if (Input.GetButtonDown("Action0") 
                && moveDirection.magnitude > 0.5f)//hit action button and is aiming in a direction
            {
                Vector3 targetPos = transform.position + moveDirection.normalized * distance;
                RaycastHit ray;
                CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();
                if (Physics.CheckCapsule(transform.TransformPoint(playerCollider.center - new Vector3(0,(playerCollider.height/3), 0)), 
                                         transform.TransformPoint(playerCollider.center + new Vector3(0, (playerCollider.height/3), 0)),
                                         playerCollider.radius
                                         //exclude enemies
                                         )
                    && Physics.Raycast(new Ray(targetPos+Vector3.up, Vector3.down), 100)
                   )
                {
                    rigidbody.MovePosition(targetPos);
                    tBlinked = Time.time;
                    /*if (hittingEnemy)
                    {
                        Enemy.SendMessage("Damage", alot);
                    }*/
                }
            }
        }
	}
}
