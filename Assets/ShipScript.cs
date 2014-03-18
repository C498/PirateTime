using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO; 

/*
 * use input handler, to have it work across platforms
 * success ratio -> after prototype
 * 
 */


public class ShipScript : MonoBehaviour
{
	public GameObject flag;
	float speed = .7f; // movement speed
	bool closeEnough = false; //island is close!
	public float distance; //distance between ship and nearest island (only island atm)
	
	public GameObject island;
	public IslandScript islandScript; //need to access island script
	
	public GameObject mainCamera;
	
	public float xCoord, zCoord; // ship coordinates
	public string rightSideMessage; //right GUI message 
	
	//ship stats
	public string statName = "The Manatee";
	public int statCrewNum = 0;
	public int statGunPowder = 4;
	public int statFood = 4;
	public int statAle = 4;
	public int statTotalMorale = 100;
	
	//Used for moving ship
	public Transform startMarker;		//start location
	public Transform endMarker;			//end location
	public float rotateSpeed = 5F;		//speed to travel
	private float startTime;			//time of start, used for lerp
	private float journeyLength;
	public GameObject target = null;	//destination
	public float smooth = 3.0F;
	bool newTrip = true; 			//used to indicate a new trip can be started
	public bool canSail = false; 	//whether can sail to specified destination
	public bool shouldRotate = false; //whether the ship should be rotating to face destination
	public bool startTrip = false;
	
	
	//mouse information
	public float mouseX;
	public float mouseZ;
	public Ray ray;
	public GameObject mouseTarget;
	public float destinationXCoordinate;
	public float destinationZCoordinate;
	public float destDistance;
	public Transform destination;
	public bool mouseOverGui = false;
	//questInfo
	string questEventName;
	
	bool questGUIOn = false;
	Quests currentQuest;
	public List<GameObject> islands;


	// Use this for initialization
	void Start ()
	{
		islandScript = GameObject.Find ("island").GetComponent<IslandScript> ();
		island = GameObject.FindGameObjectWithTag ("island"); //we need the island data

	}

	GameObject FindClosestEnemy() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("island");
		GameObject closest=null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}
	
	// Update is called once per frame
	void Update ()
	{
		xCoord = gameObject.transform.position.x;
		zCoord = gameObject.transform.position.z;

		//DISTANCE CHECKER IF ANY ISLANDS NEAR? (EARLY MECHANIC)
		distance = Vector3.Distance (transform.position, FindClosestEnemy().transform.position);
		if (distance < 6) closeEnough = true; //we are close to an island
		if (distance > 7) closeEnough = false;
		checkMouse ();
		if (canSail == true) {
			sail (); //go there
			destDistance = Vector3.Distance (transform.position, flag.transform.position); //distance to dest
			if (destDistance == 1) destDistance = 0;
		}
	}
	
	
	// GUI
	void OnGUI ()
	{
		Vector2 mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		var oldColor = GUI.backgroundColor;
		// turn GUI white if we can 'investigate'
		if (closeEnough == true) {
			GUI.color = Color.white;
			if (GUI.Button (new Rect (10, 10, 200, 100), "Investigate " + distance)) {
				investigate ();
			}
		}
		if (closeEnough == false) {
			GUI.color = Color.red;
		}
		// INVESTIGATE BUTTON 
		/*if (GUI.Button (new Rect (10, 10, 200, 100), "Investigate " + distance)) {
			investigate ();
		}*/
		
		//GUI.backgroundColor = Color.white;
		GUI.color = Color.white;
		GUI.contentColor = Color.white;
		
		// Navigation Tool //
		Rect r2 = new Rect(10, 200, 200, 100);
		GUI.Box(r2, " ");
		GUI.Label (new Rect (40, 220, 200, 100), "Destination: " + destinationXCoordinate + ", " + destinationZCoordinate);
		GUI.Label (new Rect (40, 255, 200, 100), "Distance: " + destDistance);
		
		// My Coordinates //
		GUI.Button (new Rect (10, 100, 200, 100), "My coords: " + xCoord + " " + zCoord);
		
		// Character Sheet//
		if (GUI.Button (new Rect (10, 300, 200, 100), "Character Sheet")) {
			//showShipSheet ();
		}
		
		// Right Side Message //
		GUI.Button (new Rect (1000, 10, 200, 400), rightSideMessage);
		
		GUI.backgroundColor = oldColor;

		
		if (questGUIOn){
			GUIStyle boxStyle = "box";
			boxStyle.wordWrap = true;
			GUI.Box (new Rect (400, 100, 500, 500), currentQuest.eventText, boxStyle);
			string imagename = currentQuest.eventName.ToString().TrimEnd( '\r', '\n' );
			Texture2D eventTexture = Resources.Load (imagename) as Texture2D;
			GUI.Box (new Rect(450, 150, 400, 314), eventTexture, boxStyle);
			GUI.Button (new Rect (470, 490, 42, 22), "Fight" );
			GUI.Button (new Rect (590, 490, 42, 22), "Flee" );
		}
		
	}
	
	public void showShipSheet ()
	{
		rightSideMessage =
			
			"Ship Details\n" +
				"\n" +
				"Ship Name: " +
				statName +
				"\n" +
				"Crew Count: " +
				statCrewNum +
				"\n" +
				"Total Morale: " +
				statTotalMorale +
				"\n" +
				"Gun Powder: " +
				statGunPowder +
				"\n" +
				"Food: " +
				statFood +
				"\n" +
				"Ale " +
				statAle +
				"\n";
	}
	
	public void investigate ()
	{
		
		rightSideMessage = "island coords: " + (int)islandScript.xCoord + " " + (int)islandScript.zCoord + "\n" +
			"my coords: " + (int)xCoord + " " + (int)zCoord;
		
		CameraScript cScript;
		cScript = (CameraScript)FindObjectOfType(typeof(CameraScript));
		int questCount = cScript.questList.Count;
		int randomQuestNumber = Random.Range (0, questCount); //for ints
		currentQuest = cScript.questList[randomQuestNumber];
		
		questGUIOn = true;
		//add quest to gui. 
		
	}
	
	public void sail () //will move the ship from its current location to a user specified one
	{
		//move flag to coordinates of destination
		flag.transform.position = new Vector3 (destinationXCoordinate, 0, destinationZCoordinate);
		
		//rotate
		if (shouldRotate) {
			target = flag;
			Quaternion rotation = Quaternion.LookRotation (target.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotateSpeed);
		}
		
		// set up variables used for trip
		if (newTrip) {
			startTime = Time.time;
			startMarker = this.transform;
			endMarker = flag.transform;
			journeyLength = Vector3.Distance (startMarker.position, endMarker.position);
			newTrip = false;
			startTrip = true;
		} 
		
		if (startTrip) {
			float distanceCovered = (Time.time - startTime); //keeps track of distance covered
			float fracJourney = distanceCovered / journeyLength * speed;	//fraction of journey completed
			transform.position = Vector3.Lerp (startMarker.position, endMarker.position, fracJourney);
			float closeEnough = 2f;
			if (Vector3.Distance(this.transform.position, flag.transform.position) < closeEnough){
				canSail = false;
				shouldRotate = false;
				startTrip = false;
			}
		}
		
		
	}
	
	public void checkMouse ()
	{
		
		
		if (Input.GetMouseButtonDown (0) ) {
			Vector2 mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
			
			//if clicked outside of event gui, turn it off
			if (new Rect (400, 100, 500, 500).Contains (mouse) && questGUIOn){
				canSail = false;
			} else { questGUIOn = false; }
			
			//if clicked outside of gui, sail there.
			if (new Rect (10, 10, 200, 400).Contains (mouse)){
				canSail = false;
			} else {
				if (questGUIOn == false) {
					ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, 100)) {
						
						if (hit.collider.tag == "island") {
							Debug.Log ("YOU CLICKED THE ISLAND YAY!)");
						}
						
						mouseX = hit.point.x;
						mouseZ = hit.point.z;
						
						if (mouseOverGui == false){
							canSail = true;
							shouldRotate = true; //if player clicked a location, should rotate ship towards
							newTrip = true; //if player clicked a location, will make a new trip
						}
						destinationXCoordinate = Mathf.Round(mouseX * 100f) / 100f;
						destinationZCoordinate = Mathf.Round(mouseZ * 100f) / 100f;
						
					}}
			}
			
		}    
		
		
	}
	
	
	
}