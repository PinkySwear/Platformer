using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public bool isDead;
	private int direction;
	private Rigidbody myRb;
	public GameObject dog;
	//public GameObject noticePrefab;
	public GameObject notice;
	public bool seesDog = false;
	//private Renderer myRend;
	private float speed;
	public float initialSpeed;
	private float dogAngle;
	public float initialDogAngle;
	private float dogDistance;
	public float initialDogDistance;
	public float attentionSpan;
	private float attentionCountdown;
	public int num;
	public int myType;
	public int health;

	public bool nearDog;
	private DogControls dogC;
	private float attackCD;

	private AudioSource hitSound;


	// Use this for initialization
	void Start () {
		direction = 1;
		isDead = false;
		initialSpeed = 6f;
		initialDogAngle = 45f;
		initialDogDistance = 5f;
		attentionSpan = 2f;
		attentionCountdown = -1f;
		speed = initialSpeed;
		myRb = GetComponent<Rigidbody> ();
		myRb.freezeRotation = true;
		//myRend = GetComponent<Renderer> ();
		if (myType == 0) {
			health = 2;
		}
		else if (myType == 1) {
			health = 1;
		}
		nearDog = false;
		dogC = dog.GetComponent<DogControls> ();
		attackCD = 1f;
		Debug.Log("Interaction"+num);
		notice =  GameObject.Find("Interaction"+num);
		hitSound = GetComponent<AudioSource> ();
		//Debug.Log(notice);
		notice.GetComponent<MeshRenderer>().enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (!isDead) {
			RaycastHit rh;
			Vector3 eyePosition = transform.position + Vector3.up * 0.666f;
			Vector3 dogDirection = dog.transform.position - eyePosition;
			if (seesDog) {
				dogAngle = 181f;
				speed = 9f;
				if (Vector3.Distance (transform.position, dog.transform.position) < 2f) {
					speed = 0f;
				}
				dogDistance = 10f;
				if (dogDirection.x < 0) {
					direction = -1;
				}
				else {
					direction = 1;
				}
				attentionCountdown = 2f;
			}
			else {
				if (attentionCountdown < 0f) {
					dogAngle = initialDogAngle;
					speed = initialSpeed;
					dogDistance = initialDogDistance;
					notice.GetComponent<MeshRenderer>().enabled = false;
					dogC.observedArray[num-1] = true;
				}
				else {
					speed = 0f;
					attentionCountdown -= Time.deltaTime;
				}
			}
			seesDog = false;
			if (Vector3.Distance (dog.transform.position, transform.position) < 2f) {
				seesDog = true;
				dogC.observedArray[num-1] = false;
			}
			if (Vector3.Angle (dogDirection, Vector3.right * direction + Vector3.down) < dogAngle) {
				if (Physics.Raycast (eyePosition, dogDirection.normalized * dogDistance, out rh, dogDistance, ~(1 << LayerMask.NameToLayer ("Interactable") | 1 <<LayerMask.NameToLayer("Enemy")))) {
					if (rh.collider.tag == "Dog") {
						Debug.Log ("I SAW THE FUCKING DOG");
						seesDog = true;
						dogC.observedArray[num-1] = false;
						notice.GetComponent<MeshRenderer>().enabled = true;
						Vector3 temp = new Vector3(transform.position.x,transform.position.y + 3,0);
						notice.transform.position = temp;
						//Debug.Log("SEEN!");
					}
				}
			}
				

			Debug.DrawRay (eyePosition, dogDirection.normalized * dogDistance, Color.green);
			Debug.DrawRay (eyePosition, Vector3.right * direction * dogDistance, Color.red);
			Debug.DrawRay (eyePosition, Vector3.down * dogDistance, Color.red);
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);
			myRb.velocity = new Vector3 (direction * speed, myRb.velocity.y, myRb.velocity.z);

			Vector3 s = transform.localScale;
			s.x = direction;
			transform.localScale = s;

			if (myType == 0) {
				if (nearDog) {
					attackCD -= Time.deltaTime;
				}
				if (nearDog && attackCD <= 0f && !isDead) {
					Debug.Log ("I ATTACKED");
					hitSound.Play ();
					dogC.takeDamage (1);
					attackCD = 1f;
				}
			}
		}
		else {
			transform.rotation = Quaternion.Euler (0f, 0f, 90f);
			dogC.observedArray[num-1] = true;
		}

	}



	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Dog") {
			Debug.Log ("gothere");
			if (!dogC.isDead) {
				nearDog = true;
			}
			attackCD = 0.5f;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Dog") {
			nearDog = false;
		}
	}

	public void takeDamage (int dm) {
		health -= dm;
		if (health == 0) {
			isDead = true;
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Thingy" && !isDead) {
			direction = direction * -1;
		}
	}
}
