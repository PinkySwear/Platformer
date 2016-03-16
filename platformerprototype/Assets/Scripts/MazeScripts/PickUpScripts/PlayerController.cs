using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float speed = 1;

	private Rigidbody rb;
	private int count;
	private Color szin;

	public Text countText;
	public Text winText;

	public float colorChangeDelay = 0.1f;
	float currentDelay = 0f;
	bool colorChangeCollision = false;


	void Start()
	{
		rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText ();
		winText.text = "";
	}

	void Update()
	{
		checkColorChange (szin);
	}

	void FixedUpdate()
	{

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Yellow Pick Up"))
		{
			Debug.Log ("Contact!");
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText ();

			colorChangeCollision = true;
			currentDelay = Time.time + colorChangeDelay;

			szin = Color.yellow;
		}

		if (other.gameObject.CompareTag ("Green Pick Up"))
		{
			Debug.Log ("Contact!");
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText ();

			colorChangeCollision = true;
			currentDelay = Time.time + colorChangeDelay;

			szin = Color.green;
		}

		if (other.gameObject.CompareTag ("Blue Pick Up"))
		{
			Debug.Log ("Contact!");
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText ();

			colorChangeCollision = true;
			currentDelay = Time.time + colorChangeDelay;

			szin = Color.blue;
		}

		if (other.gameObject.CompareTag ("Black Pick Up"))
		{
			Debug.Log ("Contact!");
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText ();

			colorChangeCollision = true;
			currentDelay = Time.time + colorChangeDelay;

			szin = Color.black;
		}

		if (other.gameObject.CompareTag ("Red Pick Up"))
		{
			Debug.Log ("Contact!");
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText ();

			colorChangeCollision = true;
			currentDelay = Time.time + colorChangeDelay;

			szin = Color.red;
		}
	}

	void checkColorChange(Color ize)
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

	void SetCountText()
	{
		countText.text = "Count:" + count.ToString ();

		if (count >= 4)
		{
			winText.text = "";
		}
	}
		
}
