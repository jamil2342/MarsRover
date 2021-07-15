using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NameToTitle : MonoBehaviour {

	public Text title;


	void OnMouseEnter()
	{

		
		
		//title.text = name;
	}

	void OnMouseExit()
	{
		title.text = "Mars Rover";
		title.color = Color.white;
	}
}
