using UnityEngine;
using System.Collections;

public class UIManager : Singleton<UIManager> {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
