﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour 
{
    [System.Serializable]
    public struct CardinalDirections
    {
        public bool east;
        public bool west;
        public bool north;
        public bool south;
    }
    public CardinalDirections possibleDoorlocations;
    [HideInInspector]public CardinalDirections generatedDoorLocations;

    public GameObject[] enemies;
    public List<DoorTransition> doors;

    public Transform cameraTargetPosition;
    private GameObject mainCamera;

    [Header("Prefab References")]
    public GameObject northSouthWallPrefab;
    public GameObject eastWestWallPrefab;

    [Header("Transforms for Walls")]
    public Transform eastWall;
    public Transform westWall;
    public Transform northWall;
    public Transform southWall;
    
    private bool isLocked = true;


	// Use this for initialization
	void Start () {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        foreach (GameObject go in enemies)
        {
            go.SetActive(false);
        }

        foreach (DoorTransition door in doors)
        {
            door.transform.parent = transform;
            if (door.doorLocation == DoorTransition.CardinalDirections.east)
            {
                generatedDoorLocations.east = true;
                door.transform.parent = eastWall;
                door.transform.eulerAngles = new Vector3(0, 180, 0);
                door.transform.localPosition = Vector3.zero;
            }else if (door.doorLocation == DoorTransition.CardinalDirections.west)
            {
                generatedDoorLocations.west = true;
                door.transform.parent = westWall;
                door.transform.eulerAngles = Vector3.zero;
                door.transform.localPosition = Vector3.zero;
            }
            else if (door.doorLocation == DoorTransition.CardinalDirections.north)
            {
                generatedDoorLocations.north = true;
                door.transform.parent = northWall;
                door.transform.eulerAngles = new Vector3(0, 90, 0);
                door.transform.localPosition = Vector3.zero;
            }
            else if (door.doorLocation == DoorTransition.CardinalDirections.south)
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
        for (int i = 0; i < doors.Count; i++ )
        {
            Destroy(doors[i].blocker);
        }
        isLocked = false;
    }

    bool CheckIfEnemies()
    {
        bool enemiesExist = false;
        foreach (GameObject go in enemies)
        {
            if (go != null)
                enemiesExist = true;
        }
        return enemiesExist;
    }

    public void PlayerEnter ()
    {
        
        foreach (GameObject go in enemies)
        {
            go.SetActive(true);
        }
    }

    public void AddDoor(DoorTransition door)
    {
        doors.Add(door);
    }
}