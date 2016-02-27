using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject focus;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (focus.transform.position.x, focus.transform.position.y, -15f);
	}
}
