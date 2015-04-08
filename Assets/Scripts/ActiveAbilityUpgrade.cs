using UnityEngine;
using System.Collections;

public class ActiveAbilityUpgrade : MonoBehaviour 
{
    public Component abilityToAdd;

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
            System.Type type = abilityToAdd.GetType();
            //other.gameObject.AddComponent<type>();
        }
    }
	
}
