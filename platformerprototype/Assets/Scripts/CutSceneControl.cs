using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutSceneControl : MonoBehaviour {

	public int situation;
	public GameObject dog;
	public GameObject textbox;
	public GameObject textbox2;
	public bool next;
	public int NPC;
	
	// situation 0 => sees first enemy, situation 1 => sees first object
	// situation 2 => come to locked door, situation 3 => come to unlocked door
	// situation 4 => cutscene NPC dialogue, situation 5 => in-game NPC dialogue
	
	// If situation 4 => NPC 0 -> Supervisor Intro, NPC 1 -> dilemma 1
	//                   NPC 2 -> dilemma 2, NPC 3 -> Boss Dialgoue
	//                   NPC 4 -> Supervisor Outro ???
	
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
				//Debug.Log(textbox2.GetComponent<Text>().text);
				//Debug.Log("And now this!");
			}
			
			else if(timeup){
				dogAngle = initialDogAngle;
				dogDistance = initialDogDistance;
				if (Vector3.Angle (dogDirection, Vector3.right * direction + Vector3.down) < dogAngle) {
					if (Physics.Raycast (eyePosition, dogDirection.normalized * dogDistance, out rh, dogDistance, ~(1 << LayerMask.NameToLayer ("Interactable")))) {
						if (rh.collider.tag == "Dog") {
							//Debug.Log ("Begin Dialogue"+situation);
							//Debug.Log (dialogueStart);
							seesDog = true;
						}
					}
				}
			}
		}
	}
	
	void beginCutScene(){
		//message = guiText.text;
		//textbox.GetComponent<Text>().text = "";
		//textbox2.GetComponent<Text>().text = "";
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
		//if(situation ==3 && nearestEnemy.hasKey){
		//	Debug.Log("howdy!");
		//	dog.transform.position = new Vector3 (54f,12f,0f);
		//}
		StartCoroutine(TypeText ());
	}
	
	IEnumerator TypeText () {
		string diag = message;
		next = !next;
		if(situation == 2 && !nearestEnemy.hasKey){
			diag = "Door appears to be locked.";
		}
		foreach (char letter in diag.ToCharArray()) {
			if(next){
				textbox.GetComponent<Text>().text += letter;
				//Debug.Log(textbox.GetComponent<Text>().text);
			}
			else{
				textbox2.GetComponent<Text>().text += letter;
				//Debug.Log(textbox2.GetComponent<Text>().text);
			}
			if (sound) {
				GetComponent<AudioSource> ().PlayOneShot (sound);
				yield return 0;
			}
			yield return new WaitForSeconds (letterPause);
			//Debug.Log(letter);
		}
		//if(situation ==2 && !nearestEnemy.hasKey){
		//	yield return new WaitForSeconds (2);
		//	dialogueStart = false;
		//}
		if(situation == 1){
			yield return new WaitForSeconds (5);
			diagOver = true;
			textbox.GetComponent<Text>().text = "";
			textbox2.GetComponent<Text>().text = "";
		}
		else if(situation == 2 && !nearestEnemy.hasKey){
			yield return new WaitForSeconds (5);
			diagOver = false;
			textbox.GetComponent<Text>().text = "";
			textbox2.GetComponent<Text>().text = "";
		}
		else if(situation == 0){
			diagOver = true;
			//Debug.Log(nearestEnemy.beginCutScene);
			nearestEnemy.beginCutScene = false;
			textbox.GetComponent<Text>().text = "";
			textbox2.GetComponent<Text>().text = "";
		}
		else if(situation == 2 && nearestEnemy.hasKey){
			nearestEnemy.enterNewRoom = 0;
			diagOver = true;
			yield return new WaitForSeconds (5);
			textbox.GetComponent<Text>().text = "";
			textbox2.GetComponent<Text>().text = "";
			diagOver = false;
			dialogueStart = false;
			seesDog = false;
		}
		else if(situation == 4){
			nearestEnemy.enterNewRoom = 0;
			diagOver = true;
			yield return new WaitForSeconds (5);
			textbox.GetComponent<Text>().text = "";
			textbox2.GetComponent<Text>().text = "";
			diagOver = false;
			dialogueStart = false;
			seesDog = false;
		}
		else{
			diagOver = true;
			textbox.GetComponent<Text>().text = "";
			textbox2.GetComponent<Text>().text = "";
		}
		
		//Debug.Log(dialogueStart);
		//newThing = true;
		//textbox.GetComponent<Text>().text = "Press k to kill the dude, h to help him";
		//Debug.Log(nearestEnemy.killNPC);
	}
}
