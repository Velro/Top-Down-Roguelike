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
	}

    void PlaceRoom(RoomManager roomInConstruction)
    {
        
        //get all potential places we can connect a new room in
        List<ConnectionPoint> potentialConnectionPoints = new List<ConnectionPoint>();
        for (int roomTestIndex = 0; roomTestIndex < builtRooms.Count; roomTestIndex++)
        {
            if (builtRooms[roomTestIndex].possibleDoorlocations.east && roomInConstruction.possibleDoorlocations.west)
            {
                ConnectionPoint point;
                point.room = builtRooms[roomTestIndex];
                point.doorLocation = CardinalDirections.east;
                potentialConnectionPoints.Add(point);
            }
            if (builtRooms[roomTestIndex].possibleDoorlocations.west && roomInConstruction.possibleDoorlocations.east)
            {
                ConnectionPoint point;
                point.room = builtRooms[roomTestIndex];
                point.doorLocation = CardinalDirections.west;
                potentialConnectionPoints.Add(point);
            }
            if (builtRooms[roomTestIndex].possibleDoorlocations.north && roomInConstruction.possibleDoorlocations.south)
            {
                ConnectionPoint point;
                point.room = builtRooms[roomTestIndex];
                point.doorLocation = CardinalDirections.north;
                potentialConnectionPoints.Add(point);
            }
            if (builtRooms[roomTestIndex].possibleDoorlocations.south && roomInConstruction.possibleDoorlocations.north)
            {
                ConnectionPoint point;
                point.room = builtRooms[roomTestIndex];
                point.doorLocation = CardinalDirections.south;
                potentialConnectionPoints.Add(point);
            }
        }

        ConnectionPoint randomConnectionPoint = potentialConnectionPoints[Random.Range(0, potentialConnectionPoints.Count)];
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
                AddDoors(randomConnectionPoint, roomInConstruction);
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
                AddDoors(randomConnectionPoint, roomInConstruction);
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
                AddDoors(randomConnectionPoint, roomInConstruction);
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
                AddDoors(randomConnectionPoint, roomInConstruction);
                builtRooms.Add(roomInConstruction);
            }
        }
    }

    void AddDoors(ConnectionPoint randomConnectionPoint, RoomManager roomInConstruction)
    {
        if (randomConnectionPoint.doorLocation == CardinalDirections.east)
        {
            randomConnectionPoint.room.possibleDoorlocations.east = true;
            roomInConstruction.generatedDoorLocations.west = true;

            DoorTransition doorForAlreadyBuiltRoom = Instantiate(eastWestDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
            DoorTransition doorForRoomInConstruction = Instantiate(eastWestDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
            doorForAlreadyBuiltRoom.destination = doorForRoomInConstruction;
            doorForRoomInConstruction.destination = doorForAlreadyBuiltRoom;

            doorForAlreadyBuiltRoom.doorLocation = DoorTransition.CardinalDirections.east;
            doorForRoomInConstruction.doorLocation = DoorTransition.CardinalDirections.west;

            //flag this as off for following rooms
            randomConnectionPoint.room.possibleDoorlocations.east = false;
            roomInConstruction.possibleDoorlocations.west = false;

            randomConnectionPoint.room.AddDoor(doorForAlreadyBuiltRoom);
            roomInConstruction.AddDoor(doorForRoomInConstruction);

            SendMapInfoToMinimap(roomInConstruction, randomConnectionPoint);
        }
        else if (randomConnectionPoint.doorLocation == CardinalDirections.west)
        {
            randomConnectionPoint.room.possibleDoorlocations.west = true;
            roomInConstruction.generatedDoorLocations.east = true;

            DoorTransition doorForAlreadyBuiltRoom = Instantiate(eastWestDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
            DoorTransition doorForRoomInConstruction = Instantiate(eastWestDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
            doorForAlreadyBuiltRoom.destination = doorForRoomInConstruction;
            doorForRoomInConstruction.destination = doorForAlreadyBuiltRoom;

            doorForAlreadyBuiltRoom.doorLocation = DoorTransition.CardinalDirections.west;
            doorForRoomInConstruction.doorLocation = DoorTransition.CardinalDirections.east;

            //flag this as off for following rooms
            randomConnectionPoint.room.possibleDoorlocations.west = false;
            roomInConstruction.possibleDoorlocations.east = false;

            randomConnectionPoint.room.AddDoor(doorForAlreadyBuiltRoom);
            roomInConstruction.AddDoor(doorForRoomInConstruction);

            SendMapInfoToMinimap(roomInConstruction, randomConnectionPoint);
        }
        else if (randomConnectionPoint.doorLocation == CardinalDirections.north)
        {
            randomConnectionPoint.room.possibleDoorlocations.north = true;
            roomInConstruction.generatedDoorLocations.south = true;

            DoorTransition doorForAlreadyBuiltRoom = Instantiate(northSouthDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
            DoorTransition doorForRoomInConstruction = Instantiate(northSouthDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
            doorForAlreadyBuiltRoom.destination = doorForRoomInConstruction;
            doorForRoomInConstruction.destination = doorForAlreadyBuiltRoom;

            doorForAlreadyBuiltRoom.doorLocation = DoorTransition.CardinalDirections.north;
            doorForRoomInConstruction.doorLocation = DoorTransition.CardinalDirections.south;

            //flag this as off for following rooms
            randomConnectionPoint.room.possibleDoorlocations.north = false;
            roomInConstruction.possibleDoorlocations.south = false;

            randomConnectionPoint.room.AddDoor(doorForAlreadyBuiltRoom);
            roomInConstruction.AddDoor(doorForRoomInConstruction);

            SendMapInfoToMinimap(roomInConstruction, randomConnectionPoint);
        }
        else if (randomConnectionPoint.doorLocation == CardinalDirections.south)
        {
            randomConnectionPoint.room.possibleDoorlocations.south = true;
            roomInConstruction.generatedDoorLocations.north = true;

            DoorTransition doorForAlreadyBuiltRoom = Instantiate(northSouthDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
            DoorTransition doorForRoomInConstruction = Instantiate(northSouthDoorPrefab, Vector3.zero, Quaternion.identity) as DoorTransition;
            doorForAlreadyBuiltRoom.destination = doorForRoomInConstruction;
            doorForRoomInConstruction.destination = doorForAlreadyBuiltRoom;

            doorForAlreadyBuiltRoom.doorLocation = DoorTransition.CardinalDirections.south;
            doorForRoomInConstruction.doorLocation = DoorTransition.CardinalDirections.north;

            //flag this as off for following rooms
            randomConnectionPoint.room.possibleDoorlocations.south = false;
            roomInConstruction.possibleDoorlocations.north = false;

            randomConnectionPoint.room.AddDoor(doorForAlreadyBuiltRoom);
            roomInConstruction.AddDoor(doorForRoomInConstruction);

            SendMapInfoToMinimap(roomInConstruction, randomConnectionPoint);
        }

        
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

    void SendMapInfoToMinimap (RoomManager roomInConstruction, ConnectionPoint connectionPoint)
    {
        Vector3 transformedRoomInConstructionPos = new Vector3(roomInConstruction.transform.position.x, roomInConstruction.transform.position.z, 0);
        Vector3 transformedConnectionPointRoomPos = new Vector3(connectionPoint.room.transform.position.x, connectionPoint.room.transform.position.z, 0);
        MinimapManager.Instance.roomPositions.Add(transformedRoomInConstructionPos);//move from XZ plane to XY plane
        MinimapManager.Instance.doorPositions.Add(Vector3.Lerp(transformedRoomInConstructionPos,
                                                               transformedConnectionPointRoomPos, 0.5f));
    }
}
