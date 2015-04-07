using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PauseMenu : MonoBehaviour 
{
    public InputField seed;

	
	// Update is called once per frame
	void Update () 
    {
        seed.text = "" + Random.seed;
	}
}
