using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemInteraction : MonoBehaviour {

	public GameObject lever;
	public GameObject key;
	public GameObject dog;
	public bool isDoor;
	public bool isSwitch;
	public bool needsKey;
	public bool needsLever;
	public GameObject textbox;
	public float minFloor;
	public float maxFloor;
	public float doorHeight;
	public int doorIndx;
	
	private bool down;
	private bool seesDog;
	private bool openDoor;
	private DogControls dogC;
	private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
		dogC = dog.GetComponent<DogControls> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(isDoor){
			if((Mathf.Abs(dog.GetComponent<Transform> ().position.x - transform.position.x) <= 5)
				&& (Mathf.Abs(dog.GetComponent<Transform> ().position.y - transform.position.y) <= 5)
			    && ((transform.position.y-dog.GetComponent<Transform> ().position.y) > 0)){
				seesDog = true;
				dogC.doorArray[doorIndx] = true;
			}
			else{
				seesDog = false;
				dogC.doorArray[doorIndx] = false;
			}
			if (seesDog && !openDoor && !dogC.moralChoice){
				textbox.GetComponent<Text>().text = "Press 'D' to Open Door";
				if (dogC.openDoor && ((dogC.hasKey && needsKey) || (!needsKey))){
					openDoor = true;
					if(needsKey){
						needsKey = false;
						dogC.hasKey = false;
					}
				}
				else if(dogC.openDoor && !dogC.moralChoice){
					textbox.GetComponent<Text>().text = "Door appears to be locked, a key may be necessary.";
				}
			}
			if((openDoor & seesDog) && (transform.position.y <= originalPosition.y+5)){
				transform.position = new Vector3 (transform.position.x,transform.position.y+0.1f,transform.position.z);
			}
			if(openDoor & !seesDog){
				transform.position = new Vector3 (transform.position.x,transform.position.y-0.1f,transform.position.z);
			}
			if(transform.position.y <= originalPosition.y){
				openDoor = false;
			}
		}
		else if(!needsLever){
			if(!isSwitch){
				if(down){
					transform.position = new Vector3 (transform.position.x,transform.position.y-0.1f,transform.position.z);
				}
				else{
					transform.position = new Vector3 (transform.position.x,transform.position.y+0.1f,transform.position.z);
				}
				if(transform.position.y < minFloor){
					down = false;
				}
				else if(transform.position.y > maxFloor){
					down = true;
				}
			}
			else{
				if((Mathf.Abs(dog.GetComponent<Transform> ().position.x - transform.position.x) <= 5)
				&& (Mathf.Abs(dog.GetComponent<Transform> ().position.y - transform.position.y) <= 5)
			    && ((transform.position.y-dog.GetComponent<Transform> ().position.y) > 0)){
					seesDog = true;
					dogC.switchArray[doorIndx] = true;
				}
				else{
					seesDog = false;
					dogC.switchArray[doorIndx] = false;
				}
				if(seesDog && !dogC.moralChoice){
					textbox.GetComponent<Text>().text = "Press 'D' to Activate Elevator";
					if(dogC.openDoor){
						lever.GetComponent<ItemInteraction> ().needsLever = false;
						textbox.GetComponent<Text>().text = "Elevator has been activated.";
					}
				}
			}
		}
	}
}
