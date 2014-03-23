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
	
	//ship stats and resources
	public int statCrewMembers = 8;
	public int statGunPowder = 4;
	public int statAle = 50;
	public int statIntegrity = 100;
	public int statTreasure = 0;
	
	public int statMorale = 1;
	public int statEvil = 1;
	public int statWit = 1;
	public int statNotoriety = 1;
	public int statCharisma = 1;
	
	//Used for moving ship
	public Transform startMarker;		//start location
	public Transform endMarker;			//end location
	public float rotateSpeed = 5F;		//speed to travel
	private float startTime;			//time of start, used for lerp
	private float journeyLength;
	public GameObject target = null;	//destination
	public float smooth = 3.0F;
	bool newTrip = true; 			//used to indicate a new trip can be started
	public bool canSail = true; 	//whether can sail to specified destination
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
	bool questSuccess = false;
	string resultText = "";
	bool secondQuestGUIOn = false;
	
	bool questGUIOn = false;
	Quests currentQuest;
	public List<GameObject> islands;
	
	
	//moving enemy ship
	public bool enemyNear = false;
	
	// Use this for initialization
	void Start ()
	{
		islandScript = Resources.Load ("statImages/ship") as IslandScript; 
		//island = GameObject.FindGameObjectWithTag ("island"); //we need the island data
		
	}
	
	GameObject FindClosestShip() {
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
	
	
	GameObject FindClosestEnemy() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("enemy");
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
		GameObject isl = FindClosestShip();
		if (isl != null){
			distance = Vector3.Distance (transform.position, isl.transform.position);
			if (distance < 6) closeEnough = true; //we are close to an island
			if (distance > 7) closeEnough = false;
		} else closeEnough = false;


		GameObject closestE = null;
		//check if enemy exists and is close to us
		if (GameObject.FindGameObjectWithTag("enemy") != null){ 
			closestE = FindClosestEnemy ();
			distance = Vector3.Distance (transform.position, closestE.transform.position);
			if (distance < 6) enemyNear = true; //we are close to an island
			if (distance > 6) enemyNear = false;
		} 

		if (enemyNear) {
			chaseShip(closestE);
		}

		checkMouse ();

		//Debug.Log ("Cansail is " + canSail);
		if (canSail == true) {
			sail ();
			destDistance = Vector3.Distance (transform.position, flag.transform.position);
			if (destDistance == 1) destDistance = 0;
		}
	}
	
	
	// GUI
	void OnGUI ()
	{
		
		// Box for resources at bottom left
		GUI.Box(new Rect(10, 610, 400, 52), "");
		
		Texture2D statIntegrityTexture = Resources.Load ("statImages/" + "integrity") as Texture2D;
		Texture2D statGunPowderTexture = Resources.Load ("statImages/" + "cannonball") as Texture2D;
		Texture2D statCrewTexture = Resources.Load ("statImages/" + "crew") as Texture2D;
		Texture2D statAleTexture = Resources.Load ("statImages/" + "ale") as Texture2D;
		Texture2D statTreasureTexture = Resources.Load ("statImages/" + "treasure") as Texture2D;
		
		Texture2D statNotorietyTexture = Resources.Load ("statImages/" + "notoriety") as Texture2D;
		Texture2D statWitTexture = Resources.Load ("statImages/" + "wit") as Texture2D;
		Texture2D statCharismaTexture = Resources.Load ("statImages/" + "charisma") as Texture2D;
		Texture2D statEvilTexture = Resources.Load ("statImages/" + "evil") as Texture2D;
		Texture2D statMoraleTexture = Resources.Load ("statImages/" + "morale") as Texture2D;
		
		GUI.DrawTexture(new Rect(10, 612, 50, 50), statIntegrityTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.Label (new Rect(65, 625, 50, 50), ""+statIntegrity);
		
		GUI.DrawTexture(new Rect(95, 612, 50, 50), statCrewTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.Label (new Rect(150, 625, 50, 50), ""+statCrewMembers);
		
		GUI.DrawTexture(new Rect(170, 612, 50, 50), statAleTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.Label (new Rect(225, 625, 50, 50), ""+statAle);
		
		GUI.DrawTexture(new Rect(245, 612, 50, 50), statGunPowderTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.Label (new Rect(300, 625, 50, 50), ""+statGunPowder);
		
		GUI.DrawTexture(new Rect(320, 612, 50, 50), statTreasureTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.Label (new Rect(385, 625, 50, 50), ""+statTreasure);
		
		
		
		//Box for stats at bottom right
		GUI.Box(new Rect(870, 610, 400, 52), "");
		int rightBarXLocation = 870;
		GUI.DrawTexture(new Rect(rightBarXLocation += 20, 612, 50, 50), statMoraleTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.Label (new Rect(rightBarXLocation += 50, 625, 50, 50), ""+statMorale);
		
		GUI.DrawTexture(new Rect(rightBarXLocation += 20, 612, 50, 50), statEvilTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.Label (new Rect(rightBarXLocation += 50, 625, 50, 50), ""+statEvil);
		
		GUI.DrawTexture(new Rect(rightBarXLocation += 20, 612, 50, 50), statCharismaTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.Label (new Rect(rightBarXLocation += 50, 625, 50, 50), ""+statCharisma);
		
		GUI.DrawTexture(new Rect(rightBarXLocation += 20, 612, 50, 50), statNotorietyTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.Label (new Rect(rightBarXLocation += 50, 625, 50, 50), ""+statNotoriety);
		
		GUI.DrawTexture(new Rect(rightBarXLocation += 20, 612, 50, 50), statWitTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.Label (new Rect(rightBarXLocation += 50, 625, 50, 50), ""+statWit);
		
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
		GUI.Box(new Rect(10, 200, 200, 100), " ");
		GUI.Label (new Rect (40, 220, 200, 100), "Destination: " + destinationXCoordinate + ", " + destinationZCoordinate);
		GUI.Label (new Rect (40, 255, 200, 100), "Distance: " + destDistance);
		
		// My Coordinates //
		GUI.Button (new Rect (10, 100, 200, 100), "My coords: " + xCoord + " " + zCoord);
		
		// Character Sheet//
		if (GUI.Button (new Rect (10, 300, 200, 100), "Character Sheet")) {
			//showShipSheet ();
		}
		
		// Right Side Message //
		//GUI.Button (new Rect (1000, 10, 200, 400), rightSideMessage);
		
		GUI.backgroundColor = oldColor;
		
		
		if (questGUIOn){
			GUIStyle boxStyle = "box";
			boxStyle.wordWrap = true;
			GUI.Box (new Rect (400, 100, 420, 430), ""); //outer event box
			
			//quest image
			string imagename = currentQuest.eventName.ToString().TrimEnd( '\r', '\n' );
			Texture2D questTexture = Resources.Load ("questImages/" + imagename) as Texture2D;
			GUI.DrawTexture(new Rect (460, 110, 300, 220), questTexture, ScaleMode.StretchToFill, true, 10.0F);
			
			//quest text
			GUI.Label (new Rect (415, 340, 410, 50 ), currentQuest.eventText);
			
			GUI.Label (new Rect (575, 390, 200, 50 ), "Options:");
			
			//first option
			if (GUI.Button (new Rect (410, 420, 400, 40), currentQuest.optionText1 )) {
				resultHandler(1);
				
				if (questSuccess == true) {
					resultText = currentQuest.successText1;
					//results off success
					statAle += currentQuest.successAle1;
					statCharisma += currentQuest.successCharisma1;
					statCrewMembers += currentQuest.successCrew1;
					statEvil += currentQuest.successEvil1;
					statGunPowder += currentQuest.successGunpowder1;
					statIntegrity += currentQuest.successIntegrity1;
					statMorale += currentQuest.successMorale1;
					statNotoriety += currentQuest.successNotoriety1;
					statTreasure += currentQuest.successCoins1;
					statWit += currentQuest.successWit1;
					
					
				}else {
					resultText = currentQuest.failText1;
					
					//results off fail
					//results off failure
					statAle += currentQuest.failureAle1;
					statCharisma += currentQuest.failureCharisma1;
					statCrewMembers += currentQuest.failureCrew1;
					statEvil += currentQuest.failureEvil1;
					statGunPowder += currentQuest.failureGunpowder1;
					statIntegrity += currentQuest.failureIntegrity1;
					statMorale += currentQuest.failureMorale1;
					statNotoriety += currentQuest.failureNotoriety1;
					statTreasure += currentQuest.failureCoins1;
					statWit += currentQuest.failureWit1;
					
					
				}
				
				secondQuestGUIOn = true;
				questGUIOn = false;
			}
			//second option
			Debug.Log(currentQuest.optionText2);
			Debug.Log(resultText);
			if (GUI.Button (new Rect (410, 480, 400, 40), currentQuest.optionText2)) {
				
				resultHandler(2);
				
				if (questSuccess == true) {
					resultText = currentQuest.successText2;
					
					//results off success
					statAle += currentQuest.successAle2;
					statCharisma += currentQuest.successCharisma2;
					statCrewMembers += currentQuest.successCrew2;
					statEvil += currentQuest.successEvil2;
					statGunPowder += currentQuest.successGunpowder2;
					statIntegrity += currentQuest.successIntegrity2;
					statMorale += currentQuest.successMorale2;
					statNotoriety += currentQuest.successNotoriety2;
					statTreasure += currentQuest.successCoins2;
					statWit += currentQuest.successWit2;
					
				} else {
					resultText = currentQuest.failText2;
					
					//results off failure
					statAle += currentQuest.failureAle2;
					statCharisma += currentQuest.failureCharisma2;
					statCrewMembers += currentQuest.failureCrew2;
					statEvil += currentQuest.failureEvil2;
					statGunPowder += currentQuest.failureGunpowder2;
					statIntegrity += currentQuest.failureIntegrity2;
					statMorale += currentQuest.failureMorale2;
					statNotoriety += currentQuest.failureNotoriety2;
					statTreasure += currentQuest.failureCoins2;
					statWit += currentQuest.failureWit2;
					
					
				}
				
				secondQuestGUIOn = true;
				questGUIOn = false;
			}
			
		}
		
		
		
		if (secondQuestGUIOn){
			GUIStyle boxStyle = "box";
			boxStyle.wordWrap = true;
			GUI.Box (new Rect (400, 100, 420, 430), ""); //outer event box
			
			//quest image
			string imagename = currentQuest.eventName.ToString().TrimEnd( '\r', '\n' );
			Texture2D questTexture = Resources.Load ("questImages/" + imagename) as Texture2D;
			GUI.DrawTexture(new Rect (460, 110, 300, 220), questTexture, ScaleMode.StretchToFill, true, 10.0F);
			
			//quest text
			GUI.Label (new Rect (415, 340, 410, 50 ), resultText);
		}
		
	}
	
	public void showShipSheet ()
	{
		rightSideMessage =
			
			"Ship Details\n" +
				"\n"+
				"Crew Count: " +
				statCrewMembers +
				"\n" +
				"Total Morale: " +
				statMorale +
				"\n" +
				"Gun Powder: " +
				statGunPowder +
				"\n" +
				"Ale " +
				statAle +
				"\n";
	}
	
	public void investigate ()
	{
		
		/*		rightSideMessage = "island coords: " + (int)islandScript.xCoord + " " + (int)islandScript.zCoord + "\n" +
			"my coords: " + (int)xCoord + " " + (int)zCoord;*/
		
		CameraScript cScript;
		cScript = (CameraScript)FindObjectOfType(typeof(CameraScript));
		int questCount = cScript.questList.Count;
		int randomQuestNumber = Random.Range (0, questCount); //for ints
		currentQuest = cScript.questList[randomQuestNumber];
		
		questGUIOn = true;
		
		
		//quest stuff
		
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
			Debug.Log("WE CLICKED THE MOUSE");
			Vector2 mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
			
			//if clicked outside of event gui, turn it off
			if (new Rect (400, 100, 500, 500).Contains (mouse) && secondQuestGUIOn){
				canSail = false;
			} else {  
				secondQuestGUIOn = false;
			}
			
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
	
	public void resultHandler (int choice){
		int diceRoll = Random.Range (1, 100); //for ints
		
		if (choice == 1){
			if (diceRoll >= currentQuest.odds1) {
				questSuccess = true;
			} else questSuccess = false;
			
			
		}else if (choice == 2){
			if (diceRoll >= currentQuest.odds2) {
				questSuccess = true;
			} else questSuccess = false;
			
		} else { Debug.Log ("error"); }
		
		
	}
	
	
	public void chaseShip(GameObject enemy){
		Debug.Log ("ship should chase us");
		//destination is 
		//rotate enemy
		Quaternion rotation = Quaternion.LookRotation (transform.position - enemy.transform.position);
		transform.rotation = Quaternion.Slerp (enemy.transform.rotation, rotation, Time.deltaTime * rotateSpeed);
		float enemySpeed = 1.0f;
		enemy.transform.position = Vector3.Lerp (enemy.transform.position, transform.position, enemySpeed);
		float closeEnough = 3f;
		if (Vector3.Distance(transform.position, enemy.transform.position) < closeEnough){
			Debug.Log ("enemy hit us!");
			///remove ship
			Destroy (enemy);
		}
	}
	
	
}