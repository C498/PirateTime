using UnityEngine;
using System.Collections;

public class ShipScript : MonoBehaviour
{
	public GameObject flag;
	float speed = .7f; // movement speed
	bool closeEnough = false; //island is close!
	public float distance; //distance between ship and nearest island (only island atm)
	
	public GameObject island;
	public IslandScript islandScript; //need to access island script
	
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
	public int destDistance;
	public Transform destination;
	
	// Use this for initialization
	void Start ()
	{
		islandScript = GameObject.Find ("island").GetComponent<IslandScript> ();
		island = GameObject.FindGameObjectWithTag ("island"); //we need the island data
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		xCoord = gameObject.transform.position.x;
		zCoord = gameObject.transform.position.z;
		
		//INPUTS. WILL CHANGE. NEED TO CREATE NAVIGATION SYSTEM
		if (Input.GetKey ("w")) {
			transform.Translate (-speed * Vector3.back * Time.deltaTime);
		}
		
		if (Input.GetKey ("s")) {	
			transform.Translate (speed * Vector3.back * Time.deltaTime);
		}
		
		if (Input.GetKey ("a")) {
			
			transform.Translate (-speed * Vector2.right * Time.deltaTime);
		}
		
		if (Input.GetKey ("d")) {
			
			transform.Translate (speed * Vector2.right * Time.deltaTime);
		}
		
		//DISTANCE CHECKER IF ANY ISLANDS NEAR? (EARLY MECHANIC)
		distance = Vector3.Distance (transform.position, island.transform.position);
		
		
		if (distance < 6) {
			closeEnough = true; //we are close to an island
		}
		if (distance > 7) {
			closeEnough = false;	
		}
		
		
		//check for mouse click, if clicked, get and save coordinates.
		checkMouse ();
		
		if (canSail == true) { //if coordinates were entered that exist, then the ship will set sail to it
			
			sail (); //go there
			destDistance = (int)Vector3.Distance (transform.position, flag.transform.position); //distance to dest
			if (destDistance == 1) destDistance = 0;
		}
		
		
		
		
		
		
	}
	
	
	// GUI
	void OnGUI ()
	{
		
		
		
		
		var oldColor = GUI.backgroundColor;
		
		// turn GUI white if we can 'investigate'
		if (closeEnough == true) {
			GUI.color = Color.white;
		}
		if (closeEnough == false) {
			GUI.color = Color.red;
		}
		// INVESTIGATE BUTTON 
		if (GUI.Button (new Rect (10, 10, 200, 100), "Investigate " + distance)) {
			investigate ();
		}
		
		
		//GUI.backgroundColor = Color.white;
		GUI.color = Color.white;
		GUI.contentColor = Color.white;
		
		
		
		// Navigation Tool //
		GUI.Box (new Rect (10, 200, 200, 100), " ");
		GUI.Label (new Rect (40, 220, 200, 100), "Destination: " + destinationXCoordinate + ", " + destinationZCoordinate);
		GUI.Label (new Rect (40, 255, 200, 100), "Distance: " + destDistance);
		
		
		
		// My Coordinates //
		GUI.Button (new Rect (10, 100, 200, 100), "My coords: " + (int)xCoord + " " + (int)zCoord);
		
		if (GUI.Button (new Rect (10, 300, 200, 100), "Character Sheet")) {
			showShipSheet ();
		}
		
		GUI.Button (new Rect (780, 10, 200, 400), rightSideMessage);
		
		GUI.backgroundColor = oldColor;
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
		
	}
	
	public void sail () //will move the ship from its current location to a user specified one
	{

		Debug.Log ("in sail");
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
			float distanceCovered = (Time.time - startTime) * speed; //keeps track of distance covered
			float fracJourney = distanceCovered / journeyLength;	//fraction of journey completed
			
			//sail
			transform.position = Vector3.Lerp (startMarker.position, endMarker.position, fracJourney);

			//check if close ENOUGH to destination, if so, stop
			if (this.transform.position.z <= flag.transform.position.z - 1 && this.transform.position.z >= flag.transform.position.z + 1) {
				if (this.transform.position.x <= (flag.transform.position.x  - 1) && this.transform.position.x >= (flag.transform.position.x + 1)) {
					{
						canSail = false;
						shouldRotate = false;
						startTrip = false;
						
					}
				}
			}
		}
	}
	
	public void checkMouse ()
	{
		if (Input.GetMouseButtonDown (0)) {
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 100)) {
				
				if (hit.collider.tag == "island") {
					Debug.Log ("YOU CLICKED THE ISLAND YAY!)");
				}
				
				mouseX = hit.point.x;
				mouseZ = hit.point.z;
				Debug.Log (mouseX);
				Debug.Log (mouseZ);
				
				canSail = true;
				shouldRotate = true;
				newTrip = true;
				
				mouseX = Mathf.Round(mouseX * 100f) / 100f;
				mouseZ = Mathf.Round(mouseZ * 100f) / 100f;
				destinationXCoordinate = (int)mouseX;
				destinationZCoordinate = (int)mouseZ;
			}    
		}    
		
		
	}
	
	
	
}