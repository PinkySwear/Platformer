using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutSceneControl : MonoBehaviour {

	public int scene;
	public GameObject dog;
	public GameObject textbox;
	public string message;
	private DogControls dogC;
	
	private bool seesDog = false;

	// Use this for initialization
	void Start () {
		dogC = dog.GetComponent<DogControls> ();
	}
	
	// Update is called once per frame
	void Update () {
		if((Mathf.Abs(dog.GetComponent<Transform> ().position.x - transform.position.x) <= 5)
		&& (Mathf.Abs(dog.GetComponent<Transform> ().position.y - transform.position.y) <= 5)
		&& ((transform.position.y-dog.GetComponent<Transform> ().position.y) > 0)){
			seesDog = true;
			dogC.sceneArray[scene] = true;
		}
		else{
			seesDog = false;
			dogC.sceneArray[scene] = false;
		}
		if(seesDog){
			textbox.GetComponent<Text>().text = message;
		}
	}
	
}
