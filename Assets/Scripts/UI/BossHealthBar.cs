using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealthBar : MonoBehaviour 
{
    public Enemy[] bosses;

    private Image image;
    private float maxHealth;

    private float currentHealth;

	// Use this for initialization
	void Start () 
    {
        image = GetComponent<Image>();
        foreach (Enemy enemy in bosses)
        {
            maxHealth += enemy.health;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        currentHealth = 0;
        foreach (Enemy enemy in bosses)
        {
            currentHealth += enemy.health;
        }
        image.fillAmount = currentHealth / maxHealth;
	}
}
