using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LightSoundControl : MonoBehaviour {

	public GameObject dog;
	public AudioSource[] aSources;
	public bool finalBoss;
	public bool fightBoss;

	private float changeTime = 1f;
	private bool fromAttack = false;
	private DogControls dogC;
	private float ogBlue = 0f;
	private float ogRed = 0f;
	private AudioSource default_music;
	private AudioSource fight_music;
	private AudioSource boss_dialogue_music;
	private AudioSource boss_fight_music;
	private bool startDialogue;
	private bool startFight;

	bool wait;

	// Use this for initialization
	void Start () {
		//Debug.Log(RenderSettings.ambientLight.b);
		RenderSettings.ambientLight = new Color (ogRed,0f,ogBlue,0f);

		aSources = gameObject.GetComponents<AudioSource>();
		fight_music = aSources[0];
		default_music = aSources[1];
		boss_dialogue_music = aSources[2];
		boss_fight_music = aSources[3];
		dogC = dog.GetComponent<DogControls> ();
		aSources[0].volume = 0f;
		aSources[2].volume = 0f;
		aSources[3].volume = 0f;
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
		if(finalBoss){
			if(!startDialogue){
				boss_dialogue_music.Play();
				startDialogue = true;
			}
		}
		if(fightBoss){
			if(!startFight){
				boss_fight_music.Play();
				Debug.Log("yees?");
				startFight = true;
			}
		}
	}

	IEnumerator PlayEvery(float seconds)
	{
		if (wait) yield break;
		wait = true;
		yield return new WaitForSeconds(seconds);
		wait = false;
		if(!finalBoss){
			if (dogC.isObserved){
				fight_music.volume -= 0.05f;
				default_music.volume += 0.05f;
				if(ogBlue <= 1f){
					ogRed -= 0.1f;
					RenderSettings.ambientLight = new Color (ogRed,0f,ogBlue,0f);
				}
				if(default_music.volume == 1f){
					fromAttack = false;
				}
			}
			else if(!dogC.isObserved){
				default_music.volume -= 0.05f;
				fight_music.volume += 0.05f;
				if(ogRed <= 1f){
					ogRed += 0.1f;
					RenderSettings.ambientLight = new Color (ogRed,0f,ogBlue,0f);
				}
				if(fight_music.volume == 1f){
					fromAttack = true;
				}
			}
		}
		else{
			if(ogRed>0){
				ogRed -= 0.1f;
				RenderSettings.ambientLight = new Color (ogRed,0f,ogBlue,0f);
			}
			if(default_music.volume > 0f){
				default_music.volume -= 0.05f;
			}
			if(fight_music.volume > 0f){
				fight_music.volume -= 0.05f;
			}
			if(boss_dialogue_music.volume < 1f){
				boss_dialogue_music.volume += 0.05f;
			}
		}
		if(fightBoss){
			if(boss_dialogue_music.volume > 0f){
				boss_dialogue_music.volume -= 0.1f;
			}
			if(boss_dialogue_music.volume < 0.1f){
				boss_fight_music.volume = 1f;
			}
		}
	}

}