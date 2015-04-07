using UnityEngine;
using System.Collections;

public class TreasureRoomGetUpgrade : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
        GameObject upgrade = Instantiate(UpgradeManager.Instance.PickRoomUpgrade(), Vector3.zero, Quaternion.identity) as GameObject;
        upgrade.transform.parent = transform;
        upgrade.transform.localPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
