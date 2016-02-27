using UnityEngine;
using System.Collections;

public class DogControls : MonoBehaviour {


	public float velocity;
	public float jumpForce;
	private Rigidbody myRb;
	private bool onSomething = false;
	private bool underSomething = false;
	private bool movingLeft;
	private bool movingRight;
	private bool jump = false;
	private bool crouching = false;
	private bool nearEnemy = false;
	private EnemyBehavior nearestEnemy;

	// Use this for initialization
	void Start () {
		velocity = 10f;
		jumpForce = 1000f;
		myRb = GetComponent<Rigidbody> ();
		myRb.freezeRotation = true;
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Dog"), LayerMask.NameToLayer("Interactable"));
	}

	void Update() {
		Vector3 right = transform.position + Vector3.right * transform.lossyScale.x * 0.5f;
		Vector3 left = transform.position - Vector3.right * transform.lossyScale.x * 0.5f;

		Debug.DrawLine (right, right + (Vector3.down * transform.lossyScale.y * 0.5f));
		Debug.DrawLine (left, left + (Vector3.down * transform.lossyScale.y * 0.5f));
		Debug.DrawLine (right, right + (Vector3.up * 0.76f));
		Debug.DrawLine (left, left + (Vector3.up * 0.76f));

		onSomething = Physics.Linecast (right, right + (Vector3.down * transform.lossyScale.y * 0.5f), 1 << LayerMask.NameToLayer ("Obstacle")) 
			|| Physics.Linecast (left, left + (Vector3.down * transform.lossyScale.y * 0.5f), 1 << LayerMask.NameToLayer ("Obstacle"));
		
		underSomething = Physics.Linecast (right, right + (Vector3.up * 0.76f), 1 << LayerMask.NameToLayer ("Obstacle")) 
			|| Physics.Linecast (left, left + (Vector3.up * 0.76f), 1 << LayerMask.NameToLayer ("Obstacle"));

		movingLeft = false;
		movingRight = false;

		if (Input.GetKey (KeyCode.A)) {
			movingLeft = true;
			Vector3 s = transform.localScale;
			s.x = -2;
			transform.localScale = s;
		}
		if (Input.GetKey (KeyCode.D)) {
			movingRight = true;
			Vector3 s = transform.localScale;
			s.x = 2;
			transform.localScale = s;
		}

		if (Input.GetKeyDown(KeyCode.Space) && onSomething && !crouching)
		{
			jump = true;
		}
		if (Input.GetKey (KeyCode.S)) {
			crouching = true;
		}
		if (!Input.GetKey (KeyCode.S) && !underSomething) {
			crouching = false;
		}

		if (Input.GetMouseButtonDown (0)) {
			if (nearEnemy) {
				nearestEnemy.isDead = true;
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (movingLeft) {
			//restrict movement to one plane
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);
			myRb.velocity = new Vector3 (-1 * velocity, myRb.velocity.y, myRb.velocity.z);
		}
		if (movingRight) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);
			myRb.velocity = new Vector3 (velocity, myRb.velocity.y, myRb.velocity.z);
		}

		if (crouching) {
			transform.localScale = new Vector3 (transform.localScale.x, 0.5f, 1f);
			velocity = 5f;
		}
		else {
			transform.localScale = new Vector3 (transform.localScale.x, 1f, 1f);
			velocity = 10f;
		}

		if (jump) {
			myRb.AddForce (Vector3.up * jumpForce);
//			Vector3 jumpDir = Vector3.up + (myRb.velocity.x / velocity * Vector3.right);
//			myRb.AddForce (jumpDir * jumpForce);
			jump = false;
		}
//		if (Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.A)) {
		if (!movingRight && !movingLeft) {
			myRb.velocity = new Vector3(0f, myRb.velocity.y, myRb.velocity.z);
		}
		transform.rotation = Quaternion.Euler (Vector3.zero);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			nearestEnemy = other.gameObject.GetComponent<EnemyBehavior> ();
			if (!nearestEnemy.isDead) {
				nearEnemy = true;
			}

		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			nearEnemy = false;
			nearestEnemy = null;
		}
	}
}
