using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class GamePlayManager : Singleton<GamePlayManager> 
{
    public float delayToEnemyEnable = 1;

    private GameObject player;
    private GameObject mainCamera;

    //ugly
    private RoomManager currentRoom;
    private RoomManager destination;
    private DoorTransition destinationDoor;

    private bool isPaused;

	// Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main.gameObject;
	}

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
            Time.timeScale = (isPaused) ? 0 : 1;
            UIManager.Instance.TogglePauseMenu(isPaused);
        }
    }
	
    public void PlayerDied ()
    {
        Time.timeScale = 0;
        UIManager.Instance.GameOver();
    }

	public void RoomTransition (RoomManager currentRoom, RoomManager destination, DoorTransition destinationDoor)
    {
        this.currentRoom = currentRoom;
        this.destination = destination;
        this.destinationDoor = destinationDoor;
        UIManager.Instance.GetComponent<Animator>().SetTrigger("FadeOutFadeIn");
        player.GetComponent<ThirdPersonUserControl>().enabled = false;
        MinimapManager.Instance.UpdatePlayerPosition(destination);
    }

    public void FadedOut ()
    {
        player.GetComponent<Rigidbody>().MovePosition(destinationDoor.playerSpawnPosition.position);
        mainCamera.transform.position = destination.cameraTargetPosition.position;
    }

    public void FadedIn()
    {
        currentRoom.PlayerExit();
        player.GetComponent<ThirdPersonUserControl>().enabled = true;
        destination.PlayerEnter();
    }
}
