﻿using UnityEngine;
using System.Collections;

public class DoorTransition : MonoBehaviour 
{

    [System.Serializable]
    public enum CardinalDirections
    {
        east,
        west,
        north,
        south
    }
    public CardinalDirections doorLocation;

    [HideInInspector]
    public RoomManager roomManager;

    public DoorTransition destination;
    public Transform playerSpawnPosition;
    public GameObject blocker;

    [HideInInspector]
    public GameObject minimapTile;


    
    void Awake()
    {
        roomManager = transform.GetComponentInParent<RoomManager>();
    }

    // Use this for initialization
	void Start () 
    {
        roomManager = transform.GetComponentInParent<RoomManager>();
        Debug.DrawLine(transform.position, destination.transform.position, Color.cyan, 1000000f);
	}
	
	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            GamePlayManager.Instance.RoomTransition(destination.roomManager, destination);
        }
    }
}
