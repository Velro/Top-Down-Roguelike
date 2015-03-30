using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class GamePlayManager : Singleton<GamePlayManager> 
{

    private GameObject player;
    private GameObject mainCamera;

    //ugly
    private RoomManager destination;
    private DoorTransition destinationDoor;

	// Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main.gameObject;
	}
	
	public void RoomTransition (RoomManager destination, DoorTransition destinationDoor)
    {
        this.destination = destination;
        this.destinationDoor = destinationDoor;
        UIManager.Instance.GetComponent<Animator>().SetTrigger("FadeOutFadeIn");
        player.GetComponent<ThirdPersonUserControl>().enabled = false;
        MinimapManager.Instance.UpdatePlayerPosition(destination);
    }

    public void FadedOut ()
    {
        
        player.transform.position = destinationDoor.playerSpawnPosition.position;
        mainCamera.transform.position = destination.cameraTargetPosition.position;
    }

    public void FadedIn()
    {
        player.GetComponent<ThirdPersonUserControl>().enabled = true;
        destination.PlayerEnter();
    }
}
