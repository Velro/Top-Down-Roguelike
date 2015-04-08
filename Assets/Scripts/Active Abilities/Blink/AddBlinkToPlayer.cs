using UnityEngine;
using System.Collections;

public class AddBlinkToPlayer : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.AddComponent<Blink>();
            Destroy(gameObject);
        }
    }
}
