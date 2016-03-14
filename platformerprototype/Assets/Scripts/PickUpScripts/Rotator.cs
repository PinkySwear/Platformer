using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour 
{

	void Update () 
	{
		transform.Rotate (new Vector3 (15, 15, 15) * 2 * Time.deltaTime);
	}
}
