using UnityEngine;
using System.Collections;

public class BossRoomGetUpgrade : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
        GameObject upgrade = Instantiate(UpgradeManager.Instance.PickBossUpgrade(), Vector3.zero, Quaternion.identity) as GameObject;
        upgrade.transform.parent = transform;
        upgrade.transform.localPosition = Vector3.zero;
	}
}
