using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StatUpgrade : MonoBehaviour 
{
    public StatUpgrade_Stats upgradeStats;


    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {
            other.SendMessage("Upgrade", upgradeStats);
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Stats/StatUpgrade_Stats")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<StatUpgrade_Stats>();
    }
#endif
}
