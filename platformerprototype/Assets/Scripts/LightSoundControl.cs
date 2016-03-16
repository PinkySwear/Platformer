using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightSoundControl : MonoBehaviour {

	public GameObject textbox;
	public GameObject dog;
	public AudioSource[] aSources;
	
    private float changeTime = 0.01f;
	private bool fromAttack = false;
	private DogControls dogC;
	bool wait;
	
	// Use this for initialization
	void Start () {
		aSources = gameObject.GetComponents<AudioSource>();
		dogC = dog.GetComponent<DogControls> ();
        aSources[0].volume = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (dogC.isObserved){
			Debug.Log(changeTime*Time.deltaTime);
			RenderSettings.ambientLight = Color.Lerp (Color.blue, Color.red,changeTime*Time.deltaTime);
			StartCoroutine(PlayEvery(0.1f));
			ShowHealth(false);
		}
		
		
		//if (dogC.findAid){
		//	ShowHealth(true);
		//}
		
		if (!dogC.isObserved && fromAttack){
			RenderSettings.ambientLight = Color.Lerp (Color.red, Color.blue,changeTime*Time.deltaTime);
			StartCoroutine(PlayEvery(0.1f));
		}
	}
	
	IEnumerator PlayEvery(float seconds)
    {
        if (wait) yield break;
        wait = true;
        yield return new WaitForSeconds(seconds);
		wait = false;
		if (fromAttack && !dogC.isObserved){
			aSources[0].volume -= 0.05f;
			aSources[1].volume += 0.05f;
			if(aSources[1].volume == 1f){
				fromAttack = false;
			}
		}
		else if(!fromAttack && dogC.isObserved){
			Debug.Log(aSources[0].volume);
			aSources[1].volume -= 0.05f;
			aSources[0].volume += 0.05f;
			if(aSources[0].volume == 1f){
				fromAttack = true;
			}
		}
	}
	
	
	void ShowHealth (bool temp) {
		// Do Something...
	}
	
}
