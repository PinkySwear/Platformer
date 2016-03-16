using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelController : MonoBehaviour {

	public GameObject dog;
	public DogControls dogC;

	// Use this for initialization
	void Start () {
		dogC = dog.GetComponent<DogControls> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (dogC.isDead) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}
		}
	}
}
