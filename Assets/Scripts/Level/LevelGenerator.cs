using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : Singleton<LevelGenerator> 
{
    [System.Serializable]
    public enum CardinalDirections
    {
        east,
        west,
        north,
        south
    }

    [System.Serializable]
    public struct roomPrefabWeightPair
    {
        public RoomManager room;
        public int weight;
    }
    
    struct ConnectionPoint
    {
        public RoomManager room;
        public CardinalDirections doorLocation;
    }

    List<RoomManager> builtRooms;

    public int minNumberOfRooms;
    public int maxNumberOfRooms;
    private int numberOfRooms;


    [Range(0f, 100f)]
    public float chanceForInferredDoorConnections = 70f;

    [Header("Prefab Connections")]
    public roomPrefabWeightPair[] roomTypes;
    public DoorTransition eastWestDoorPrefab;
    public DoorTransition northSouthDoorPrefab;

    public float spaceBetweenRooms = 20;

	// Use this for initialization
	void Awake () 
    {
        builtRooms = new List<RoomManager>();
        Dictionary<RoomManager, int> roomWeightDictionary = new Dictionary<RoomManager, int>();
        for (int i = 0; i < roomTypes.Length; i++ )
        {
            roomWeightDictionary.Add(roomTypes[i].room, roomTypes[i].weight);
        }

        numberOfRooms = Random.Range(minNumberOfRooms, maxNumberOfRooms);

        //instantiate initial room
        RoomManager initialRoom = Instantiate(roomTypes[0].room, Vector3.zero, Quaternion.identity) as RoomManager;
        builtRooms.Add(initialRoom);
        MinimapManager.Instance.roomPositions.Add(initialRoom.transform.position);
        MinimapManager.Instance.currentRoom = initialRoom;
        for (int roomIndex = 1; roomIndex < numberOfRooms; roomIndex++)
        {
            RoomManager roomInConstruction = Instantiate(WeightedRandomizer.From(roomWeightDictionary).TakeOne(), Vector3.zero, Quaternion.identity) as RoomManager;
            PlaceRoom(roomInConstruction);
        }

        AddInferredDoors(builtRooms.ToArray());

        PopulateMinimap(builtRooms.ToArray());
	}

    void PlaceRoom(RoomManager roomInConstruction)
    {
        ConnectionPoint randomConnectionPoint = GetRandomConnectionPoint(builtRooms, roomInConstruction);

        //figure out where to place next room based on connection point
        Vector3 roomPosition = Vector3.zero;
        

        if (randomConnectionPoint.doorLocation == CardinalDirections.east)
        {
            roomPosition = randomConnectionPoint.room.gameObject.transform.position + Vector3.right * spaceBetweenRooms;
            if (isRoomLocationOccupied(roomPosition))
            {
                PlaceRoom(roomInConstruction);
            }
            else
            {
                roomInConstruction.transform.position = roomPosition;
                AddDoor(randomConnectionPoint.room, roomInConstruction);
                builtRooms.Add(roomInConstruction);
            }
        }
        else if (randomConnectionPoint.doorLocation == CardinalDirections.west)
        {
            roomPosition = randomConnectionPoint.room.gameObject.transform.position + -Vector3.right * spaceBetweenRooms;
            if (isRoomLocationOccupied(roomPosition))
            {
                PlaceRoom(roomInConstruction);
            }
            else
            {
                roomInConstruction.transform.position = roomPosition;
                AddDoor(randomConnectionPoint.room, roomInConstruction);
                builtRooms.Add(roomInConstruction);
            }
        }
        else if (randomConnectionPoint.doorLocation == CardinalDirections.north)
        {
            roomPosition = randomConnectionPoint.room.gameObject.transform.position + Vector3.forward * spaceBetweenRooms;
            if (isRoomLocationOccupied(roomPosition))
            {
                PlaceRoom(roomInConstruction);
            }
            else
            {
                roomInConstruction.transform.position = roomPosition;
                AddDoor(randomConnectionPoint.room, roomInConstruction);
                builtRooms.Add(roomInConstruction);
            }
        }
        else if (randomConnectionPoint.doorLocation == CardinalDirections.south)
        {
            roomPosition = randomConnectionPoint.room.gameObject.transform.position + -Vector3.forward * spaceBetweenRooms;
            if (isRoomLocationOccupied(roomPosition))
            {
                PlaceRoom(roomInConstruction);
            }
            else
            {
                roomInConstruction.transform.position = roomPosition;
                AddDoor(randomConnectionPoint.room, roomInConstruction);
                builtRooms.Add(roomInConstruction);
            }
        }
    }

    ConnectionPoint GetRandomConnectionPoint (List<RoomManager> builtRooms, RoomManager roomInConstruction)
    {
        //get all potential places we can connect a new room in
        List<ConnectionPoint> potentialConnectionPoints = new List<ConnectionPoint>();
        for (int roomTestIndex = 0; roomTestIndex < builtRooms.Count; roomTestIndex++)
        {
            if (builtRooms[roomTestIndex].possibleDoorLocations.east && roomInConstruction.possibleDoorLocations.west)
            {
                ConnectionPoint point;
                point.room = builtRooms[roomTestIndex];
                point.doorLocation = CardinalDirections.east;
                potentialConnectionPoints.Add(point);
            }
            if (builtRooms[roomTestIndex].possibleDoorLocations.west && roomInConstruction.possibleDoorLocations.east)
            {
                ConnectionPoint point;
                point.room = builtRooms[roomTestIndex];
                point.doorLocation = CardinalDirections.west;
                potentialConnectionPoints.Add(point);
            }
            if (builtRooms[roomTestIndex].possibleDoorLocations.north && roomInConstruction.possibleDoorLocations.south)
            {
                ConnectionPoint point;
                point.room = builtRooms[roomTestIndex];
                point.doorLocation = CardinalDirections.north;
                potentialConnectionPoints.Add(point);
            }
            if (builtRooms[roomTestIndex].possibleDoorLocations.south && roomInConstruction.possibleDoorLocations.north)
            {
                ConnectionPoint point;
                point.room = builtRooms[roomTestIndex];
                point.doorLocation = CardinalDirections.south;
                potentialConnectionPoints.Add(point);
            }
        }

        ConnectionPoint randomConnectionPoint = potentialConnectionPoints[Random.Range(0, potentialConnectionPoints.Count)];
        return randomConnectionPoint;
    }

    bool isRoomLocationOccupied (Vector3 potentialPosition)
    {
        bool result = false;
        for (int i = 0; i < builtRooms.Count; i++ )
        {
            if (potentialPosition == builtRooms[i].transform.position)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    void PopulateMinimap(RoomManager[] rooms)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            Vector3 transformedRoomInConstructionPos = new Vector3(rooms[i].transform.position.x, rooms[i].transform.position.z, 0);
            MinimapManager.Instance.roomPositions.Add(transformedRoomInConstructionPos);
        }
    }


    void AddInferredDoors (RoomManager[] builtRooms)
    {
        for (int workingRoomIndex = 0; workingRoomIndex < builtRooms.Length; workingRoomIndex++)
        {
            for (int checkRoomIndex = 0; checkRoomIndex < builtRooms.Length; checkRoomIndex++)
            {
                AddDoor(builtRooms[workingRoomIndex], builtRooms[checkRoomIndex], chanceForInferredDoorConnections);//this will fail a lot
            }
        }
    }

    void AddDoor(RoomManager roomA, RoomManager roomB, float chanceToSucceed = 100f)
    {
        Vector3 roomDifference = roomA.transform.position - roomB.transform.position;
        if (roomDifference.magnitude == spaceBetweenRooms && chanceToSucceed >= Random.Range(0, 100))
        {
            if (roomDifference.x == spaceBetweenRooms &&
                roomA.possibleDoorLocations.west && roomB.possibleDoorLocations.east &&
                !roomA.generatedDoorLocations.west && !roomB.generatedDoorLocations.east)
            {
                //roomA.generatedDoorLocations.west = true;
                //roomB.generatedDoorLocations.east = true;

                DoorTransition doorA = Instantiate(eastWestDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                DoorTransition doorB = Instantiate(eastWestDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                doorA.destination = doorB;
                doorB.destination = doorA;

                doorA.doorLocation = DoorTransition.CardinalDirections.west;
                doorB.doorLocation = DoorTransition.CardinalDirections.east;

                //flag this as off for following rooms
                roomA.possibleDoorLocations.west = false;
                roomB.possibleDoorLocations.east = false;

                roomA.AddDoor(doorA);
                roomB.AddDoor(doorB);

                Vector3 roomApos = new Vector3(roomA.transform.position.x, roomA.transform.position.z, 0);
                Vector3 roomBpos = new Vector3(roomB.transform.position.x, roomB.transform.position.z, 0);
                MinimapManager.Instance.doorPositions.Add(Vector3.Lerp(roomApos,
                                                                       roomBpos, 0.5f));

            }
            else if (roomDifference.x == -spaceBetweenRooms &&
                 roomA.possibleDoorLocations.east && roomB.possibleDoorLocations.west &&
                !roomA.generatedDoorLocations.east && !roomB.generatedDoorLocations.west)
            {
                //roomA.generatedDoorLocations.east = true;
                //roomB.generatedDoorLocations.west = true;

                DoorTransition doorA = Instantiate(eastWestDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                DoorTransition doorB = Instantiate(eastWestDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                doorA.destination = doorB;
                doorB.destination = doorA;

                doorA.doorLocation = DoorTransition.CardinalDirections.east;
                doorB.doorLocation = DoorTransition.CardinalDirections.west;

                //flag this as off for following rooms
                roomA.possibleDoorLocations.east = false;
                roomB.possibleDoorLocations.west = false;

                roomA.AddDoor(doorA);
                roomB.AddDoor(doorB);

                Vector3 roomApos = new Vector3(roomA.transform.position.x, roomA.transform.position.z, 0);
                Vector3 roomBpos = new Vector3(roomB.transform.position.x, roomB.transform.position.z, 0);
                MinimapManager.Instance.doorPositions.Add(Vector3.Lerp(roomApos,
                                                                       roomBpos, 0.5f));
            }
            else if (roomDifference.z == spaceBetweenRooms &&
                     roomA.possibleDoorLocations.south && roomB.possibleDoorLocations.north &&
                    !roomA.generatedDoorLocations.south && !roomB.generatedDoorLocations.north)
            {
                //roomA.generatedDoorLocations.south = true;
                //roomB.generatedDoorLocations.north = true;

                DoorTransition doorA = Instantiate(northSouthDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                DoorTransition doorB = Instantiate(northSouthDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                doorA.destination = doorB;
                doorB.destination = doorA;

                doorA.doorLocation = DoorTransition.CardinalDirections.south;
                doorB.doorLocation = DoorTransition.CardinalDirections.north;

                //flag this as off for following rooms
                roomA.possibleDoorLocations.south = false;
                roomB.possibleDoorLocations.north = false;

                roomA.AddDoor(doorA);
                roomB.AddDoor(doorB);

                Vector3 roomApos = new Vector3(roomA.transform.position.x, roomA.transform.position.z, 0);
                Vector3 roomBpos = new Vector3(roomB.transform.position.x, roomB.transform.position.z, 0);
                MinimapManager.Instance.doorPositions.Add(Vector3.Lerp(roomApos,
                                                                       roomBpos, 0.5f));
            }
            else if (roomDifference.z == -spaceBetweenRooms &&
                     roomA.possibleDoorLocations.north && roomB.possibleDoorLocations.south &&
                    !roomA.generatedDoorLocations.north && !roomB.generatedDoorLocations.south)
            {
                //roomA.generatedDoorLocations.north = true;
                //roomB.generatedDoorLocations.south = true;

                DoorTransition doorA = Instantiate(northSouthDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                DoorTransition doorB = Instantiate(northSouthDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                doorA.destination = doorB;
                doorB.destination = doorA;

                doorA.doorLocation = DoorTransition.CardinalDirections.north;
                doorB.doorLocation = DoorTransition.CardinalDirections.south;

                //flag this as off for following rooms
                roomA.possibleDoorLocations.north = false;
                roomB.possibleDoorLocations.south = false;

                roomA.AddDoor(doorA);
                roomB.AddDoor(doorB);

                Vector3 roomApos = new Vector3(roomA.transform.position.x, roomA.transform.position.z, 0);
                Vector3 roomBpos = new Vector3(roomB.transform.position.x, roomB.transform.position.z, 0);
                MinimapManager.Instance.doorPositions.Add(Vector3.Lerp(roomApos,
                                                                       roomBpos, 0.5f));
            }
        }
    }
}
