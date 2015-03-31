using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Spikes : MonoBehaviour 
{
    [HideInInspector]
    public float damage;

    public Spikes_Stats stats;


    void Start()
    {
        damage = stats.damage;
    }
	
	void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.SendMessage("Damage", damage);
        }
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Stats/Spikes_Stats")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<Spikes_Stats>();
    }
#endif
}

public class Spikes_Stats : ScriptableObject
{
    public float damage;
}
