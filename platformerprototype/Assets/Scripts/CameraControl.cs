using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject focus;

	// Use this for initialization
	void Start () {
		GetComponent<Camera> ().orthographicSize = 10f;
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (focus.transform.position.x, focus.transform.position.y + 3f, -15f);
	}
}
