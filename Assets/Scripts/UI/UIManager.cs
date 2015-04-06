using UnityEngine;
using System.Collections;

public class UIManager : Singleton<UIManager> 
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    public GameObject bossHealthBarGO;

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

    //called on room enter
    public void EnableBossHealthBar(Enemy[] bosses)
    {
        bossHealthBarGO.SetActive(true);
        BossHealthBar bossHealthBar = bossHealthBarGO.GetComponentInChildren<BossHealthBar>();
        bossHealthBar.bosses = bosses;
    }

    //called after boss death animation is finished
    public void DisableBossHealthBar()
    {
        bossHealthBarGO.SetActive(false);
    }
}
