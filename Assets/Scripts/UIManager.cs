using UnityEngine;
using System.Collections;

public class UIManager : Singleton<UIManager> {

    public GameObject pauseMenu;
    public GameObject gameOverMenu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void TogglePauseMenu (bool isPaused)
    {
        
        if (isPaused)
        {
            //select first obj in pause menu
            pauseMenu.SetActive(true);
        }
        else
        {
            //select something else
            pauseMenu.SetActive(false);
        }

    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
    }

    public void FadedOut()
    {
        GamePlayManager.Instance.FadedOut();
    }

    public void FadedIn()
    {
        GamePlayManager.Instance.FadedIn();
    }
}
