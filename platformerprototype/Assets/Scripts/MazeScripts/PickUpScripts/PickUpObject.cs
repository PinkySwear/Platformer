using UnityEngine;
using System.Collections;

public class PickUpObject : MonoBehaviour 
{
	// Used for controlling the playe color
	public Color szin;

	// Have a MazeCell handy
	private MazeCell currentCell;


	public void Update()
	{
		transform.GetComponent<Renderer> ().material.color = szin;
	}

	// Be able to place the player in a MazeCell to prevent stacking of other objects
	public void SetLocation(MazeCell cell)
	{
		currentCell = cell;
		transform.localPosition = cell.transform.localPosition;
	}
}
