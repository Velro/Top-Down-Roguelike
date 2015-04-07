using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour 
{
    public RoomType roomType;

    [System.Serializable]
    public struct NonExclusiveCardinalDirections
    {
        public bool east;
        public bool west;
        public bool north;
        public bool south;
    }
    public NonExclusiveCardinalDirections possibleDoorLocations;
    [HideInInspector]
    public NonExclusiveCardinalDirections generatedDoorLocations;

    [HideInInspector]
    public Enemy[] enemies;

    [HideInInspector]
    public DoorTransition[] doors;

    public Transform cameraTargetPosition;

    [Header("Prefab References")]
    public GameObject northSouthWallPrefab;
    public GameObject eastWestWallPrefab;
    public GameObject bossRoomDoor;
    public GameObject treasureRoomDoor;
    public GameObject normalDoor;

    [Header("Transforms for Walls")]
    public Transform eastWall;
    public Transform westWall;
    public Transform northWall;
    public Transform southWall;
    
    private bool isLocked = true;

    [HideInInspector]
    public GameObject minimapTile;


	// Use this for initialization
	void Start () 
    {
        enemies = transform.GetComponentsInChildren<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.enabled = false;
        }
        TreasureRoomGetUpgrade[] treasureRoomUpgrades = GetComponentsInChildren<TreasureRoomGetUpgrade>();
        foreach (TreasureRoomGetUpgrade treasureSpawner in treasureRoomUpgrades)
        {
            treasureSpawner.enabled = false;
        }
        
        doors = transform.GetComponentsInChildren<DoorTransition>();

        foreach (DoorTransition door in doors)
        {
            door.transform.parent = transform;
            if (door.doorLocation == CardinalDirections.east)
            {
                generatedDoorLocations.east = true;
                door.transform.parent = eastWall;
                door.transform.eulerAngles = new Vector3(0, 180, 0);
                door.transform.localPosition = Vector3.zero;
            }else if (door.doorLocation == CardinalDirections.west)
            {
                generatedDoorLocations.west = true;
                door.transform.parent = westWall;
                door.transform.eulerAngles = Vector3.zero;
                door.transform.localPosition = Vector3.zero;
            }
            else if (door.doorLocation == CardinalDirections.north)
            {
                generatedDoorLocations.north = true;
                door.transform.parent = northWall;
                door.transform.eulerAngles = new Vector3(0, 90, 0);
                door.transform.localPosition = Vector3.zero;
            }
            else if (door.doorLocation == CardinalDirections.south)
            {
                generatedDoorLocations.south = true;
                door.transform.parent = southWall;
                door.transform.eulerAngles = new Vector3(0, 270, 0);
                door.transform.localPosition = Vector3.zero; 

            }
        }

        //add walls where there is no door
        if (!generatedDoorLocations.east)
        {
            GameObject wall = Instantiate(eastWestWallPrefab);
            wall.transform.parent = eastWall;
            wall.transform.eulerAngles = Vector3.zero;
            wall.transform.localPosition = Vector3.zero; 
        }
        if (!generatedDoorLocations.west)
        {
            GameObject wall = Instantiate(eastWestWallPrefab);
            wall.transform.parent = westWall;
            wall.transform.eulerAngles = Vector3.zero;
            wall.transform.localPosition = Vector3.zero; 
        }
        if (!generatedDoorLocations.north)
        {
            GameObject wall = Instantiate(northSouthWallPrefab);
            wall.transform.eulerAngles = new Vector3(0, 90, 0);
            wall.transform.parent = northWall;

            wall.transform.localPosition = Vector3.zero;
        }
        if (!generatedDoorLocations.south)
        {
            GameObject wall = Instantiate(northSouthWallPrefab);
            wall.transform.eulerAngles = new Vector3(0, 90, 0);
            wall.transform.parent = southWall;
            
            wall.transform.localPosition = Vector3.zero; 
        }   
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocked)
            return;

        if (!CheckIfEnemies())
        {
            UnlockDoors();
        }
	}



    void UnlockDoors()
    {
        for (int i = 0; i < doors.Length; i++ )
        {
            Destroy(doors[i].doorModel);
        }
        isLocked = false;
    }

    bool CheckIfEnemies()
    {
        bool enemiesExist = false;
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
                enemiesExist = true;
        }
        return enemiesExist;
    }

    public void PlayerEnter ()
    {
        Invoke("EnableEnemies", GamePlayManager.Instance.delayToEnemyEnable);
        TreasureRoomGetUpgrade[] treasureRoomUpgrades = GetComponentsInChildren<TreasureRoomGetUpgrade>();
        foreach (TreasureRoomGetUpgrade treasureSpawner in treasureRoomUpgrades)
        {
            treasureSpawner.enabled = true;
        }
    }

    public void PlayerExit ()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)//will be null if player re-enters room
            {
                Destroy(enemy.gameObject);
            }
        }
    }

    public void AddDoor(DoorTransition door)
    {
        door.transform.parent = transform;//just throw this anywhere for now
    }

    private void EnableEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)//will be null if player re-enters room
            {
                enemy.enabled = true;
            }
        }

        if (roomType == RoomType.bossRoom)
        {
            Enemy[] bosses = enemies;
            UIManager.Instance.EnableBossHealthBar(bosses);
        }
    }
}
