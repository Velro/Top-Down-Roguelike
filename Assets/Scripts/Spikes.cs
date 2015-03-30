using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour 
{

    public float damage;
	
	void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.SendMessage("Damage", damage);
        }
    }
}
