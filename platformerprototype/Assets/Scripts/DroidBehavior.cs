using UnityEngine;
using System.Collections;

public class DroidBehavior : MonoBehaviour {

	public bool left = true;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(left){
			transform.position = new Vector3 (transform.position.x-0.1f,transform.position.y,transform.position.z);
		}
		else{
			transform.position = new Vector3 (transform.position.x+0.1f,transform.position.y,transform.position.z);
		}
		if(transform.position.x < 0){
			left = false;
		}
		else if(transform.position.x > 30){
			left = true;
		}
	}
}
