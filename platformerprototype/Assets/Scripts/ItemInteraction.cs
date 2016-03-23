using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemInteraction : MonoBehaviour {

	public GameObject lever;
	public GameObject key;
	public GameObject dog;
	public bool isDoor;
	public bool needsKey;
	public bool needsLever;
	public GameObject textbox;
	public float minFloor;
	public float maxFloor;
	
	private int direction;
	private bool down;
	private float dogAngle;
	public float initialDogAngle;
	public float initialDogDistance;
	private float dogDistance;
	private bool seesDog;
	private bool openDoor;
	private DogControls dogC;
	private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
		direction = 1;
		originalPosition = transform.position;
		dogC = dog.GetComponent<DogControls> ();
		initialDogAngle = 45f;
		initialDogDistance = 5f;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit rh;
		Vector3 eyePosition = transform.position + Vector3.up * 0.666f;
		Vector3 dogDirection = dog.transform.position - eyePosition;
		dogAngle = initialDogAngle;
		dogDistance = initialDogDistance;
		textbox.GetComponent<Text>().text = "";
		if(isDoor){
			if ((Vector3.Angle (dogDirection, Vector3.left * direction + Vector3.down) < dogAngle) ||
			    (Vector3.Angle (dogDirection, Vector3.right * direction + Vector3.down) < dogAngle)) {
				if (Physics.Raycast (eyePosition, dogDirection.normalized * dogDistance, out rh, dogDistance, ~(1 << LayerMask.NameToLayer ("Interactable")))) {
					if ((rh.collider.tag == "Dog") || (rh.collider.tag == "Enemy")){
						seesDog = true;
						dogC.nearDoor = true;
					}
				}
			}
			if (seesDog & !openDoor){
				if(needsKey){
					textbox.GetComponent<Text>().text = "Press 'D' to Open Door";
					Debug.Log(textbox.GetComponent<Text>().text);
				}
				if(dogC.openDoor){
					Debug.Log(dogC.hasKey);
				}
				if (dogC.openDoor && ((dogC.hasKey && needsKey) || (!needsKey))){
					openDoor = true;
					if(needsKey){
						needsKey = false;
						dogC.hasKey = false;
					}
				}
				else if(dogC.openDoor){
					textbox.GetComponent<Text>().text = "Door appears to be locked, a key may be necessary.";
				}
			}
			//else if(!seesDog){
			//	Debug.Log("hm?");
			//	textbox.GetComponent<Text>().text = "";
			//	dogC.nearDoor = false;
			//}
			//Debug.Log(openDoor);
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
		seesDog = false;
	}
}
