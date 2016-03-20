//This is free to use and no attribution is required
//No warranty is implied or given
using UnityEngine;
using System.Collections;
 
[RequireComponent (typeof(LineRenderer))]
 
public class LaserBeam : MonoBehaviour {
   
    public float laserWidth;
    public float noise;
    public float maxLength;
	public bool vertical;
    public Color color = Color.red;
	public GameObject droid2;
	public GameObject dog;
	
	private AudioSource hitSound;
	private DogControls dogC;
	private float attackCD;
	private bool intruderDetected = false;
	
	bool wait;

   
    LineRenderer lineRenderer;
    int length;
    Vector3[] position;
    //Cache any transforms here
    Transform myTransform;
    Transform endEffectTransform;
    //The particle system, in this case sparks which will be created by the Laser
    //public ParticleSystem endEffect;
    Vector3 offset;
   
   
    // Use this for initialization
    void Start () {
		dogC = dog.GetComponent<DogControls> ();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetWidth(laserWidth, laserWidth);
		//lineRenderer.SetColor(color, color);
		//lineRenderer.SetPosition(0, transform.position);
        myTransform = transform;
        offset = new Vector3(0,0,0);
		//lineRenderer.SetPosition(1, offset);
		hitSound = GetComponent<AudioSource> ();
        //endEffect = GetComponentInChildren<ParticleSystem>();
        //if(endEffect)
        //    endEffectTransform = endEffect.transform;
		attackCD = 1f;
    }
   
    // Update is called once per frame
    void Update () {
        RenderLaser();
		StartCoroutine(attack (0.25f));
    }
   
    void RenderLaser(){
       
        //Shoot our laserbeam forwards!
        UpdateLength();
       
        lineRenderer.SetColors(color,color);
        //Move through the Array
        if(vertical){
			offset.x =myTransform.position.x+Random.Range(-noise,noise);
			offset.y =myTransform.position.y-maxLength;
        }
		else{
			offset.y =myTransform.position.y+Random.Range(-noise,noise);
		}
		offset.z =Random.Range(-noise,noise)+myTransform.position.z;
		Debug.DrawRay (transform.position, offset, Color.red);   
		Vector3 dogDirection = dog.GetComponent<Transform>().position - transform.position;
	    Debug.DrawRay (transform.position, dogDirection.normalized * 10f, Color.green);
		//lineRenderer.SetPosition(0, transform.position);
		//lineRenderer.SetPosition(1, offset);
	   
       
    }
   
    void UpdateLength(){
        //Raycast from the location of the cube forwards
		//Debug.Log(offset);
        //hit = Physics.RaycastAll(myTransform.position, offset, maxLength);
		RaycastHit rh;
		Vector3 dogDirection = dog.GetComponent<Transform>().position - transform.position;
		if (Physics.Raycast (myTransform.position, offset, out rh, 10f, ~(1 << LayerMask.NameToLayer ("Interactable") | 1 <<LayerMask.NameToLayer("Enemy")))) {
			if (rh.collider.tag == "Dog") {
				//Debug.Log("INTRUDER DETECTED");
				intruderDetected = true;
			}
			else{
				intruderDetected = false;
			}
		}
		else{
			intruderDetected = false;
		}
        lineRenderer.SetVertexCount(length);
    }
	
	IEnumerator attack (float second) {
		
		if (wait) yield break;
		wait = true;
		yield return new WaitForSeconds(second);
		wait = false;
		if(intruderDetected){
			hitSound.Play ();
			Debug.Log("INTRUDER DETECTED");
			dogC.takeDamage (1);
		}
	
	}
}