using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {


	public Vector3 lastCheckPoint;
	public int enemiesKilled;
	public float timeDetected;
	public float timeElapsed;

	public static PlayerInfo Instance;

	// Use this for initialization
	void Start () {
//		DontDestroyOnLoad (transform.gameObject);
		timeElapsed = 0f;
	}

	void Awake() {
		DontDestroyOnLoad (transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
	}
}
