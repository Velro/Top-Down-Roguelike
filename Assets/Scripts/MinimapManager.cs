using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimapManager : Singleton<MinimapManager>
{
    public List<Vector3> roomPositions = new List<Vector3>();
    public List<Vector3> doorPositions = new List<Vector3>();

    public float spaceBetweenRooms;

    public Transform minimapContent;

    [Header("Prefab Connections")]
    public GameObject roomImagePrefab;
    public GameObject doorImagePrefab;

    public RoomManager currentRoom;

	// Use this for initialization
	void Awake () 
    {
        for (int roomPosIndex = 0; roomPosIndex < roomPositions.Count; roomPosIndex++)
        {
            GameObject roomImage = Instantiate(roomImagePrefab, Vector3.zero, Quaternion.identity) as GameObject;
            roomImage.transform.SetParent(minimapContent);
            roomImage.transform.localScale = Vector3.one;
            
            roomImage.transform.localPosition = (roomPositions[roomPosIndex] / LevelGenerator.Instance.spaceBetweenRooms) * 60;
        }

        for (int doorPosIndex = 0; doorPosIndex < doorPositions.Count; doorPosIndex++)
        {
            GameObject doorImage = Instantiate(doorImagePrefab, Vector3.zero, Quaternion.identity) as GameObject;
            doorImage.transform.SetParent(minimapContent);
            doorImage.transform.localScale = Vector3.one;

            doorImage.transform.localPosition = (doorPositions[doorPosIndex] / LevelGenerator.Instance.spaceBetweenRooms) * 60;
        }
	
	}
	
	public void UpdatePlayerPosition(RoomManager destinationRoom)
    {
        Vector3 transformedDestinationRoomPos = new Vector3(-destinationRoom.transform.position.x, -destinationRoom.transform.position.z, 0);
        minimapContent.transform.localPosition = (transformedDestinationRoomPos / LevelGenerator.Instance.spaceBetweenRooms) * 60;
    }
}
