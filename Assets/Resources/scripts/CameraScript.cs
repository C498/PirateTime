using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO; 

//idea shallow water.
public class CameraScript : MonoBehaviour {
	Vector3 pos;
	public Vector3 pos_original = Camera.main.transform.position;
	public Vector3 ship_coords = GameObject.Find("Object_1").transform.position;
	
	//level variables
	public int level = 1;
	public GameObject ship;
	public ShipScript shipScript;
	public GameObject lighthouse;
	public Transform islandPrefab; 
	public Transform enemyPrefab; 
	public Transform testIsland;
	public List<Quests> questList;

	// Use this for initialization
	void Start () {
		Debug.Log("Game start!");
		shipScript = GameObject.Find ("Ship").GetComponent<ShipScript>();
		ship = GameObject.FindGameObjectWithTag ("ship");
		ship = GameObject.FindGameObjectWithTag ("lighthouse");

		questList = new List<Quests>();
		generateQuests();
		generateLevel(level);
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void generateQuests() {
		//create a list of quests from the events file. 
		TextAsset file = (TextAsset)Resources.Load ("events", typeof(TextAsset));
		string questsText = file.text;
		int propertyCount = 0;
		string currentQuestProperty = "";
		Quests currentQuest = new Quests();
		int totalQuestCount=0;
		
		foreach( char c in questsText ) {
			if ( propertyCount < 75){ //if 70 properties added, then new event must be added to list
				if ( c == '~') {
					if (propertyCount == 1){ 
						currentQuest = new Quests();
						currentQuest.eventName = currentQuestProperty; }
					if (propertyCount == 2){ currentQuest.eventText = currentQuestProperty; }
					if (propertyCount == 3){ currentQuest.optionText1 = currentQuestProperty; }
					if (propertyCount == 4){ currentQuest.successText1 = currentQuestProperty; }
					if (propertyCount == 5){ currentQuest.failText1 = currentQuestProperty; }
					if (propertyCount == 6){ currentQuest.odds1 = System.Convert.ToInt32(currentQuestProperty) ; }
					if (propertyCount == 7){ currentQuest.oddsWit1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 8){ currentQuest.oddsMorale1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 9){ currentQuest.oddsNotoriety1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 10){ currentQuest.oddsCharisma1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 11){ currentQuest.oddsEvil1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 12){ currentQuest.oddsCrew1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 13){ currentQuest.oddsIntegrity1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 14){ currentQuest.oddsAle1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 15){ currentQuest.oddsGunpowder1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 16){ currentQuest.oddsCoins1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 17){ currentQuest.successWit1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 18){ currentQuest.successMorale1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 19){ currentQuest.successNotoriety1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 20){ currentQuest.successCharisma1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 21){ currentQuest.successEvil1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 22){ currentQuest.successCrew1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 23){ currentQuest.successIntegrity1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 24){ currentQuest.successAle1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 25){ currentQuest.successGunpowder1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 26){ currentQuest.successCoins1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 27){ currentQuest.failureWit1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 28){ currentQuest.failureMorale1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 29){ currentQuest.failureNotoriety1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 30){ currentQuest.failureCharisma1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 31){ currentQuest.failureEvil1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 32){ currentQuest.failureCrew1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 33){ currentQuest.failureIntegrity1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 34){ currentQuest.failureAle1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 35){ currentQuest.failureGunpowder1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 36){ currentQuest.failureCoins1 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 37){ 
						//currentQuestProperty == "true"
						if (currentQuestProperty == "True" ) {currentQuest.makeFight1 = true;}
						else currentQuest.makeFight1 = false;
					}
					if (propertyCount == 38){ currentQuest.fightDifficulty1 = currentQuestProperty; }
					
					//change below
					if (propertyCount == 39){ currentQuest.optionText2 = currentQuestProperty; }
					if (propertyCount == 40){ currentQuest.successText2 = currentQuestProperty; }
					if (propertyCount == 41){ currentQuest.failText2 = currentQuestProperty; }
					if (propertyCount == 42){ currentQuest.odds2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 43){ currentQuest.oddsWit2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 44){ currentQuest.oddsMorale2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 45){ currentQuest.oddsNotoriety2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 46){ currentQuest.oddsCharisma2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 47){ currentQuest.oddsEvil2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 48){ currentQuest.oddsCrew2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 49){ currentQuest.oddsIntegrity2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 50){ currentQuest.oddsAle2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 51){ currentQuest.oddsGunpowder2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 52){ currentQuest.oddsCoins2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 53){ currentQuest.successWit2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 54){ currentQuest.successMorale2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 55){ currentQuest.successNotoriety2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 56){ currentQuest.successCharisma2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 57){ currentQuest.successEvil2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 58){ currentQuest.successCrew2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 59){ currentQuest.successIntegrity2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 60){ currentQuest.successAle2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 61){ currentQuest.successGunpowder2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 62){ currentQuest.successCoins2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 63){ currentQuest.failureWit2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 64){ currentQuest.failureMorale2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 65){ currentQuest.failureNotoriety2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 66){ currentQuest.failureCharisma2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 67){ currentQuest.failureEvil2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 68){ currentQuest.failureCrew2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 69){ currentQuest.failureIntegrity2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 70){ currentQuest.failureAle2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 71){ currentQuest.failureGunpowder2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 72){ currentQuest.failureCoins2 = System.Convert.ToInt32( currentQuestProperty) ; }
					if (propertyCount == 73){ 
						//currentQuestProperty == "true"
						if (currentQuestProperty == "True" ) {currentQuest.makeFight2 = true;}
						else currentQuest.makeFight2 = false; }
					if (propertyCount == 74){ currentQuest.fightDifficulty2 = currentQuestProperty; }
					
					currentQuestProperty=" "; //clear the property
					propertyCount++;
					
				} else { 
					//append character to string
					currentQuestProperty = currentQuestProperty + c;
					if (c == '^'){ //ignore text before semicolon
						currentQuestProperty = "";
					}	
				}	
			}

			if (propertyCount == 75){
				//currentQuest holds a fully populated quest, lets add it to the list!
				totalQuestCount++;
				Debug.Log ("ADDING A QUEST No. " + totalQuestCount + " with eventname " + currentQuest.eventName);
				questList.Add (currentQuest);
				propertyCount = 1;
				
				if (propertyCount == 0) propertyCount++;
			}
		}
	}
	
	void generateLevel(int levelNumber) {
		//put player ship in random location
		int shipStartX = Random.Range (-20, 20); 
		int shipStartZ = Random.Range (-20, 20);
		Vector3 position = new Vector3 (shipStartX, 0, shipStartZ);
		ship.transform.position = position;
		
		//generate random islands
		int islandCount = Random.Range (6, 10);
		int count = 0;
		while (count < islandCount){
			int islandStartX = Random.Range (-20, 20); 
			int islandStartZ = Random.Range (-20, 20);
			Vector3 p = new Vector3 (islandStartX, 0, islandStartZ);
			bool closeEnough = false;
			GameObject isl = null;
			isl = FindClosestObject(p); //find closest island to new point p
			if (isl != null){
				Debug.Log ("Island exists, lets see if it's too close");
				float distance = Vector3.Distance (p, isl.transform.position);
				
				if (distance < 15) closeEnough = true;
				if (distance > 15) closeEnough = false;
				
				if (closeEnough){
					//already an island at point p
					//Debug.Log("ISLAND WAS NOT CREATED! Island already in that spot");
				}else {
					//Debug.Log ("ISLAND WAS CREATED! Islands exist, but not in that spot");
					Vector3 islandposition = new Vector3 (islandStartX, 0, islandStartZ);
					GameObject.Instantiate(islandPrefab,islandposition,Quaternion.identity);
					count++;
				}
				
			} else {
				//Debug.Log("Islands dont exist yet");
				//no islands exist yet, we can put an island there
				Vector3 islandposition = new Vector3 (islandStartX, 0, islandStartZ);
				GameObject.Instantiate(islandPrefab,islandposition,Quaternion.identity);
				count++;
			}
		}
		
		
		//generate random enemies
		int enemyCount = Random.Range (6, 10);
		count = 0;
		while (count < enemyCount){
			int enemyStartX = Random.Range (-20, 20); 
			int enemyStartZ = Random.Range (-20, 20);
			Vector3 p = new Vector3 (enemyStartX, 0, enemyStartZ);
			bool closeEnough = false;
			GameObject e = null;
			e = FindClosestObject(p); //find closest island to new point p
			if (e != null){
				float distance = Vector3.Distance (p, e.transform.position);
				if (distance < 4) closeEnough = true;
				if (distance > 4) closeEnough = false;
				
				if (closeEnough){
					//already an island at point p
					//Debug.Log("enemy WAS NOT CREATED! something already in that spot");
				}else {
					//Debug.Log ("enemy WAS CREATED! Islands exist, but not in that spot");
					Vector3 enemyPosition = new Vector3 (enemyStartX, 0, enemyStartZ);
					GameObject.Instantiate(enemyPrefab,enemyPosition,Quaternion.identity);
					count++;
				}
				
			} else {
				//no islands exist yet, we can put an island there
				Vector3 enemyPosition = new Vector3 (enemyStartX, 0, enemyStartZ);
				GameObject.Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
				count++;
			}
		}

		/*
		Debug.Log("Gonna put the lighthouse somewhere");
		bool done = false;
		while (!done){
			//randomly generate lighthouse
			int lighthouseX = Random.Range (-20, 20); 
			int lighthouseZ = Random.Range (-20, 20);
			Vector3 l = new Vector3 (lighthouseX, 0, lighthouseZ);
			
			bool close = false;
			GameObject otherObject = null;
			otherObject = FindClosestObject(l); //find closest island to new point p
			
			if (otherObject != null){
				float distance = Vector3.Distance (l, otherObject.transform.position);
				if (distance < 4) close = true;
				if (distance > 4) close = false;
				
				if (close == false){
					done = true;
					lighthouse.transform.position = l;
				}
			}
		}*/

	}
	
	
	//find closest island to the randomly generated point and return it
	GameObject FindClosestObject(Vector3 p) {
		Debug.Log("FINDING THE CLOSEST ISLAND");
		GameObject[] gos1;
		gos1 = GameObject.FindGameObjectsWithTag("island");
		GameObject[] gos2;
		gos2 = GameObject.FindGameObjectsWithTag("ship");
		GameObject[] gos3;
		gos3 = GameObject.FindGameObjectsWithTag("enemy");
		//if (gos == null) return null;
		GameObject closest = null;
		
		float distance = Mathf.Infinity;
		foreach (GameObject go in gos1) {
			Vector3 diff = go.transform.position - p;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		
		foreach (GameObject go2 in gos2) {
			Vector3 diff = go2.transform.position - p;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go2;
				distance = curDistance;
			}
		}
		
		foreach (GameObject go3 in gos3) {
			Vector3 diff = go3.transform.position - p;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go3;
				distance = curDistance;
			}
		}
		
		return closest;
	}
	
	
	
}
