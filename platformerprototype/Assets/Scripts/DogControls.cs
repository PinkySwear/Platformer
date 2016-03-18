using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DogControls : MonoBehaviour {

	Animator anim;


	public float velocity;
	public float jumpForce;
	private Rigidbody myRb;
	private bool onSomething = false;
	private bool underSomething = false;
	private bool movingLeft;
	private bool movingRight;
	private bool jump = false;
	private bool crouching = false;
	public bool isAttacking = false;
	public bool gettingHit = false;
	public bool nearEnemy = false;
	private EnemyBehavior nearestEnemy;
	public bool beginCutScene = false;
	public int killNPC = 0;
	public bool hasKey = false;
	public bool notDecided = true;
	public int enterNewRoom = 0;
	public bool isObserved = false;
	public Texture jumpSprite;
	public Texture crouchSprite;
	public Texture idleSprite;
	public Texture walkSprite;
	public bool[] observedArray;
	public int enterHack = 0;
	public bool isDoor = false;

	private float timesincelastattack;
	private float timesincejump;
	private float timesincelasthit;

	public int level = 0;


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

		anim = GetComponent<Animator>();
//		myInfo = infoObj.GetComponent<PlayerInfo> ();
//		GetComponent<Renderer>().material.mainTexture = walkSprite;

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
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Interactable"));
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
		myHealth = 5;
		isDead = false;
		nearEnemy = false;
		healthKits = 0;
		biteSound = GetComponent<AudioSource> ();

//		transform.position = lastCheckpoint;


		transform.position = myInfo.lastCheckPoint;
		timesincelastattack = 0f;
		timesincejump = 0f;
		timesincelasthit = 0f;
	}

//	void Awake() {
//		DontDestroyOnLoad (transform.gameObject);
//	}

	void Update() {
		timesincelastattack += Time.deltaTime;
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




			//Debug.Log(beginCutScene);
			if (!beginCutScene) {
				if (Input.GetKey (KeyCode.A) && (!isAttacking||isAttacking && movingLeft)) {
					movingLeft = true;
					Vector3 s = transform.localScale;
					s.x = -2;
					transform.localScale = s;
				}
				else {
					movingLeft = false;
				}
				if (Input.GetKey (KeyCode.D) && (!isAttacking||isAttacking && movingRight)) {
					movingRight = true;
					Vector3 s = transform.localScale;
					s.x = 2;
					transform.localScale = s;
				}
				else {
					movingRight = false;
				}

//				if (Input.GetKey (KeyCode.D) && Input.GetKey (KeyCode.A)) {
//					movingLeft = false;
//					movingRight = false;
//				}

				if (Input.GetKeyDown (KeyCode.Space) && onSomething && !crouching) {
					jump = true;
					timesincejump = 0f;
//					GetComponent<Renderer>().material.mainTexture = jumpSprite;
				}
				
				if (Input.GetKey (KeyCode.S)) {
					crouching = true;
//					GetComponent<Renderer>().material.mainTexture = crouchSprite;
				}
				if (!Input.GetKey (KeyCode.S) && !underSomething && onSomething) {
					crouching = false;
//					GetComponent<Renderer>().material.mainTexture = walkSprite;
				}
				
				if (!Input.GetKey (KeyCode.S) && !underSomething && !onSomething) {
//					GetComponent<Renderer>().material.mainTexture = jumpSprite;
				}


				if (Input.GetMouseButtonDown (0) && !crouching && onSomething && timesincelastattack > 0.5f && !gettingHit) {
					biteSound.Play ();
					if (nearEnemy) {
						nearestEnemy.takeDamage (1);

					}
					isAttacking = true;
					timesincelastattack = 0f;
				}
				else {
					if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Attack")) {
						isAttacking = false;
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
				if (Input.GetKey (KeyCode.UpArrow)) {
					enterNewRoom = 1;
					Debug.Log ("ENTER");
					if(level==0){
						transform.position = new Vector3 (54f,12f,0f);
						beginCutScene = false;
					}
					else if(level==1){
						transform.position = new Vector3 (200f,-3f,0f);
						beginCutScene = false;
					}
					else if(level==2){
						transform.position = new Vector3 (200f,-25f,0f);
						beginCutScene = false;
					}
					else if(level==3){
						transform.position = new Vector3 (73.7f,23.9f,0f);
						beginCutScene = false;
					}
					else if(level ==4){
						transform.position = new Vector3 (215.84f,-16.52f,0f);
						beginCutScene = false;
					}
					level++;
				}
				if (Input.GetKey (KeyCode.DownArrow)) {
					enterNewRoom = 2;
					Debug.Log ("EXIT");
					beginCutScene = false;
				}
				if (Input.GetKey (KeyCode.LeftArrow)) {
					enterNewRoom = 1;
					Debug.Log ("KILL");
					beginCutScene = false;
				}
				if (Input.GetKey (KeyCode.RightArrow)) {
					enterNewRoom = 2;
					Debug.Log ("SAVE");
					beginCutScene = false;
				}
				if (Input.GetKey (KeyCode.Space)) {
					enterHack = 0;
					Debug.Log("NEWROOM");
					beginCutScene = false;
					SceneManager.LoadScene ("BasicMaze");
				}
			}
		}
		else {
			transform.rotation = Quaternion.Euler (0f, 0f, 180f);
		}

		if (gettingHit) {
			velocity = 0f;
			timesincelasthit += Time.deltaTime;
			if (timesincelasthit > 0.5f) {
				gettingHit = false;
			}
		}
		anim.SetBool ("Ground", onSomething);
		anim.SetFloat ("vSpeed", myRb.velocity.y);
		anim.SetFloat ("Speed", Mathf.Abs (myRb.velocity.x));
		anim.SetBool ("isCrawling", crouching);
		anim.SetBool ("isJumping", jump);
		anim.SetBool ("isAttacking", isAttacking);
		anim.SetBool ("isHit", gettingHit);
//		anim.SetBool ("isHit", false);
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

			if (crouching && !gettingHit) {
				transform.localScale = new Vector3 (transform.localScale.x, 2f, 1f);
				
				velocity = 5f;
			}
			else if (!gettingHit){
				transform.localScale = new Vector3 (transform.localScale.x, 2f, 1f);
				velocity = 10f;
			}

			if (jump) {
				if (timesincejump > 0.1f) {
					myRb.AddForce (Vector3.up * jumpForce);
					jump = false;
				}
				else {
					timesincejump += Time.deltaTime;
				}
			}
			if (!movingRight && !movingLeft) {
				myRb.velocity = new Vector3 (0f, myRb.velocity.y, myRb.velocity.z);
			}
			transform.rotation = Quaternion.Euler (Vector3.zero);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			EnemyBehavior tempE = other.gameObject.GetComponent<EnemyBehavior> ();
			if (!tempE.isDead) {
				nearestEnemy = tempE;
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

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			EnemyBehavior tempE = other.gameObject.GetComponent<EnemyBehavior> ();
			if (!tempE.isDead) {
				nearestEnemy = tempE;
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

	public void takeDamage(int dm) {
		myHealth -= dm;
		if (myHealth == 0) {
			isDead = true;
			Debug.Log ("DOG IS DEAD");
			anim.SetBool ("isDead", true);
		}
		timesincelasthit = 0;
		gettingHit = true;
	}
}
