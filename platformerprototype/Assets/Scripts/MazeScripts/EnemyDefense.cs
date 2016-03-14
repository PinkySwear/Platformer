using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyDefense : MonoBehaviour 
{
	// This is a variant on the Player object and is meant to be a "gate" or a "block" that the Player
	// can't get past until obtaining a key.  Once a key of a given color is obtained, the intent is
	// that the gates for that color will no longer cause a reset of the maze

	// Outdated, could be used for moving enemies
	public float speed = 1;

	// Outdated, could be used for color-changing enemies
	private Color szin;

	// Outdated, all for the lerp behaviors below
	public float colorChangeDelay = 0.1f;
	float currentDelay = 0f;
	bool colorChangeCollision = false;

	// Keep a MazeCell handy for reference
	private MazeCell currentCell;

	// Has the enemy touched a player? If so, it will cause a reset of the maze
	public bool touchedPlayer = false;

	// Set the color to plain black, meaning inactive
	public void Start()
	{
		szin = Color.black;
	}

	// See if the color should be changed
	public void Update()
	{
		CheckColorChange (szin);
	}


	public void setEnemyColor(Color ize)
	{
		szin = ize;
	}

	public Color getEnemyColor()
	{
		return szin;
	}

	// Used for lerp behavior
	public void CheckColorChange(Color ize)
	{
		if(colorChangeCollision)
		{
			if (Time.time > currentDelay)
			{
				transform.GetComponent<Renderer> ().material.color = ize;
				colorChangeCollision = false;
			}
		}
	}

	// Outdated, can be used for other behaviors if desired
	public void ChangeColorCollision(Collider other)
	{
		if(other.gameObject.CompareTag("Enemy"))
		{
			Debug.Log ("Blarg");
			touchedPlayer = true;
		}
		if (other.gameObject.CompareTag ("Yellow Pick Up"))
		{
			Debug.Log ("Contact!");
			other.gameObject.SetActive (false);

			colorChangeCollision = true;
			currentDelay = Time.time + colorChangeDelay;

			szin = Color.yellow;
		}
		else if (other.gameObject.CompareTag ("Green Pick Up"))
		{
			Debug.Log ("Contact!");
			other.gameObject.SetActive (false);

			colorChangeCollision = true;
			currentDelay = Time.time + colorChangeDelay;

			szin = Color.green;
		}
		else if (other.gameObject.CompareTag ("Blue Pick Up"))
		{
			Debug.Log ("Contact!");
			other.gameObject.SetActive (false);

			colorChangeCollision = true;
			currentDelay = Time.time + colorChangeDelay;

			szin = Color.blue;
		}
		else if (other.gameObject.CompareTag ("Black Pick Up"))
		{
			Debug.Log ("Contact!");
			other.gameObject.SetActive (false);

			colorChangeCollision = true;
			currentDelay = Time.time + colorChangeDelay;

			szin = Color.black;
		}
		else if (other.gameObject.CompareTag ("Red Pick Up"))
		{
			Debug.Log ("Contact!");
			other.gameObject.SetActive (false);

			colorChangeCollision = true;
			currentDelay = Time.time + colorChangeDelay;

			szin = Color.red;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) 
		{
			Debug.Log ("Blarg");
			touchedPlayer = true;
		}
	}

	// If the enemy has touched the player, return that to the Game Manager for resetting the Maze
	public bool isPlayerDead()
	{
		return touchedPlayer;
	}

	// For placing the Enemy object in the Maze to prevent stacking
	public void SetLocation(MazeCell cell)
	{
		currentCell = cell;
		transform.localPosition = cell.transform.localPosition;
	}
		
}