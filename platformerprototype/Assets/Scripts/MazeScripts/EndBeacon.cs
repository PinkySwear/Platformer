using UnityEngine;
using System.Collections;

public class EndBeacon : MonoBehaviour 
{
	// This script is intended to be used as a visual notifier 
	// that the EndBeacon object is accessible and active for the player to use

	// Have a cell ready to be referenced
	private MazeCell currentCell;

	// Have a renderer handy for changing an object's color
	public Renderer rend;

	// Have a control for when the flashing behavior should be allowed
	public bool ExitEnabled = false;

	// Timing elements for the lerp function below; accesible for tweaking behavior
	public float timer = 2f;
	public float waitTime = .5f;
	public float resetPoint;

	// The two color points to use; freely editable
	public Color startColor = Color.black;
	public Color endColor = Color.white;


	public void Start()
	{
		rend = GetComponent<Renderer> ();
		resetPoint = waitTime * 2;
	}


	public void Update()
	{
		// If the exit can be used, it is "active", then it should flash between the two chosen colors
		if (ExitEnabled == true) 
		{
			timer += Time.deltaTime;

			if (timer < waitTime) 
			{
				rend.material.color = Color.Lerp (startColor, endColor, .1f);
			}

			if (timer > waitTime) 
			{
				rend.material.color = Color.Lerp (endColor, startColor, .1f);
			}

			if (timer > resetPoint) 
			{
				timer = 0;
			}
		} 
		else 
		{
			// If the exit is inactive, just leave it a solid color
			rend.material.color = startColor;
		}
	}


	// Be able to set the location of the EndBeacon so that it can be place and 
	// so that it won't be on top of other objects in the game
	public void SetLocation(MazeCell cell)
	{
		currentCell = cell;
		transform.localPosition = cell.transform.localPosition;
	}
}

