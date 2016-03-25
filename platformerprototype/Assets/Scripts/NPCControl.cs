using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCControl : MonoBehaviour {

	public int NPC;
	public GameObject dog;
	public GameObject textbox;
	public GameObject door;
	public GameObject elevator;
	public GameObject options;
	public string message;
	public int scene;
	
	private DogControls dogC;
	private ItemInteraction doorC;
	private ItemInteraction elevatorC;
	private bool firstContact1 = true;
	private bool firstContact2 = true;
	private bool isDead;
	private bool seesDog;
	private int mercy = 0;

	// Use this for initialization
	void Start () {
		dogC = dog.GetComponent<DogControls> ();
		doorC = door.GetComponent<ItemInteraction> ();
		elevatorC = elevator.GetComponent<ItemInteraction> ();
	}
	
	// Update is called once per frame
	void Update () {
		if((Mathf.Abs(dog.GetComponent<Transform> ().position.x - transform.position.x) <= 5)
		&& (Mathf.Abs(dog.GetComponent<Transform> ().position.y - transform.position.y) <= 5)
		&& ((transform.position.y-dog.GetComponent<Transform> ().position.y) > 0)){
			seesDog = true;
			dogC.sceneArray[NPC] = true;
		}
		else{
			seesDog = false;
			dogC.sceneArray[NPC] = false;
		}
		if(!isDead){
			if((NPC == 0) && seesDog){
				if(firstContact1){
					firstContact1 = false;
					if(!dogC.beginCutScene){
						beginFirstNPCScene1();
					}
				}
				else if(dogC.hasNPCKit){
					if(!dogC.beginCutScene){
						scene = 1;
						dogC.hasNPCKit = false;
						beginFirstNPCScene2();
					}
				}
				if(dogC.isAttacking){
					textbox.GetComponent<Text>().text = "Ugh...but...why?";
					scene = 3;
				}
				if((scene == 0) && !firstContact1){
					//textbox.GetComponent<Text>().text = "Please help...";
				}
				if(scene == 2){
					textbox.GetComponent<Text>().text = "Thanks again, pal!";
				}
			}
			else if((NPC == 1) && seesDog){
				if(firstContact2){
					firstContact2 = false;
					doorC.needsKey = true;
					if(!dogC.beginCutScene){
						beginSecondNPCScene1();
					}
				}
				if((scene == 5) && (GetComponent<EnemyBehavior>().health == 1)){
					GetComponent<EnemyBehavior>().seesDog = false;
					dogC.isAttacking = false;
					if(!dogC.beginCutScene){
						beginSecondNPCScene3();
					}
				}
				if(scene == 6){
					GetComponent<EnemyBehavior>().seesDog = false;
				}
				if(scene == 7 && (Input.GetKey (KeyCode.D))){
					textbox.GetComponent<Text>().text = "Thank you, the elevator has been activated.";
					elevatorC.needsLever = false;
					doorC.needsKey = false;
					dogC.moralChoice = false;
					scene = 8;
				}
				if(scene == 7 && (Input.GetKey (KeyCode.A))){
					textbox.GetComponent<Text>().text = "Ugh you killed me!";
					doorC.needsKey = false;
					dogC.moralChoice = false;
					scene = 8;
				}
			}
			else if((NPC == 2) && seesDog){
				if(firstContact2){
					firstContact2 = false;
					if(!dogC.beginCutScene){
						GetComponent<EnemyBehavior>().enabled = true;
						beginThirdNPCScene();
					}
				}
			}
		}
	}
	
	void beginFirstNPCScene1(){
		dogC.beginCutScene = true;
		textbox.GetComponent<Text>().text = "So! *cough* You’re my Authority contact, I suppose? I… *cough* was trying to sneak through the basement and I fell off of one of those damn CivWatch platforms.";
		doorC.needsKey = false;
		StartCoroutine(TypeText ());
	}
	
	void beginFirstNPCScene2(){
		textbox.GetComponent<Text>().text = "Thank you! You're a lifesaver! The elevator switch has been turned on!";
		elevatorC.needsLever = false;
		StartCoroutine(TypeText ());
	}
	
	void beginSecondNPCScene1(){
		dogC.beginCutScene = true;
		textbox.GetComponent<Text>().text = "Hey, you mangy mutt, what are you doing here?  Seems I have to take care of you myself!";
		StartCoroutine(TypeText ());
	}
	
	void beginSecondNPCScene3(){
		dogC.beginCutScene = true;
		textbox.GetComponent<Text>().text = "You got me! Stop, please!  I don’t want to die, they have my family and made me do this!";
		options.GetComponent<Text>().text = "Press 'd' to spare the Thug, 'a' to eliminate.";
		dogC.moralChoice = true;
		scene = 6;
		StartCoroutine(TypeText ());
	}
	
	void beginThirdNPCScene(){
		dogC.beginCutScene = true;
		textbox.GetComponent<Text>().text = "You can’t resist The Authority, Eric. Take the information drop, run back to your masters with your tail between your legs, and in three days you’ll go back to shaving your face and sleeping in a bed. You can only win if you play the game, Avery. This is the way it has to be. ";
		StartCoroutine(TypeText ());
	}
	
	IEnumerator TypeText () {
		if(scene==0){
			yield return new WaitForSeconds (5);
			dogC.beginCutScene = false;
			if(seesDog){
				textbox.GetComponent<Text>().text = "There’s a first-aid kit at the top of the elevator. *cough* I can turn on this elevator switch if you get it for me...";
			}
		}
		if(scene==1){
			yield return new WaitForSeconds (5);
			scene = 2;
		}
		if(scene==3){
			yield return new WaitForSeconds (5);
		}
		if (scene == 4){
			yield return new WaitForSeconds (2);
			dogC.beginCutScene = false;
			GetComponent<EnemyBehavior>().enabled = true;
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0.1f);
			dogC.sceneArray[NPC] = false;
			scene = 5;
		}
		if(scene == 6){
			yield return new WaitForSeconds (1);
			dogC.beginCutScene = false;
			GetComponent<EnemyBehavior>().enabled = false;
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1f);
			scene = 7;
		}
		
		if(scene == 9){
			yield return new WaitForSeconds (5);
			dogC.beginCutScene = false;
			GetComponent<EnemyBehavior>().enabled = true;
			scene = 10;
		}
	}
}
