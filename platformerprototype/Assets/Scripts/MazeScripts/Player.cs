using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour 
{
	// This is the avatar of the player, currently a colored sphere

	// These are whether the player has a given key, or all of them
	public bool hasRedKey = false;
	public bool hasGreenKey = false;
	public bool hasBlueKey = false;
	public bool hasAllKeys = false;


	// The speed of the player
	public float speed = 1;

	// For more realistic movement
	private Rigidbody rb;

	// From an outdated idea; used for counting collectibles
	private int count;

	// Used for controlling the playe color
	private Color szin;

	// From an outdated idea, but may be fun later on
	public float colorChangeDelay = 0.1f;
	float currentDelay = 0f;
	bool colorChangeCollision = false;

	// Have a MazeCell handy
	private MazeCell currentCell;


	public void Start()
	{
		rb = GetComponent<Rigidbody> ();
		count = 0;
	}

	public void Update()
	{
		CheckColorChange (szin);
	}

	// For movement of the Player, based on physics
	public void FixedUpdate()
	{

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}


	// From an outdated idea, used for color-changing and collection
	void OnTriggerEnter(Collider other)
	{
		if(false){
			if (other.gameObject.CompareTag ("Yellow Pick Up"))
			{
				Debug.Log ("Contact!");
				other.gameObject.SetActive (false);
				count = count + 1;

				colorChangeCollision = true;
				currentDelay = Time.time + colorChangeDelay;

				szin = Color.yellow;
			}
			else if (other.gameObject.CompareTag ("Green Pick Up"))
			{
				Debug.Log ("Contact!");
				other.gameObject.SetActive (false);
				count = count + 1;

				colorChangeCollision = true;
				currentDelay = Time.time + colorChangeDelay;

				szin = Color.green;
			}
			else if (other.gameObject.CompareTag ("Blue Pick Up"))
			{
				Debug.Log ("Contact!");
				other.gameObject.SetActive (false);
				count = count + 1;

				colorChangeCollision = true;
				currentDelay = Time.time + colorChangeDelay;

				szin = Color.blue;
			}
			else if (other.gameObject.CompareTag ("Black Pick Up"))
			{
				Debug.Log ("Contact!");
				other.gameObject.SetActive (false);
				count = count + 1;

				colorChangeCollision = true;
				currentDelay = Time.time + colorChangeDelay;

				szin = Color.black;
			}
			else if (other.gameObject.CompareTag ("Red Pick Up"))
			{
				Debug.Log ("Contact!");
				other.gameObject.SetActive (false);
				count = count + 1;

				colorChangeCollision = true;
				currentDelay = Time.time + colorChangeDelay;

				szin = Color.red;
			}
		}
		SceneManager.LoadScene ("Level1");
	}

	// From an outdated idea, used for color-changing and collection
	void CheckColorChange(Color ize)
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

	// Be able to place the player in a MazeCell to prevent stacking of other objects
	public void SetLocation(MazeCell cell)
	{
		currentCell = cell;
		transform.localPosition = cell.transform.localPosition;
	}
		
}