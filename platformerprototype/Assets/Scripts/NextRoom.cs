using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class NextRoom : MonoBehaviour {

	public int NPC;
	public GameObject dog;
	public GameObject textbox;
	public float letterPause = 0.01f;
	public AudioClip sound;
	private DogControls nearestEnemy;
 
	public string message;
	
	private int direction;
	private float dogAngle;
	public float initialDogAngle;
	private bool seesDog = false;
	private float dogDistance;
	public float initialDogDistance;
	private bool dialogueStart = false;
	public bool newThing = false;

	// Use this for initialization
	void Start () {
		direction = 1;
		initialDogAngle = 45f;
		initialDogDistance = 5f;
	}
	
	// Update is called once per frame
	void Update () {
		
		RaycastHit rh;
		Vector3 eyePosition = transform.position + Vector3.up * 0.666f;
		Vector3 dogDirection = dog.transform.position - eyePosition;
		
		if(seesDog){
			nearestEnemy = dog.GetComponent<DogControls> ();
			nearestEnemy.beginCutScene = true;
			if(nearestEnemy.enterNewRoom){
				SceneManager.LoadScene ("Basic Maze");
			}
		}
		
		else{
			dogAngle = initialDogAngle;
			dogDistance = initialDogDistance;
			if (Vector3.Angle (dogDirection, Vector3.right * direction + Vector3.down) < dogAngle) {
				if (Physics.Raycast (eyePosition, dogDirection.normalized * dogDistance, out rh, dogDistance, ~(1 << LayerMask.NameToLayer ("Interactable")))) {
					if (rh.collider.tag == "Dog") {
						//Debug.Log ("Begin Dialogue");
						seesDog = true;
					}
				}
			}
		}
	}

}
