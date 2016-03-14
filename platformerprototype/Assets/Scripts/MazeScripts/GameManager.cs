using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{

	public Maze mazePrefab;
	private Maze mazeInstance;

	public Player playerPrefab;
	private Player playerInstance;

	public EndBeacon endBeaconPrefab;
	private EndBeacon endBeaconInstance;

	public EnemyDefense enemyDefensePrefab;
	private EnemyDefense enemyDefenseInstance;

	public PickUpObject pickUpObjectPrefab;
	private PickUpObject pickUpObjectInstance;



	private void Start () 
	{
		StartCoroutine(BeginGame());
	}
	
	private void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			RestartGame();
		}

		if (enemyDefenseInstance != null) 
		{
			if (enemyDefenseInstance.isPlayerDead ()) 
			{
				RestartGame ();
			}
		}
	}

	private IEnumerator BeginGame () 
	{
		mazeInstance = Instantiate(mazePrefab) as Maze;
		yield return StartCoroutine (mazeInstance.Generate ());

		playerInstance = Instantiate (playerPrefab) as Player;
		playerInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));
		playerInstance.transform.position = new Vector3 (GameObject.FindGameObjectWithTag("Player").transform.position.x, .25f, GameObject.FindGameObjectWithTag("Player").transform.position.z);

		endBeaconInstance = Instantiate (endBeaconPrefab) as EndBeacon;
		endBeaconInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));

		pickUpObjectInstance = Instantiate (pickUpObjectPrefab) as PickUpObject;
		pickUpObjectInstance.tag = "Green Pick Up";
		pickUpObjectInstance.szin = Color.green;
		pickUpObjectInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));
		pickUpObjectInstance.transform.position = new Vector3 (GameObject.FindGameObjectWithTag("Green Pick Up").transform.position.x, .25f, GameObject.FindGameObjectWithTag("Green Pick Up").transform.position.z);



		if (playerInstance.transform.position.x == endBeaconInstance.transform.position.x)
		{
			if(playerInstance.transform.position.z == endBeaconInstance.transform.position.z)
			{
				endBeaconInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));	
			}
		}

		keyCardCheck (playerInstance);

	}

	private void keyCardCheck(Player inputPlayer)
	{
		if (inputPlayer.hasRedKey == false)
		{
			enemyDefenseInstance = Instantiate (enemyDefensePrefab) as EnemyDefense;
			enemyDefenseInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));
			enemyDefenseInstance.transform.position = new Vector3 (GameObject.FindGameObjectWithTag("Enemy").transform.position.x, .25f, GameObject.FindGameObjectWithTag("Enemy").transform.position.z);


			if (inputPlayer.transform.position.x == enemyDefenseInstance.transform.position.x)
			{
				if(inputPlayer.transform.position.z == enemyDefenseInstance.transform.position.z)
				{
					enemyDefenseInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));
					enemyDefenseInstance.transform.position = new Vector3 (GameObject.FindGameObjectWithTag("Enemy").transform.position.x, .25f, GameObject.FindGameObjectWithTag("Enemy").transform.position.z);

				}
			}

//			if (endBeaconInstance.transform.position.x == enemyDefenseInstance.transform.position.x)
//			{
//				if(endBeaconInstance.transform.position.z == enemyDefenseInstance.transform.position.z)
//				{
//					endBeaconInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));				
//				}
//			}
		}
			
	}
		

	private void RestartGame () 
	{
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);

		if (playerInstance != null)
		{
			Destroy (playerInstance.gameObject);
		}

		if(endBeaconInstance != null)
		{
			Destroy (endBeaconInstance.gameObject);
		}

		if(enemyDefenseInstance != null)
		{
			Destroy (enemyDefenseInstance.gameObject);
		}

		if(pickUpObjectInstance != null)
		{
			Destroy (pickUpObjectInstance.gameObject);
		}

		StartCoroutine(BeginGame());
	}
}