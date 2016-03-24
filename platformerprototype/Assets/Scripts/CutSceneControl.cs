using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutSceneControl : MonoBehaviour {

	public int situation;
	public GameObject dog;
	public GameObject textbox;
	public bool next;
	public int NPC;
	
	public float letterPause = 0.001f;
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
	private bool timeup = true;
	private bool diagOver = false;

	// Use this for initialization
	void Start () {
		direction = 1;
		initialDogAngle = 45f;
		initialDogDistance = 5f;
	}
	
	// Update is called once per frame
	void Update () {
		if(!diagOver){
			RaycastHit rh;
			Vector3 eyePosition = transform.position + Vector3.up * 0.666f;
			Vector3 dogDirection = dog.transform.position - eyePosition;
			
			if(seesDog && !dialogueStart){
				beginCutScene();
			}
			
			else{
				dogAngle = initialDogAngle;
				dogDistance = initialDogDistance;
				if (Vector3.Angle (dogDirection, Vector3.right * direction + Vector3.down) < dogAngle) {
					if (Physics.Raycast (eyePosition, dogDirection.normalized * dogDistance, out rh, dogDistance, ~(1 << LayerMask.NameToLayer ("Interactable")))) {
						if (rh.collider.tag == "Dog") {
							seesDog = true;
						}
					}
				}
			}
		}
	}
	
	void beginCutScene(){
		seesDog = false;
		nearestEnemy = dog.GetComponent<DogControls> ();
		if(situation == 0 || situation == 3 || (situation == 2 && nearestEnemy.hasKey) || situation == 4){
			nearestEnemy.beginCutScene = true;
			dialogueStart = true;
			dog.GetComponent<Renderer>().material.mainTexture = nearestEnemy.idleSprite;
		}
		else{
			dialogueStart = true;
		}
		StartCoroutine(TypeText ());
	}
	
	IEnumerator TypeText () {
		yield return new WaitForSeconds (5);
	}
}
