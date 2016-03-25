using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehavior : MonoBehaviour {

	Animator anim;

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
	bool jump;
	bool onSomething;


	private int dogAngle;
	public int initialDogAngle;
	private float viewDistance;
	public float initialviewDistance;
	private Vector3 focusDirection;
	private int coneQuality;
	private Vector3 eyePosition;
	private Vector3 feetPosition;
	private float dogDistance;

	private Vector3 tempDirection;

	public List<RaycastHit> hits = new List<RaycastHit>();
	private RaycastHit hit;
	private float timesincedeath;


	/********************************
	 * */
	public List<Material> materials;

	Vector3[] newVertices;
	Vector2[] newUV;
	int[] newTriangles;
	Mesh mesh;
	MeshRenderer meshRenderer;
	int i;
	int v;
	/********************************
	 * */

	private float timesincelastblock;

	public float attentionSpan;
	private float attentionCountdown;
	public int num;
	public int myType;
	public int health;

	private bool isAttacking;

	private float timesincelastattack;
	private float timesincelastjump;

	public bool nearDog;
	private DogControls dogC;
	private float attackCD;

	private AudioSource hitSound;
	public bool gettingHit = false;
	public float timesincelasthit = 0f;

	public GameObject infoObj;
	private PlayerInfo myInfo;




	// Use this for initialization
	void Start () {

		if (infoObj == null) {
			infoObj = GameObject.Find ("PlayerInfo");
		}
		infoObj = GameObject.Find ("PlayerInfo");
		myInfo = infoObj.GetComponent<PlayerInfo> ();

		anim = GetComponent<Animator>();
		timesincelastattack = 0f;
		direction = 1;
		isDead = false;
		initialSpeed = 4f;
		initialDogAngle = 90;
		initialviewDistance = 10f;
		attentionSpan = 5f;
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
		//Debug.Log("Interaction"+num);
		notice =  GameObject.Find("Interaction"+num);
		hitSound = GetComponent<AudioSource> ();
		////Debug.Log(notice);
		notice.GetComponent<MeshRenderer>().enabled = false;

		focusDirection = new Vector3 (direction, -0.9f, 0f);
		coneQuality = 1;



		timesincelastblock = 0f;
		timesincelastjump = 0f;

		mesh = GetComponentInChildren<MeshFilter>().mesh;
		meshRenderer = GetComponentInChildren<MeshRenderer>();

		meshRenderer.material = materials[0];
		timesincedeath = 0f;

	}
	
	// Update is called once per frame
	void Update () {


		timesincelastblock += Time.deltaTime;
		timesincelastjump += Time.deltaTime;
		transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);
		if (!isDead) {

			Vector3 right = transform.position + Vector3.right * transform.lossyScale.x * 0.15f;
			Vector3 left = transform.position - Vector3.right * transform.lossyScale.x * 0.15f;

			Debug.DrawLine (right, right + (Vector3.down * transform.lossyScale.y * 0.62f));
			Debug.DrawLine (left, left + (Vector3.down * transform.lossyScale.y * 0.62f));

			onSomething = Physics.Linecast (right, right + (Vector3.down * transform.lossyScale.y * 0.62f), 1 << LayerMask.NameToLayer ("Obstacle") | 1 << LayerMask.NameToLayer ("Enemy"))
				|| Physics.Linecast (left, left + (Vector3.down * transform.lossyScale.y * 0.62f), 1 << LayerMask.NameToLayer ("Obstacle") | 1 << LayerMask.NameToLayer ("Enemy"));

			dogDistance = Vector3.Distance (dog.transform.position, transform.position);
			RaycastHit rh;
			bool blocked = false;
			eyePosition = transform.position + Vector3.up * 0.666f;
			feetPosition = transform.position + Vector3.down * 1f;
			Debug.DrawLine (feetPosition, feetPosition + (Vector3.right * direction).normalized, Color.red);

			Vector3 dogDirection = dog.transform.position - eyePosition;
			if (Physics.Raycast (feetPosition, Vector3.right * direction, out rh, 1f, ~(1 << LayerMask.NameToLayer ("Interactable") | 1 << LayerMask.NameToLayer ("Enemy") | 1 << LayerMask.NameToLayer ("Dog")))) {
				if (rh.collider.tag == "Thingy" && !isDead && !jump) {
					blocked = true;
					timesincelastblock = 0f;
				}
			}

			if (seesDog) {
				if(blocked && !jump && onSomething && timesincelastjump > 0.05f) {
					jump = true;
					timesincelastjump = 0f;
				}
				focusDirection = dogDirection;
				dogAngle = 180;
				speed = 7f;
				if (Vector3.Distance (transform.position, dog.transform.position) < 1.5f) {
					speed = 0f;
				}
				viewDistance = 10f;
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
					if (dogAngle > initialDogAngle) {
						dogAngle = dogAngle - 5;
					}
					else if (dogAngle < initialDogAngle) {
						dogAngle = initialDogAngle;
					}
					focusDirection = new Vector3 (direction, -0.9f, 0f);
//					dogAngle = initialDogAngle;
					speed = initialSpeed;
					viewDistance = initialviewDistance;
					notice.GetComponent<MeshRenderer>().enabled = false;
					dogC.observedArray[num-1] = true;
					if(blocked && !jump && onSomething && timesincelastjump > 0.1f) {
						if (timesincelastblock > 0.3f) {
							if (Random.value > 0.8f) {
								jump = true;
								timesincelastjump = 0f;
							}
						}
						else if (Random.value > 2f * timesincelastblock + 0.2f) {
							jump = true;
							timesincelastjump = 0f;
						}
						else {
							direction = direction * -1;
						}
					}
				}
				else {
					speed = 0f;
					attentionCountdown -= Time.deltaTime;
				}
			}
			if (gettingHit) {
				speed = 0f;
			}
			seesDog = false;
			if (Vector3.Distance (dog.transform.position, transform.position) < 2f) {
				seesDog = true;
				//Debug.Log("I see you!");
				dogC.observedArray[num-1] = false;
			}
//			if (Vector3.Angle (dogDirection, Vector3.right * direction + Vector3.down) < dogAngle) {
//				if (Physics.Raycast (eyePosition, dogDirection.normalized * viewDistance, out rh, viewDistance, ~(1 << LayerMask.NameToLayer ("Interactable") | 1 <<LayerMask.NameToLayer("Enemy")))) {
//					if (rh.collider.tag == "Dog") {
////						Debug.Log ("I SAW THE FUCKING DOG");
//						seesDog = true;
//						dogC.observedArray[num-1] = false;
//						notice.GetComponent<MeshRenderer>().enabled = true;
//						Vector3 temp = new Vector3(transform.position.x,transform.position.y + 3,0);
//						notice.transform.position = temp;
//						//Debug.Log("SEEN!");
//					}
//				}
//			}
				

//			Debug.DrawRay (eyePosition, dogDirection.normalized * viewDistance, Color.green);
//			Debug.DrawRay (eyePosition, Vector3.right * direction * viewDistance, Color.red);
//			Debug.DrawRay (eyePosition, Vector3.down * viewDistance, Color.red);
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);
			myRb.velocity = new Vector3 (direction * speed, myRb.velocity.y, myRb.velocity.z);

			Vector3 s = transform.localScale;
			s.x = direction * 3f;
			transform.localScale = s;

			if (myType == 0) {
				if (nearDog) {
					attackCD -= Time.deltaTime;
				}
				if (nearDog && attackCD <= 0f && !isDead && !(dogC.gettingHit) && seesDog && !gettingHit) {
//					Debug.Log ("I ATTACKED");
					hitSound.Play ();
					dogC.takeDamage (1);
					attackCD = 1f;
					isAttacking = true;
					timesincelastattack = 0f;
					anim.SetBool ("isAttacking", isAttacking);
				}
				if (dogC.gettingHit) {
					attackCD = 1f;
				}
			}
			if (dogDistance < 50) {
				castRays ();
				updateMesh ();
				updateMeshMaterial ();
			}

		}
		else {
			notice.GetComponent<MeshRenderer>().enabled = false;
//			transform.rotation = Quaternion.Euler (0f, 0f, 90f);
			if (timesincedeath < 0.01f) {
				myRb.AddForce ((Vector3.up * 5f + Vector3.right * Mathf.Sign((transform.position - dog.transform.position).x) * 3f).normalized * 300f);
//				myRb.AddForce ((Vector3.up * 5f).normalized * 300f);
			}
			timesincedeath += Time.deltaTime;
			dogC.observedArray[num-1] = true;
			gameObject.layer = 9;
			meshRenderer.enabled = false;
			myRb.freezeRotation = false;
			myRb.AddTorque (new Vector3 (70f, 0f, 0f));
//			myRb.AddTorque (new Vector3 (0f, 0f, direction * 90f));
//			myRb.rotation = (Quaternion.Lerp(Quaternion.Euler(0f, 0f, 0f), Quaternion.Euler(0f, 0f, direction * 90f), timesincedeath * 2.5f));
			transform.position = new Vector3 (transform.position.x, transform.position.y, 10f);
		}
//		if (timesincelastattack < 0.1f) {
			timesincelastattack += Time.deltaTime;
//		}
		if (speed == 0) {
			anim.SetBool ("isIdling", true);
			anim.SetBool ("isWalking", false);
		}
		else {
			anim.SetBool ("isIdling", false);
			anim.SetBool ("isWalking", true);
		}
		if (isAttacking) {
//			Debug.Log ("IAMATTACKING");
			timesincelastattack += Time.deltaTime;
			if (timesincelastattack > 0.2f) {
				isAttacking = false;
			}
		}

		if (gettingHit) {
			speed = 0f;
			timesincelasthit += Time.deltaTime;
			if (timesincelasthit > 0.3f) {
				gettingHit = false;
			}
		}

		anim.SetBool ("isHit", gettingHit);
		anim.SetBool ("isAttacking", isAttacking);


	}

	void FixedUpdate() {
		if (jump) {
			myRb.AddForce (Vector3.up * 950f);
			jump = false;
		}
	}


	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Dog") {
			//Debug.Log ("gothere");
			if (!dogC.isDead) {
				nearDog = true;
			}
			if (timesincelastattack > 2f) {
				attackCD = 0.25f;
			}
			else {
				attackCD = 1f;
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Dog") {
			nearDog = false;
		}
	}

	public void takeDamage (int dm) {
		gettingHit = true;
		timesincelasthit = 0f;
		health -= dm;
		if (health == 0) {
			isDead = true;
			myInfo.enemiesKilled++;
		}
	}

//	void OnCollisionEnter (Collision collision) {
//		if (collision.collider.tag == "Thingy" && !isDead) {
//			direction = direction * -1;
//		}
//	}




	void castRays() {
		int numRays = dogAngle * coneQuality;
		float currAngle = dogAngle / -2;

		hits.Clear ();
	

		for (int i = 0; i < numRays; i++)
		{
			tempDirection = Quaternion.AngleAxis (currAngle, Vector3.forward) * focusDirection;
			tempDirection = Vector3.Normalize (tempDirection);
			hit = new RaycastHit();

			if (Physics.Raycast (eyePosition, tempDirection, out hit, viewDistance, ~(1 << LayerMask.NameToLayer ("Interactable") | 1 << LayerMask.NameToLayer ("Enemy"))) == false) {		
//				Debug.Log (hit.collider);
				hit.point = eyePosition + (tempDirection * viewDistance);
				hits.Add (hit);
			}
			else {
				if (hit.collider.tag == "Dog") {
//					hit.point = eyePosition + (tempDirection * viewDistance);
					seesDog = true;
					dogC.observedArray [num - 1] = false;
					notice.GetComponent<MeshRenderer> ().enabled = true;
					Vector3 temp = new Vector3 (transform.position.x, transform.position.y + 3, 0);
					notice.transform.position = temp;
					hit.point = hit.collider.transform.position;
					hits.Add (hit);
//					if (Physics.Raycast (eyePosition, tempDirection, out hit, viewDistance, ~(1 << LayerMask.NameToLayer ("Interactable") | 1 << LayerMask.NameToLayer ("Enemy") | 1 << LayerMask.NameToLayer ("Dog"))) == false) {		
//						//				Debug.Log (hit.collider);
//						hit.point = eyePosition + (tempDirection * viewDistance);
//
//					}
//					hits.Add (hit);

				}
				else {
					hits.Add (hit);
				}
			}

//			hits.Add (hit);

			currAngle += 1f / coneQuality;
		}
	}

	void updateMesh () {
		if (hits == null || hits.Count == 0)
			return;

		if (mesh.vertices.Length != hits.Count + 1)
		{
			mesh.Clear();
			newVertices = new Vector3[hits.Count + 1];
			newTriangles = new int[(hits.Count - 1) * 3];

			i = 0;
			v = 1;
			while (i < newTriangles.Length)
			{
				if ((i % 3) == 0)
				{
					newTriangles[i] = 0;
					newTriangles[i + 1] = v;
					newTriangles[i + 2] = v + 1;
					v++;
				}
				i++;
			}
		}

		newVertices[0] = transform.InverseTransformPoint(eyePosition);
		for (i = 1; i <= hits.Count; i++)
		{
			newVertices[i] = transform.InverseTransformPoint(hits[i-1].point);
		}

		newUV = new Vector2[newVertices.Length];
		i = 0;
		while (i < newUV.Length) {
			newUV[i] = new Vector2(newVertices[i].x, newVertices[i].z);
			i++;
		}

		mesh.vertices = newVertices;
		mesh.triangles = newTriangles;
		mesh.uv = newUV;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		meshRenderer.gameObject.transform.localScale = new Vector3 (-1 * direction, 1f, 1f);
		meshRenderer.gameObject.transform.localEulerAngles = new Vector3 (0f, (direction + 1) * 90, 0f);
	}


	void updateMeshMaterial () {
//		for (i = 0; i < materials.Count; i++)
//		{
//			if (meshRenderer.material != materials[i])
//			{
//				meshRenderer.material = materials[i];
//			}
//		}

		if (seesDog) {
			meshRenderer.material = materials [1];
		}
		else {
			meshRenderer.material = materials [0];
		}
	}

//	void OnDrawGizmosSelected()
//	{
//		Gizmos.color = Color.cyan;
//
//		if (true && hits.Count > 0) 
//		{
//			foreach (RaycastHit hit in hits)
//			{
//				Gizmos.DrawSphere(hit.point, 0.04f);
//				Gizmos.DrawLine(transform.position, hit.point);
//			}
//		}
//	}




//	void OnCollisionStay (Collision collision) {
//		if (collision.collider.tag == "Thingy" && !isDead) {
//			Debug.Log ("CHANGING DIRECTION");
//			direction = direction * -1;
//		}
//	}
}
