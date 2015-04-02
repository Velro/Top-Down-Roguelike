using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RoomType
{
    normalRoom,
    treasureRoom,
    bossRoom
}

[System.Serializable]
public enum CardinalDirections
{
    east,
    west,
    north,
    south
}

public class LevelGenerator : Singleton<LevelGenerator> 
{
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
    public roomPrefabWeightPair[] treasureRooms;
    public roomPrefabWeightPair[] bossRooms;
    public DoorTransition eastWestDoorPrefab;
    public DoorTransition northSouthDoorPrefab;
    public DoorTransition eastWestBossRoomDoor;
    public DoorTransition northSouthBossRoomDoor;
    public DoorTransition eastWestTreasureRoomDoor;
    public DoorTransition northSouthTreasureRoomDoor;

    public float spaceBetweenRooms = 20;

	// Use this for initialization
	void Awake () 
    {
        builtRooms = new List<RoomManager>();
        //normal rooms
        Dictionary<RoomManager, int> roomWeightDictionary = new Dictionary<RoomManager, int>();
        for (int i = 0; i < roomTypes.Length; i++ )
        {
            roomWeightDictionary.Add(roomTypes[i].room, roomTypes[i].weight);
        }
        //boss rooms
        Dictionary<RoomManager, int> bossRoomWeightDictionary = new Dictionary<RoomManager, int>();
        for (int i = 0; i < bossRooms.Length; i++)
        {
            bossRoomWeightDictionary.Add(bossRooms[i].room, bossRooms[i].weight);
        }
        //treasure rooms
        Dictionary<RoomManager, int> treasureRoomWeightDictionary = new Dictionary<RoomManager, int>();
        for (int i = 0; i < treasureRooms.Length; i++)
        {
            treasureRoomWeightDictionary.Add(treasureRooms[i].room, treasureRooms[i].weight);
        }

        numberOfRooms = Random.Range(minNumberOfRooms, maxNumberOfRooms);

        //instantiate initial room
        RoomManager initialRoom = Instantiate(roomTypes[0].room, Vector3.zero, Quaternion.identity) as RoomManager;
        builtRooms.Add(initialRoom);
        MinimapManager.Instance.roomPositions.Add(initialRoom.transform.position);
        MinimapManager.Instance.currentRoom = initialRoom;
        //regular rooms
        for (int roomIndex = 1; 
            roomIndex < numberOfRooms - 2;//subtract 2 is for the boss room and treasure room, which is added on after this loop 
            roomIndex++)
        {
            RoomManager roomInConstruction = Instantiate(WeightedRandomizer.From(roomWeightDictionary).TakeOne(), Vector3.zero, Quaternion.identity) as RoomManager;
            roomInConstruction.roomType = RoomType.normalRoom;
            PlaceRoom(roomInConstruction);
        }
        AddInferredDoors(builtRooms.ToArray());
       
        //special rooms
        RoomManager bossRoomInConstruction = Instantiate(WeightedRandomizer.From(bossRoomWeightDictionary).TakeOne(), Vector3.zero, Quaternion.identity) as RoomManager;
        bossRoomInConstruction.roomType = RoomType.bossRoom;
        PlaceRoom(bossRoomInConstruction);
        bossRoomInConstruction.possibleDoorLocations.east = false;
        bossRoomInConstruction.possibleDoorLocations.west = false;
        bossRoomInConstruction.possibleDoorLocations.north = false;
        bossRoomInConstruction.possibleDoorLocations.south = false;
        RoomManager treasureRoomInConstruction = Instantiate(WeightedRandomizer.From(treasureRoomWeightDictionary).TakeOne(), Vector3.zero, Quaternion.identity) as RoomManager;
        treasureRoomInConstruction.roomType = RoomType.treasureRoom;
        PlaceRoom(treasureRoomInConstruction);


        PopulateMinimap(builtRooms.ToArray());
        MinimapManager.Instance.UpdatePlayerPosition(initialRoom);
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
            MinimapManager.Instance.AddRoom(ref rooms[i]);
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

                DoorTransition doorA;
                DoorTransition doorB;
                if (roomA.roomType == RoomType.bossRoom || roomB.roomType == RoomType.bossRoom)
                {
                    doorA = Instantiate(eastWestBossRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                    doorB = Instantiate(eastWestBossRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                }
                else if (roomA.roomType == RoomType.treasureRoom || roomB.roomType == RoomType.treasureRoom)
                {
                    doorA = Instantiate(eastWestTreasureRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                    doorB = Instantiate(eastWestTreasureRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                }
                else
                {
                    doorA = Instantiate(eastWestDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                    doorB = Instantiate(eastWestDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                }
                doorA.roomManager = roomA;
                doorB.roomManager = roomB;

                doorA.destination = doorB;
                doorB.destination = doorA;

                doorA.doorLocation = CardinalDirections.west;
                doorB.doorLocation = CardinalDirections.east;

                //flag this as off for following rooms
                roomA.possibleDoorLocations.west = false;
                roomB.possibleDoorLocations.east = false;

                roomA.AddDoor(doorA);
                roomB.AddDoor(doorB);

                MinimapManager.Instance.AddDoor(doorA, doorB);
            }
            else if (roomDifference.x == -spaceBetweenRooms &&
                 roomA.possibleDoorLocations.east && roomB.possibleDoorLocations.west &&
                !roomA.generatedDoorLocations.east && !roomB.generatedDoorLocations.west)
            {
                //roomA.generatedDoorLocations.east = true;
                //roomB.generatedDoorLocations.west = true;

                DoorTransition doorA;
                DoorTransition doorB;
                if (roomA.roomType == RoomType.bossRoom || roomB.roomType == RoomType.bossRoom)
                {
                    doorA = Instantiate(eastWestBossRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                    doorB = Instantiate(eastWestBossRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                }
                else if (roomA.roomType == RoomType.treasureRoom || roomB.roomType == RoomType.treasureRoom)
                {
                    doorA = Instantiate(eastWestTreasureRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                    doorB = Instantiate(eastWestTreasureRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                }
                else
                {
                    doorA = Instantiate(eastWestDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                    doorB = Instantiate(eastWestDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                }
                doorA.roomManager = roomA;
                doorB.roomManager = roomB;

                doorA.destination = doorB;
                doorB.destination = doorA;

                doorA.doorLocation = CardinalDirections.east;
                doorB.doorLocation = CardinalDirections.west;

                //flag this as off for following rooms
                roomA.possibleDoorLocations.east = false;
                roomB.possibleDoorLocations.west = false;

                roomA.AddDoor(doorA);
                roomB.AddDoor(doorB);

                MinimapManager.Instance.AddDoor(doorA, doorB);
            }
            else if (roomDifference.z == spaceBetweenRooms &&
                     roomA.possibleDoorLocations.south && roomB.possibleDoorLocations.north &&
                    !roomA.generatedDoorLocations.south && !roomB.generatedDoorLocations.north)
            {
                //roomA.generatedDoorLocations.south = true;
                //roomB.generatedDoorLocations.north = true;

                DoorTransition doorA;
                DoorTransition doorB;
                if (roomA.roomType == RoomType.bossRoom || roomB.roomType == RoomType.bossRoom)
                {
                    doorA = Instantiate(northSouthBossRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                    doorB = Instantiate(northSouthBossRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                }
                else if (roomA.roomType == RoomType.treasureRoom || roomB.roomType == RoomType.treasureRoom)
                {
                    doorA = Instantiate(northSouthTreasureRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                    doorB = Instantiate(northSouthTreasureRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                }
                else
                {
                    doorA = Instantiate(northSouthDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                    doorB = Instantiate(northSouthDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                }
                doorA.roomManager = roomA;
                doorB.roomManager = roomB;

                doorA.destination = doorB;
                doorB.destination = doorA;

                doorA.doorLocation = CardinalDirections.south;
                doorB.doorLocation = CardinalDirections.north;

                //flag this as off for following rooms
                roomA.possibleDoorLocations.south = false;
                roomB.possibleDoorLocations.north = false;

                roomA.AddDoor(doorA);
                roomB.AddDoor(doorB);

                MinimapManager.Instance.AddDoor(doorA, doorB);
            }
            else if (roomDifference.z == -spaceBetweenRooms &&
                     roomA.possibleDoorLocations.north && roomB.possibleDoorLocations.south &&
                    !roomA.generatedDoorLocations.north && !roomB.generatedDoorLocations.south)
            {
                //roomA.generatedDoorLocations.north = true;
                //roomB.generatedDoorLocations.south = true;

                DoorTransition doorA;
                DoorTransition doorB;
                if (roomA.roomType == RoomType.bossRoom || roomB.roomType == RoomType.bossRoom)
                {
                    doorA = Instantiate(northSouthBossRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                    doorB = Instantiate(northSouthBossRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                }
                else if (roomA.roomType == RoomType.treasureRoom || roomB.roomType == RoomType.treasureRoom)
                {
                    doorA = Instantiate(northSouthTreasureRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                    doorB = Instantiate(northSouthTreasureRoomDoor, Vector3.zero, Quaternion.identity) as DoorTransition;
                }
                else
                {
                    doorA = Instantiate(northSouthDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                    doorB = Instantiate(northSouthDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
                }
                doorA.roomManager = roomA;
                doorB.roomManager = roomB;

                doorA.destination = doorB;
                doorB.destination = doorA;

                doorA.doorLocation = CardinalDirections.north;
                doorB.doorLocation = CardinalDirections.south;

                //flag this as off for following rooms
                roomA.possibleDoorLocations.north = false;
                roomB.possibleDoorLocations.south = false;

                roomA.AddDoor(doorA);
                roomB.AddDoor(doorB);

                MinimapManager.Instance.AddDoor(doorA, doorB);
            }
        }
    }
}
