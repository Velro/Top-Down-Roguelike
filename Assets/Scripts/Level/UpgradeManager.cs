using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [System.Serializable]
    struct UpgradeWeightPair
    {
        public GameObject upgrade;
        public int weight;
    }



    [SerializeField]UpgradeWeightPair[] roomDrops;
    private Dictionary<GameObject, int> roomDropsDictionary = new Dictionary<GameObject, int>();
    
    [SerializeField]UpgradeWeightPair[] bossDrops;
    private Dictionary<GameObject, int> bossDropsDictionary = new Dictionary<GameObject, int>();

	// Use this for initialization
	void Start () {
        for (int i = 0; i < roomDrops.Length; i++)
        {
            roomDropsDictionary.Add(roomDrops[i].upgrade, roomDrops[i].weight);
        }
        for (int i = 0; i < bossDrops.Length; i++)
        {
            bossDropsDictionary.Add(bossDrops[i].upgrade, bossDrops[i].weight);
        }
	}


    public GameObject PickRoomUpgrade ()
    {
        return PickUpgrade(roomDropsDictionary);
    }

    public GameObject PickBossUpgrade()
    {
        return PickUpgrade(bossDropsDictionary);
    }

    private GameObject PickUpgrade (Dictionary<GameObject, int> dict)
    {
        GameObject upgrade = WeightedRandomizer.From(dict).TakeOne();
        
        //so we dont get the same upgrade twice
        if (roomDropsDictionary.ContainsKey(upgrade))
        {
            roomDropsDictionary.Remove(upgrade);
        }
        if (bossDropsDictionary.ContainsKey(upgrade))
        {
            bossDropsDictionary.Remove(upgrade);
        }

        return upgrade;
    }
}
