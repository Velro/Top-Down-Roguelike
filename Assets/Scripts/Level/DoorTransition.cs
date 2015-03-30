using UnityEngine;
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

    private RoomManager roomManager;

    public DoorTransition destination;
    public Transform playerSpawnPosition;
    public GameObject blocker;


    private GameObject player;
    
    // Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
