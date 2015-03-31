using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimapManager : Singleton<MinimapManager>
{
    public List<Vector3> roomPositions = new List<Vector3>();
    public List<Vector3> doorPositions = new List<Vector3>();

    public RoomManager[] rooms;

    public float spaceBetweenRooms;

    public Transform minimapContent;

    [Header("Prefab Connections")]
    public GameObject roomImagePrefab;
    public GameObject doorImagePrefab;

    public RoomManager currentRoom;
	
	public void UpdatePlayerPosition(RoomManager destinationRoom)
    {
        Vector3 transformedDestinationRoomPos = new Vector3(-destinationRoom.transform.position.x, -destinationRoom.transform.position.z, 0);
        minimapContent.transform.localPosition = (transformedDestinationRoomPos / LevelGenerator.Instance.spaceBetweenRooms) * 60;

        destinationRoom.minimapTile.SetActive(true);
        DoorTransition[] destinationRoomDoors = destinationRoom.GetComponentsInChildren<DoorTransition>();
        foreach (DoorTransition door in destinationRoomDoors)
        {
            door.minimapTile.SetActive(true);
        }
    }

    public void AddRoom(ref RoomManager room)
    {
        room.minimapTile = AddToMap(room.transform.position, roomImagePrefab);
    }

    public void AddDoor(DoorTransition doorA, DoorTransition doorB)
    {
        GameObject go = AddToMap(Vector3.Lerp(doorA.roomManager.transform.position, doorB.roomManager.transform.position, 0.5f), doorImagePrefab);
        doorA.minimapTile = go;
        doorB.minimapTile = go;
    }

    GameObject AddToMap(Vector3 pos, GameObject uiObj)
    {
        pos = worldSpaceToUISpace(pos);
        GameObject uiObjInstance = Instantiate(uiObj, Vector3.zero, Quaternion.identity) as GameObject;
        uiObjInstance.transform.SetParent(minimapContent);
        uiObjInstance.transform.localScale = Vector3.one;

        uiObjInstance.transform.localPosition = (pos / LevelGenerator.Instance.spaceBetweenRooms) * 60;
        uiObjInstance.SetActive(false);

        return uiObjInstance;
    }

    Vector3 worldSpaceToUISpace (Vector3 input)
    {
        return new Vector3(input.x, input.z, 0);
    }
}
