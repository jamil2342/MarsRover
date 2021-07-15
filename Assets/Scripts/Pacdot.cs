using UnityEngine;
using System.Collections;

public class Pacdot : MonoBehaviour {

    static int count = 0;
    private void Start()
    {

        if (count!=0)
        {
            Destroy(gameObject);
        }
        count++;



    }

    void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "robot")
		{
			GameManager.score += 10;
		    GameObject[] pacdots = GameObject.FindGameObjectsWithTag("pacdot");
            Destroy(gameObject);

		    if (pacdots.Length == 1)
		    {
		        GameObject.FindObjectOfType<GameGUINavigation>().LoadLevel();
		    }
		}
	}
}
