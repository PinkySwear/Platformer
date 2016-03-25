using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]

public class PlayGameAudio : MonoBehaviour {
public AudioClip themeMusic;
AudioSource gameTheme;

	// Use this for initialization
	void Start () {
	gameTheme = GetComponent<AudioSource>();
	gameTheme.PlayOneShot(themeMusic, 0.2f);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
