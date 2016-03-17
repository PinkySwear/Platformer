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
	public bool nearEnemy = false;
	private EnemyBehavior nearestEnemy;
	public bool beginCutScene = false;
	public int killNPC = 0;
	public bool hasKey = false;
	public bool notDecided = true;
	public bool enterNewRoom = false;
	public bool isObserved = false;
	public Texture jumpSprite;
	public Texture crouchSprite;
	public Texture idleSprite;
	public Texture walkSprite;
	public bool[] observedArray;

//	public Vector3 lastCheckpoint;

	public GameObject infoObj;
	private PlayerInfo myInfo;

	public GameObject initialSpawn;

	public int healthKits;

	public int myHealth;
	public bool isDead;

	private AudioSource biteSound;

	// Use this for initialization
	void Start () {
//		myInfo = infoObj.GetComponent<PlayerInfo> ();
		GetComponent<Renderer>().material.mainTexture = walkSprite;

		if (infoObj == null) {
			infoObj = GameObject.Find ("PlayerInfo");
		}
		myInfo = infoObj.GetComponent<PlayerInfo> ();
		Debug.Log (myInfo.lastCheckPoint);
		Debug.Log (myInfo.timeElapsed);
		if (myInfo.lastCheckPoint == Vector3.zero) {
			Debug.Log ("setting spawn to level start");
			myInfo.lastCheckPoint = initialSpawn.transform.position;
		}
		Debug.Log (myInfo.lastCheckPoint);
		velocity = 8f;
		jumpForce = 950f;
		myRb = GetComponent<Rigidbody> ();
		myRb.freezeRotation = true;
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Dog"), LayerMask.NameToLayer("Interactable"));
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
		myHealth = 5;
		isDead = false;
		nearEnemy = false;
		healthKits = 0;
		biteSound = GetComponent<AudioSource> ();

//		transform.position = lastCheckpoint;


		transform.position = myInfo.lastCheckPoint;
	}

//	void Awake() {
//		DontDestroyOnLoad (transform.gameObject);
//	}

	void Update() {
		//Debug.Log (myInfo.timeElapsed);
		isObserved = observedArray[0];
		for(int i = 1; i < observedArray.Length; i++) {
			isObserved = observedArray[i] && isObserved;
		}
		if (!isDead) {
			Vector3 right = transform.position + Vector3.right * transform.lossyScale.x * 0.5f;
			Vector3 left = transform.position - Vector3.right * transform.lossyScale.x * 0.5f;

			Debug.DrawLine (right, right + (Vector3.down * transform.lossyScale.y * 0.5f));
			Debug.DrawLine (left, left + (Vector3.down * transform.lossyScale.y * 0.5f));
			Debug.DrawLine (right, right + (Vector3.up * 0.76f));
			Debug.DrawLine (left, left + (Vector3.up * 0.76f));

			onSomething = Physics.Linecast (right, right + (Vector3.down * transform.lossyScale.y * 0.5f), 1 << LayerMask.NameToLayer ("Obstacle") | 1 << LayerMask.NameToLayer ("Enemy"))
				|| Physics.Linecast (left, left + (Vector3.down * transform.lossyScale.y * 0.5f), 1 << LayerMask.NameToLayer ("Obstacle") | 1 << LayerMask.NameToLayer ("Enemy"));
		
			underSomething = Physics.Linecast (right, right + (Vector3.up * 0.76f), 1 << LayerMask.NameToLayer ("Obstacle"))
			|| Physics.Linecast (left, left + (Vector3.up * 0.76f), 1 << LayerMask.NameToLayer ("Obstacle"));

			movingLeft = false;
			movingRight = false;
			if (!beginCutScene) {
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

				if (Input.GetKeyDown (KeyCode.Space) && onSomething && !crouching) {
					jump = true;
					GetComponent<Renderer>().material.mainTexture = jumpSprite;
				}
				
				if (Input.GetKey (KeyCode.S)) {
					crouching = true;
					GetComponent<Renderer>().material.mainTexture = crouchSprite;
				}
				if (!Input.GetKey (KeyCode.S) && !underSomething && onSomething) {
					crouching = false;
					GetComponent<Renderer>().material.mainTexture = walkSprite;
				}
				
				if (!Input.GetKey (KeyCode.S) && !underSomething && !onSomething) {
					GetComponent<Renderer>().material.mainTexture = jumpSprite;
				}

				if (Input.GetMouseButtonDown (0)) {
					biteSound.Play ();
					if (nearEnemy) {
						nearestEnemy.takeDamage (1);

					}
				}

				if (Input.GetKey (KeyCode.E)) {
					if (healthKits > 0) {
						myHealth = Mathf.Min(myHealth + 2, 5);
						healthKits--;
					}
				}
			}
			else {
				if (Input.GetKey (KeyCode.K)) {
					crouching = true;
					Debug.Log ("KILL");
					killNPC = 2;
				}
				if (Input.GetKey (KeyCode.H)) {
					crouching = false;
					Debug.Log ("SAVE");
					killNPC = 1;
				}
				if (Input.GetKey (KeyCode.D)) {
					enterNewRoom = true;
				}
			}
		}
		else {
			transform.rotation = Quaternion.Euler (0f, 0f, 180f);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!isDead) {
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
				jump = false;
			}
			if (!movingRight && !movingLeft) {
				myRb.velocity = new Vector3 (0f, myRb.velocity.y, myRb.velocity.z);
			}
			transform.rotation = Quaternion.Euler (Vector3.zero);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			nearestEnemy = other.gameObject.GetComponent<EnemyBehavior> ();
			if (!nearestEnemy.isDead) {
				nearEnemy = true;
			}
		}
		if (other.gameObject.tag == "Key") {
			hasKey = true;
			Destroy (other.gameObject);
		}
		if (other.gameObject.tag == "Health") {
			Destroy (other.gameObject);
			healthKits++;
		}
		if (other.gameObject.tag == "Checkpoint") {
			myInfo.lastCheckPoint = other.gameObject.transform.position;
//			lastCheckpoint = other.gameObject.transform.position;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			nearEnemy = false;
			nearestEnemy = null;
		}
	}

	public void takeDamage(int dm) {
		myHealth -= dm;
		if (myHealth == 0) {
			isDead = true;
			Debug.Log ("DOG IS DEAD");
		}
	}
}
