using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCControl : MonoBehaviour {

	public int NPC;
	public GameObject dog;
	public GameObject textbox;
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
		
		if(seesDog && !dialogueStart){
			beginCutScene();
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
		if(newThing){
				if((nearestEnemy.killNPC == 1)&&nearestEnemy.notDecided){
					textbox.GetComponent<Text>().text = "Woah you saved me! Here's a key to the next room, wowza!";
					nearestEnemy.hasKey = true;
					nearestEnemy.notDecided = false;
				}
				else if((nearestEnemy.killNPC == 2)&&nearestEnemy.notDecided){
					textbox.GetComponent<Text>().text = "Oh no! You've killed me, ugh! *YOU HAVE PICKED THE NEXT ROOM KEY";
					nearestEnemy.hasKey = true;
					nearestEnemy.notDecided = false;
				}
				if(nearestEnemy.notDecided == false){
					nearestEnemy.beginCutScene = false;
				}
			}
	}
	
	void beginCutScene(){
		//message = guiText.text;
		textbox.GetComponent<Text>().text = "";
		StartCoroutine(TypeText ());
		seesDog = false;
		nearestEnemy = dog.GetComponent<DogControls> ();
		nearestEnemy.beginCutScene = true;
		dialogueStart = true;
	}
	
	IEnumerator TypeText () {
		foreach (char letter in message.ToCharArray()) {
			textbox.GetComponent<Text>().text += letter;
			if (sound) {
				GetComponent<AudioSource> ().PlayOneShot (sound);
				yield return 0;
			}
			yield return new WaitForSeconds (letterPause);
			Debug.Log(letter);
		}
		newThing = true;
		textbox.GetComponent<Text>().text = "Press k to kill the dude, h to help him";
		Debug.Log(nearestEnemy.killNPC);
	}
}
