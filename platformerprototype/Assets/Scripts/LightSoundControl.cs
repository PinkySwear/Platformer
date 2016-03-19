using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LightSoundControl : MonoBehaviour {

	public GameObject dog;
	public AudioSource[] aSources;

	private float changeTime = 1f;
	private bool fromAttack = false;
	private DogControls dogC;
	private float ogBlue = 0f;
	private float ogRed = 0f;

	bool wait;

	// Use this for initialization
	void Start () {
		//Debug.Log(RenderSettings.ambientLight.b);
		RenderSettings.ambientLight = new Color (ogRed,0f,ogBlue,0f);

		aSources = gameObject.GetComponents<AudioSource>();
		dogC = dog.GetComponent<DogControls> ();
		aSources[0].volume = 0f;
	}

	// Update is called once per frame
	void Update () {

		if (!dogC.isObserved){
			//RenderSettings.ambientLight = Color.Lerp (Color.blue, Color.red,changeTime*Time.deltaTime);
			StartCoroutine(PlayEvery(0.1f));
		}

		if (dogC.isObserved && fromAttack){
			//RenderSettings.ambientLight = Color.Lerp (Color.red, Color.blue,changeTime*Time.deltaTime);
			StartCoroutine(PlayEvery(0.1f));
		}
	}

	IEnumerator PlayEvery(float seconds)
	{
		if (wait) yield break;
		wait = true;
		yield return new WaitForSeconds(seconds);
		wait = false;
		if (dogC.isObserved){
			aSources[0].volume -= 0.05f;
			aSources[1].volume += 0.05f;
			if(ogBlue <= 1f){
				ogRed -= 0.1f;
				RenderSettings.ambientLight = new Color (ogRed,0f,ogBlue,0f);
			}
			if(aSources[1].volume == 1f){
				fromAttack = false;
			}
		}
		else if(!dogC.isObserved){
			//Debug.Log(aSources[0].volume);
			aSources[1].volume -= 0.05f;
			aSources[0].volume += 0.05f;
			if(ogRed <= 1f){
				ogRed += 0.1f;
				RenderSettings.ambientLight = new Color (ogRed,0f,ogBlue,0f);
			}
			if(aSources[0].volume == 1f){
				fromAttack = true;
			}
		}
	}

}